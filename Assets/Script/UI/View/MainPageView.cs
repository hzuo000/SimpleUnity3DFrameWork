using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class MainPageView : UIViewBase
{
    private GButton StartBtn;
    private GButton SelectBtn;
    private GButton SettingBtn;
    protected override void OnInitialized()
    {
        base.OnInitialized();
        StartBtn = Gcom.GetChild("StartBtn").asButton;
        StartBtn.onClick.Add(()=> { GameManager.Scene.LoadScene(SceneName.Stage); });
        SelectBtn = Gcom.GetChild("Select").asButton;
        SelectBtn.title = "关卡选择";
        SelectBtn.onClick.Add(()=> {
            GameManager.UI.OpenPanel(UIComponentName.MainSelect, UIType.Normal, UIMode.HideOther);
        });
        SettingBtn = Gcom.GetChild("Setting").asButton;
        SettingBtn.title = "设置";
        SettingBtn.onClick.Add(()=> {
            Util.PlayFadingMessage("啊大大大大");
             //GameManager.UI.OpenPanel(UIComponentName.MainSetting, UIType.Normal, UIMode.NeedBack);
        });
    }
    public override void OnHide()
    {
        base.OnHide();
        Debug.Log("关闭主界面");
    }
    public override void OnShow(object data)
    {
        base.OnShow(data);
        Debug.Log("打开主界面");
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        Debug.Log("销毁主界面");
    }
}
