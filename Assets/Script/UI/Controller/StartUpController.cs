using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpController : UIControllerBase
{
    public override List<string> ViewList { get => new List<string>() {
        UIComponentName.LoadingMainView,
    }; }

    public override string ControllerName { get => UIControllerName.LoadingCtr; }
    
    protected override void AddObjectCompent(GObject gobj, string panelName, int sortingOrder)
    {
        IUIView view = default;
        switch (panelName)
        {
            case UIComponentName.LoadingMainView:
                view = new StartUpView();
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
