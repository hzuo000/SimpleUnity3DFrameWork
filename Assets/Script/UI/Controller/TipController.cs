using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipController : UIControllerBase
{
    public override List<string> ViewList {
        get => new List<string>()
        {
            UIComponentName.FadingView
        };
    }

    public override string ControllerName { get => UIControllerName.TipsCtr; }

    protected override void AddObjectCompent(GObject gobj, string panelName, int sortingOrder)
    {
        IUIView view = default;
        switch (panelName)
        {
            case UIComponentName.FadingView:
                view = new FadingMessage();
                break;
        }
        view.SetCtrl(this, panelName, gobj, sortingOrder);
        view.Awake();
        UIDict[panelName] = view;
    }
}
