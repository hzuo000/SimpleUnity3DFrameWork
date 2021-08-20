using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public interface IUIView 
{
    void OnHide();

    void OnShow(object data);

    void Awake();

    void OnDestroy();

    void SetCtrl(IUIController ctrl, string name, GObject gobj, int sortingOrder);

    GObject GetGObject(string param);

    void Execute(string param);

    object GetObject2(string param);
}
public abstract class UIViewBase : IUIView
{
    private IUIController UICtrl;
    private GObject gObject;
    protected string panelName = string.Empty;
    protected int sortingOrder = 0;

    protected GComponent Gcom { get => gObject.asCom; }
    protected UIManager Manager { get => GameManager.UI; }
    protected UIControllerManager UIController { get => Manager.UIController; }

    public void Awake()
    {
        
    }

    public void Execute(string param)
    {
        
    }

    public virtual GObject GetGObject(string param)
    {
        return null;
    }

    public virtual object GetObject2(string param)
    {
        return null;
    }

    public virtual void OnDestroy()
    {
        
    }

    public virtual void OnHide()
    {
        
    }

    public virtual void OnShow(object data)
    {
        
    }

    public void SetCtrl(IUIController ctrl, string name, GObject gobj, int sortingOrder)
    {
        UICtrl = ctrl;
        gObject = gobj;
        this.sortingOrder = sortingOrder;
        panelName = name;
    }
}
