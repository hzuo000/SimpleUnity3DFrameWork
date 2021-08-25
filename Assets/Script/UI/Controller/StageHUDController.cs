using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageHUDController : UIControllerBase
{
    public override List<string> ViewList { get => new List<string>() {
        UIComponentName.StageHUD,
    }; }

    public override string ControllerName { get => UIControllerName.StageHUDCtr; }

    protected override void AddObjectCompent(GObject gobj, string panelName, int sortingOrder)
    {
        IUIView view = default;
        switch (panelName)
        {
            case UIComponentName.StageHUD:
                view = new StageHUDView();
                break;
        }
        view.SetCtrl(this, panelName, gobj, sortingOrder);
        view.Awake();
        UIDict[panelName] = view;
        
    }
}
