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
    /// <summary>
    /// 设置当前页面的控制器
    /// </summary>
    /// <param name="ctrl"></param>
    /// <param name="name"></param>
    /// <param name="gobj"></param>
    /// <param name="sortingOrder"></param>
    void SetCtrl(IUIController ctrl, string name, GObject gobj, int sortingOrder);
    /// <summary>
    /// 通过某个条件获取页面中的某个object
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    GObject GetGObject(string param);
    /// <summary>
    /// 刷新页面
    /// </summary>
    /// <param name="param"></param>
    void FreshView(string param);

    object GetObject2(string param);
}
public abstract class UIViewBase : IUIView
{
    private IUIController UICtrl;
    private GObject gObject;
    protected string panelName = string.Empty;
    protected int sortingOrder = 0;
    /// <summary>
    /// 当前页面组件
    /// </summary>
    protected GComponent Gcom { get => gObject.asCom; }
    protected UIManager Manager { get => GameManager.UI; }
    protected UIControllerManager UIController { get => Manager.UIController; }
    #region 接口
    public void Awake()
    {
        OnInitialized();
    }

    public virtual void FreshView(string param)
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

    public void SetCtrl(IUIController ctrl, string panelName, GObject gobj, int sortingOrder)
    {
        UICtrl = ctrl;
        gObject = gobj;
        this.sortingOrder = sortingOrder;
        this.panelName = panelName;
    }
    #endregion 接口
    /// <summary>
    /// 获取当前页面的控制器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetCtrl<T>() where T : UIControllerBase
    {
        return UICtrl as T;
    }
    /// <summary>
    /// 给派生类用的初始化函数
    /// </summary>
    protected virtual void OnInitialized()
    {

    }
}
