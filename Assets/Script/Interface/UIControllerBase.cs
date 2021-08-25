using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using System;


public interface IUIController
{
    string ControllerName { get; }
    /// <summary>
    /// 初始化（回调<viewName,ControllerName>）
    /// </summary>
    /// <param name="OnInitAction"></param>
    void Init(Action<string, string> OnInitAction);
    void HideView(string panelName);

    void ShowView(string panelName, object data);

    void DestroyView(string panelName);

    void OnViewCreated(GObject panelObj, string panelName, int sortingOrder);
    /// <summary>
    /// 获取view中的一个组件
    /// </summary>
    /// <param name="panelName"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    GObject GetGObject(string panelName, string param);
    /// <summary>
    /// 刷新一个view的状态
    /// </summary>
    /// <param name="panelName"></param>
    /// <param name="param">传的数据</param>
    void FreshView(string panelName, string param);
    /// <summary>
    /// 获取view中的某一个数据
    /// </summary>
    /// <param name="panelName"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    object GetObject2(string panelName, string param);
    IUIView GetView(string viewName);
    /// <summary>
    /// 当前控制器存放的所有的view
    /// </summary>
    List<string> ViewList { get; }
}

public abstract class UIControllerBase : IUIController
{
    public abstract List<string> ViewList { get; }

    public abstract string ControllerName { get;  }

    /// <summary>
    /// ui对应的页面
    /// </summary>
    protected Dictionary<string, IUIView> UIDict = new Dictionary<string, IUIView>();

    public  void FreshView(string panelName, string param)
    {
        if (!UIDict.ContainsKey(panelName))
        {
            return;
        }

        UIDict[panelName].FreshView(param);
    }

    public  GObject GetGObject(string panelName, string param)
    {
        if (!UIDict.ContainsKey(panelName))
        {
            return null;
        }

        return UIDict[panelName].GetGObject(param);
    }

    public  object GetObject2(string panelName, string param)
    {
        if (!UIDict.ContainsKey(panelName))
        {
            return null;
        }

        return UIDict[panelName].GetObject2(param);
    }

    public virtual void DestroyView(string panelName)
    {
        UIDict[panelName].OnDestroy();
        UIDict[panelName] = null;
    }

    public virtual void HideView(string panelName)
    {
        UIDict[panelName].OnHide();
    }

    public virtual void ShowView(string panelName, object data)
    {
        UIDict[panelName].OnShow(data);
    }

    public void OnViewCreated(GObject panelObj, string panelName, int sortingOrder)
    {
        AddObjectCompent(panelObj, panelName, sortingOrder);
    }
    /// <summary>
    /// 添加控制的view（绑定对象）
    /// </summary>
    /// <param name="gobj"></param>
    /// <param name="panelName"></param>
    /// <param name="sortingOrder"></param>
    protected abstract void AddObjectCompent(GObject gobj, string panelName, int sortingOrder);

    public virtual void Init(Action<string, string> OnInitAction)
    {
        for (int i = 0; i < ViewList.Count; ++i) 
        {
            OnInitAction?.Invoke(ViewList[i], ControllerName);
        }
    }
    /// <summary>
    /// 通过名字获取view
    /// </summary>
    /// <param name="viewName"></param>
    /// <returns></returns>
    public IUIView GetView(string viewName)
    {
        if (!UIDict.ContainsKey(viewName))
        {
            return UIDict[viewName];
        }
        return null;
    }
}
