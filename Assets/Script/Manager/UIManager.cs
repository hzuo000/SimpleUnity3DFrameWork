using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System;

public enum UIType
{
    /// <summary>
    /// 常规页面【(可能)入栈】
    /// </summary>
    Normal = 0,

    /// <summary>
    /// 固定窗口(类似HUD)【不入栈】
    /// </summary>
    Fixed,

    /// <summary>
    /// 弹窗【不入栈】
    /// </summary>
    PopUp,

}
/// <summary>
/// 当前UI的类型【会作用于打开下一个界面时】
/// </summary>
public enum UIMode
{
    /// <summary>
    /// 什么都不做【不入栈】
    /// </summary>
    DoNothing,

    /// <summary>
    /// 独占全屏，打开下个界面关闭前面栈里所有的界面【入栈】
    /// </summary>
    HideOther,
    /// <summary>
    /// 兼容界面，打开下个界面不关闭其他界面(需要调整好层级关系)【入栈】
    /// </summary>
    NeedBack,

    /// <summary>
    /// 关闭其他界面【不入栈】【todo：未实现】
    /// </summary>
    NoNeedBack,
}
/// <summary>
/// ui显示节点[ui的组件实例实际存储的类]
/// </summary>
public class PanelNode
{
    public GObject gGameobject;
    public string ctrlName = string.Empty;
    public string panelName = string.Empty;
    public int sortingOrder = 0;
    public UIMode mode = UIMode.DoNothing;
    public UIType type = UIType.Normal;


    public PanelNode(UIType type, UIMode mode, string ctrlName, string panelName)
    {
        this.type = type;
        this.mode = mode;
        this.ctrlName = ctrlName;
        this.panelName = panelName;
    }

    public bool IsActive()
    {
        if (gGameobject == null || gGameobject.displayObject == null || gGameobject.displayObject.gameObject == null)
        {
            return false;
        }
        
        return gGameobject.displayObject.gameObject.activeSelf;
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            GRoot.inst.AddChild(gGameobject);
        }
        else
        {
            GRoot.inst.RemoveChild(gGameobject);
        }
    }
}
/// <summary>
/// ui控制器管理器【mvc模式】[数据放在controller的派生类中，显示的逻辑写在view的派生类中]
/// </summary>
public class UIControllerManager
{
    private Dictionary<string, IUIController> controllerDic;// 所有控制器<名字，控制器>

    private Dictionary<string, string> viewToControllerDic;//页面对应控制器名字<view名字，controller名字>

    private static UIControllerManager _this;
    public static UIControllerManager Inst { get => _this; }

    public UIControllerManager()
    {
        _this = this;
        controllerDic = new Dictionary<string, IUIController>();
        viewToControllerDic = new Dictionary<string, string>();
    }
    /// <summary>
    /// 通过名字获取控制器(返回对象实例)
    /// </summary>
    /// <param name="ctrlName"></param>
    /// <returns></returns>
    public T GetControllerByName<T>(string ctrlName) where T : UIControllerBase
    {
        T local = default;
        if (controllerDic.ContainsKey(ctrlName))
        {
            return controllerDic[ctrlName] as T;
        }
        return local;

    }
    /// <summary>
    /// 通过名字获取控制器（返回控制器接口类型）
    /// </summary>
    /// <param name="ctrlName"></param>
    /// <returns></returns>
    public IUIController GetControllerByName(string ctrlName)
    {
        if (controllerDic.ContainsKey(ctrlName))
        {
            return controllerDic[ctrlName];
        }
        return null;
    }
    /// <summary>
    /// 通过类型获取控制器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetController<T>() where T : IUIController
    {
        var it = controllerDic.GetEnumerator();
        while (it.MoveNext())
        {
            IUIController controller = it.Current.Value;
            if (controller.GetType()==typeof(T))
            {
                return (T)controller;
            }
        }
        return default;
    }
    /// <summary>
    /// 用view获得ctr名字
    /// </summary>
    /// <param name="viewName">页面名称</param>
    /// <returns></returns>
    public string GetControllerName(string viewName)
    {
        if (viewToControllerDic.ContainsKey(viewName))
        {
            return viewToControllerDic[viewName];
        }
        return null;
    }
    /// <summary>
    /// 创建控制器
    /// </summary>
    /// <param name="ctrlName"></param>
    public void CreateController<T>() where T: IUIController , new()
    {
        IUIController ctr = new T();
        controllerDic.Add(ctr.ControllerName, ctr);
        ctr.Init((viewName,ctrName)=> {
            viewToControllerDic.Add(viewName, ctrName);
        });
    }
    
}

