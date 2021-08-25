using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System;

public enum UIType
{
    Normal = 0,

    //固定窗口
    Fixed,

    //弹窗窗口
    PopUp,

}

public enum UIMode
{
    DoNothing,

    // 闭其他界面
    HideOther,

    // 点击返回按钮关闭当前,不关闭其他界面(需要调整好层级关系)
    NeedBack,

    // 关闭TopBar,关闭其他界面,不加入backSequence队列
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
    private static UIControllerManager _this;
    public static UIControllerManager Inst { get => _this; }

    public UIControllerManager()
    {
        _this = this;
    }
    /// <summary>
    /// 通过名字获取控制器(返回对象实例)
    /// </summary>
    /// <param name="ctrlName"></param>
    /// <returns></returns>
    public T GetControllerByName<T>(string ctrlName) where T : IUIController
    {
        T local = default(T);
        //if (this._controllerDic.HasKey(ctrlName))
        //{
        //    return (ControllerT)this._controllerDic.GetValue(ctrlName);
        //}
        return local;

    }
    /// <summary>
    /// 通过名字获取控制器（返回控制器接口类型）
    /// </summary>
    /// <param name="ctrlName"></param>
    /// <returns></returns>
    public IUIController GetControllerByName(string ctrlName)
    {
        return null;
    }
    /// <summary>
    /// 创建控制器
    /// </summary>
    /// <param name="ctrlName"></param>
    public void CreaterController(string ctrlName)
    {

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

    private List<PanelNode> panelNodes;
    private Dictionary<string, PanelNode> panelsDict;//<名称，实例>
    public override void StartUp()
    {
        panelNodes = new List<PanelNode>();
        panelsDict = new Dictionary<string, PanelNode>();
        UIController = new UIControllerManager();
        base.StartUp();
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
        node.SetActive(true);
    }
    /// <summary>
    /// 判断这个界面是不是需要返回
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    private bool CheckIfNeedBack(PanelNode page)
    {
        if (page.type == UIType.Fixed || page.type == UIType.PopUp )
            return false;
        else if (page.mode == UIMode.NoNeedBack || page.mode == UIMode.DoNothing)
            return false;
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
    private void ShowPanelEvent(string PanelName,object data)
    {

    }
    /// <summary>
    /// 关闭界面触发的事件
    /// </summary>
    /// <param name="closePanel"></param>
    private void ClosePanelEvent(PanelNode closePanel)
    {

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
        string ctrlName = panelName;//todo:控制器暂时和ui节点名一样
        PanelNode node = new PanelNode(type, mode, ctrlName, panelName);
        string[] split = panelName.Split('_');
        if (UIPackage.GetByName(split[0])==null)//包没载入就载入包
        {
            UIPackage.AddPackage("FairyGUI/" + split[0]);
        }
        node.gGameobject = UIPackage.CreateObject(split[0], split[1]);
        node.gGameobject.Center();
        node.gGameobject.asCom.fairyBatching = true;

        node.gGameobject.asCom.MakeFullScreen();//全屏适配【请勿把容器锚点设为中央】
        panelsDict.Add(panelName, node);
        IUIController uiCtrl = UIController.GetControllerByName<IUIController>(node.ctrlName);
        if (uiCtrl != null)
        {
            uiCtrl.OnViewCreated(node.gGameobject, panelName, node.sortingOrder);
        }
        OnFinish?.Invoke(node);
    }



}
