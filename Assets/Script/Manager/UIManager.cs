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

    None,
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
/// ui显示节点
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
/// ui管理器【mvc模式】
/// </summary>
public class UIControllerManager
{
    private static UIControllerManager _this;
    public static UIControllerManager Inst { get => _this; }

    public UIControllerManager()
    {
        _this = this;
    }
    public ControllerT GetControllerByName<ControllerT>(string ctrlName) where ControllerT : IUIController
    {
        ControllerT local = default(ControllerT);
        //if (this._controllerDic.HasKey(ctrlName))
        //{
        //    return (ControllerT)this._controllerDic.GetValue(ctrlName);
        //}
        return local;

    }
}


public class UIManager : GameInterface
{
    /// <summary>
    /// UI控制器【控制逻辑】
    /// </summary>
    public UIControllerManager UIController { get; private set; }
    private List<PanelNode> panelNodes;
    private Dictionary<string, PanelNode> panelsDict;//<名称，实例>
    public override void StartUp()
    {
        ManagerType = GameManagerType.UI;
        panelNodes = new List<PanelNode>();
        panelsDict = new Dictionary<string, PanelNode>();
        UIController = new UIControllerManager();
        base.StartUp();
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