/// <summary>
/// UI总管理：【打开/关闭界面逻辑：open/close->(组件实例)node出入栈->(消息)通知对应的controller处理数据->(消息)通知对应的view更新界面显示数据】
/// </summary>
public class UIManager : GameInterface
{
    /// <summary>
    /// UI控制器【控制逻辑】
    /// </summary>
    public UIControllerManager UIController { get; private set; }

    private  int openPanelCount = 0;//当前打开的界面数量
    private const int intervalOrder = 500;//页面类型打开层级间隔
    private const int StepOrder = 2;//每打开一个页面向后增加的层级

    private List<PanelNode> panelNodes;//UI栈
    private Dictionary<string, PanelNode> panelsDict;//所有已经创建的界面<名称，实例>
    public override void StartUp()
    {
        panelNodes = new List<PanelNode>();
        panelsDict = new Dictionary<string, PanelNode>();
        UIController = new UIControllerManager();
        InitController();
        InitAction();
        SetScreenSize();
        ShowPanel(UIComponentName.LoadingMainView, UIType.Normal, UIMode.DoNothing);
        base.StartUp();
    }
    private void SetScreenSize()
    {
        GRoot.inst.SetContentScaleFactor(1080,1920, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
    }
    /// <summary>
    /// 初始化所有UI的控制器
    /// </summary>
    private void InitController()
    {
        UIController.CreateController<StartUpController>();
        UIController.CreateController<MainUIController>();
        UIController.CreateController<StageHUDController>();
    }
    private void InitAction()
    {
        GameManager.Observer.RegisterHandler(new Dictionary<ComponentMessage, Action<Message>>()
        {
            {ComponentMessage.ClearCurrentUI, HandelMSG_OnSceneUIRelease},
            {ComponentMessage.LoadNewSceneUI, HandelMSG_OnLoadNewSceneUI},

        }) ;
    }
    private void HandelMSG_OnSceneUIRelease(Message message) => ReleaseUIRoot();
    private void HandelMSG_OnLoadNewSceneUI(Message message)
    {
        string newSceneName = default;
        if (message.Contains("newSceneName"))
        {
            newSceneName = message["newSceneName"] as string;
        }
        switch (newSceneName)
        {
            case SceneName.StartUp:
                ShowPanel(UIComponentName.LoadingMainView, UIType.Normal, UIMode.DoNothing);
                break;
            case SceneName.MainUI:
                ShowPanel(UIComponentName.MainUIMainView, UIType.Normal, UIMode.NeedBack);
                ShowPanel(UIComponentName.MainHUD, UIType.Fixed, UIMode.DoNothing);
                break;
            case SceneName.Stage:
                ShowPanel(UIComponentName.StageHUD, UIType.Fixed, UIMode.DoNothing);
                break;
        }
    }
    /// <summary>
    /// 打开ui
    /// </summary>
    /// <param name="panelName"></param>
    /// <param name="type"></param>
    /// <param name="mode"></param>
    /// <param name="data"></param>
    public void ShowPanel(string panelName,UIType type,UIMode mode,object data = null)
    {
        PanelNode node;
        if (panelsDict.ContainsKey(panelName))//创建过这个view
        {
            node = panelsDict[panelName];
            node.mode = mode;
            SetPanelSortingOrding(node);
            PushNode(node, type);
            ShowPanelEvent(panelName, data);
        }
        else//没有加载这个界面，需要先创建
        {
            CreatPanel(panelName, type, mode,
                _node =>
                {
                    SetPanelSortingOrding(_node);
                    PushNode(_node, type);
                    ShowPanelEvent(panelName, data);
                });
        }
    }
    /// <summary>
    /// 刷新页面
    /// </summary>
    /// <param name="panelName"></param>
    public void FreshView(string panelName,string data)
    {
        if (!panelsDict.ContainsKey(panelName))
        {
            return;
        }
        IUIController controller = UIControllerManager.Inst.GetControllerByName(panelName);
        if (controller!=null)
        {
            controller.FreshView(panelName, data);
        }
    }
    /// <summary>
    /// 关闭界面（返回上个界面）
    /// </summary>
    /// <param name="panelName"></param>
    /// <param name="data"></param>
    public void ClosePanel(string panelName,object data = null)
    {
        if (panelsDict.ContainsKey(panelName))
        {
            PopNode(panelsDict[panelName], true, data);
        }
    }
    /// <summary>
    /// 关闭界面（不返回上个界面） 
    /// </summary>
    /// <param name="panelName"></param>
    public void CloseCurrentPanel(string panelName)
    {
        if (panelsDict.ContainsKey(panelName))
        {
            PopNode(panelsDict[panelName], false, null);
        }
    }
    /// <summary>
    /// 设置渲染层级
    /// </summary>
    private void SetPanelSortingOrding(PanelNode node)
    {
        node.sortingOrder = ((int)node.type * intervalOrder) + (++openPanelCount) * StepOrder;
        node.gGameobject.sortingOrder = node.sortingOrder;
    }
    /// <summary>
    /// ui入栈
    /// </summary>
    /// <param name="node">入栈节点</param>
    /// <param name="type">入栈类型</param>
    private void PushNode(PanelNode node,UIType type)
    {
        if (node == null)
        {
            Debug.LogError("入栈ui为null");
            return;
        }
        if (CheckIfNeedBack(node))
        {
            HideOtherNode(node.panelName);
            //如果栈里有这个界面，就直接放到栈顶
            if (panelNodes.Contains(node))
            {
                panelNodes.Remove(node);
            }
            panelNodes.Add(node);
        }
        //不需要返回的界面就不用放入栈
        
    }
    /// <summary>
    /// 判断这个界面是不是需要入栈
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    private bool CheckIfNeedBack(PanelNode page)
    {
        if (page.type == UIType.Fixed || page.type == UIType.PopUp )
        {
            return false;
        }
        else if (page.mode == UIMode.NoNeedBack || page.mode == UIMode.DoNothing)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// 如果上个打开的界面是独占全屏。则把栈里的其他界面全部关闭了【todo:这里没看懂什么意思】
    /// </summary>
    /// <param name="curPageName">当前打开界面名称</param>
    private void HideOtherNode(string curPageName)
    {
        if (panelNodes.Count < 1)
        {
            return;
        }
        PanelNode topNode = panelNodes[panelNodes.Count - 1];
        if (topNode.mode== UIMode.HideOther)
        {
            for (int i = panelNodes.Count - 1; i >= 0; --i) 
            {
                PanelNode panel = panelNodes[i];
                if (panel.IsActive() && !panel.panelName.Equals(curPageName))
                {
                    ClosePanelEvent(panel);
                }
            }
        }
    }
    /// <summary>
    /// ui出栈
    /// </summary>
    /// <param name="node"></param>
    /// <param name="isShowLastView">是否显示上一个入栈的ui</param>
    /// <param name="data"></param>
    private void PopNode(PanelNode node , bool isShowLastView, object data)
    {
        if (node == null)
        {
            return;
        }
        //没有活动的页面直接移除
        if (panelNodes.Contains(node) && !node.IsActive())
        {
            panelNodes.Remove(node);
            return;
        }
        //如果关闭的是最上面的那个界面且需要返回且有可以返回的界面，则返回上一个界面
        if (panelNodes.Count >= 1 && panelNodes[panelNodes.Count - 1] == node) 
        {
            if (panelNodes.Count>1 && isShowLastView)
            {
                panelNodes.RemoveAt(panelNodes.Count - 1);

                PanelNode lastNode = panelNodes[panelNodes.Count - 1];
                ClosePanelEvent(node);
                ShowPanel(lastNode.panelName,lastNode.type,lastNode.mode, data);
                return;
            }
        }
        //如果没在栈顶但是入栈了
        else if (CheckIfNeedBack(node) && panelNodes.Contains(node))
        {
            panelNodes.Remove(node);
        }
        ClosePanelEvent(node);
    }
    /// <summary>
    /// 打开界面触发的事件
    /// </summary>
    /// <param name="PanelName"></param>
    /// <param name="data">传递的数据</param>
    private void ShowPanelEvent(string panelName,object data)
    {
        if (!panelsDict.ContainsKey(panelName))
        {
            return;
        }
        PanelNode node = panelsDict[panelName];
        IUIController controller = UIControllerManager.Inst.GetControllerByName(node.ctrlName);
        if (controller!=null)
        {
            controller.ShowView(panelName, data);
        }
        node.SetActive(true);
    }
    /// <summary>
    /// 关闭界面触发的事件
    /// </summary>
    /// <param name="closePanel"></param>
    private void ClosePanelEvent(PanelNode closePanel)
    {
        IUIController controller = UIControllerManager.Inst.GetControllerByName(closePanel.ctrlName);
        if (controller != null)
        {
            controller.HideView(closePanel.panelName);
        }
        closePanel.SetActive(false);
    }


    /// <summary>
    /// 创建UI界面
    /// </summary>
    /// <param name="panelName">界面名称[命名方法为：包名_组件名]</param>
    /// <param name="type"></param>
    /// <param name="mode"></param>
    /// <param name="OnFinish">创建完成后回调</param>
    private void CreatPanel(string panelName,UIType type,UIMode mode,Action<PanelNode> OnFinish)
    {
        string ctrlName = UIControllerManager.Inst.GetControllerName(panelName);
        if (ctrlName == null)
        {
            Debug.LogError("这个panel没有设置ctr");
        }
        PanelNode node = new PanelNode(type, mode, ctrlName, panelName);
        string[] split = panelName.Split('_');
        if (UIPackage.GetByName(split[0])==null)//包没载入就载入包
        {//todo：这里载入资源的方式暂时用resources
            UIPackage.AddPackage("FairyGUI/" + split[0]);
        }
        node.gGameobject = UIPackage.CreateObject(split[0], split[1]);
        //node.gGameobject.Center();
        node.gGameobject.asCom.fairyBatching = true;

        node.gGameobject.asCom.MakeFullScreen();//全屏适配【请勿把容器锚点设为中央】
        panelsDict.Add(panelName, node);
        IUIController uiCtrl = UIController.GetControllerByName(node.ctrlName);
        if (uiCtrl != null)
        {//控制器通知页面创建
            uiCtrl.OnViewCreated(node.gGameobject, panelName, node.sortingOrder);
        }
        OnFinish?.Invoke(node);
    }
    /// <summary>
    /// 销毁当前存储的所有ui
    /// </summary>
    public void ReleaseUIRoot()
    {
        var it = panelsDict.GetEnumerator();
        while (it.MoveNext())
        {
            PanelNode node = it.Current.Value;
            node.gGameobject.Dispose();
            node.SetActive(false);
            IUIController controller = UIControllerManager.Inst.GetControllerByName(node.ctrlName);
            if (controller!=null)
            {
                controller.DestroyView(node.panelName);
            }
            if (panelNodes.Contains(node))
            {
                panelNodes.Remove(node);
            }
        }
        panelsDict.Clear();
        panelNodes.Clear();
    }


}
