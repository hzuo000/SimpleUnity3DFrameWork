using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIController : UIControllerBase
{
    public override List<string> ViewList { get => new List<string>() {
        UIComponentName.MainUIMainView,
        UIComponentName.MainAbout,
        UIComponentName.MainHUD,
        UIComponentName.MainSelect,
        UIComponentName.MainSetting,
    }; }

    public override string ControllerName { get => UIControllerName.MainUICtr; }

    protected override void AddObjectCompent(GObject gobj, string panelName, int sortingOrder)
    {
        IUIView view = default;
        switch (panelName)
        {
            case UIComponentName.MainUIMainView:
                view = new MainPageView();
                break;
            case UIComponentName.MainSelect:
                view = new MainSelectLevel();
                break;
            case UIComponentName.MainHUD:
                view = new MainHUDView();
                break;
            case UIComponentName.MainSetting:
                view = new MainSettingView();
                break;
            case UIComponentName.MainAbout:
                view = new MainAboutView();
                break;
        }
        view.SetCtrl(this, panelName, gobj, sortingOrder);
        view.Awake();
        UIDict[panelName] = view;
    }
    public override void Init(Action<string, string> OnInitAction)
    {
        base.Init(OnInitAction);
    }
}
