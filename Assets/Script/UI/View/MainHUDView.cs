using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class MainHUDView : UIViewBase
{
    private GTextField IDText;
    protected override void OnInitialized()
    {
        IDText = Gcom.GetChild("n3").asTextField;
        IDText.text = "UID:111222";
    }
}
public class MainSettingView : UIViewBase
{
    GButton SelectBtn;
    protected override void OnInitialized()
    {
        Gcom.GetChild("BackBtn").asButton.onClick.Add(() => {
            GameManager.UI.ClosePanel(panelName);
        });
        SelectBtn = Gcom.GetChild("Select").asButton;
        SelectBtn.title = "关卡选择";
        SelectBtn.onClick.Add(() => {
            GameManager.UI.OpenPanel(UIComponentName.MainSelect, UIType.Normal, UIMode.HideOther);
        });
    }
    public override void OnHide()
    {
        base.OnHide();
        Debug.Log("关闭设置");
    }
    public override void OnShow(object data)
    {
        base.OnShow(data);
        Debug.Log("打开设置");
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        Debug.Log("销毁设置");
    }
}
public class MainSelectLevel : UIViewBase
{
    GButton BackBtn;
    GButton AboutBtn;
    protected override void OnInitialized()
    {
        BackBtn = Gcom.GetChild("BackBtn").asButton;
        BackBtn.title = "返回";
        BackBtn.onClick.Add(()=> {
            GameManager.UI.ClosePanel(panelName);
        });
        AboutBtn = Gcom.GetChild("ToAbout").asButton;
        AboutBtn.title = "关于";
        AboutBtn.onClick.Add(()=> {
            GameManager.UI.OpenPanel(UIComponentName.MainAbout, UIType.Normal, UIMode.HideOther);
        });
    }
    public override void OnHide()
    {
        base.OnHide();
        Debug.Log("关闭选择");
    }
    public override void OnShow(object data)
    {
        base.OnShow(data);
        Debug.Log("打开选择");
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        Debug.Log("销毁选择");
    }
}
public class MainAboutView : UIViewBase
{
    GButton BackBtn;
    protected override void OnInitialized()
    {
        base.OnInitialized();
        BackBtn = Gcom.GetChild("BackBtn").asButton;
        BackBtn.title = "返回";
        BackBtn.onClick.Add(() => {
            GameManager.UI.ClosePanel(panelName);
        });
    }
    public override void OnHide()
    {
        base.OnHide();
        Debug.Log("关闭关于");
    }
    public override void OnShow(object data)
    {
        base.OnShow(data);
        Debug.Log("打开关于");
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        Debug.Log("销毁关于");
    }
}
