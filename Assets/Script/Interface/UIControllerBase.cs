using System.Collections;
using System.Collections.Generic;
using FairyGUI;


public interface IUIController
{
    void OnHide(string panelName);

    void OnShow(string panelName, object data);

    void OnDestroy(string panelName);

    void OnViewCreated(GObject panelObj, string panelName, int sortingOrder);

    GObject GetGObject(string panelName, string param);

    void Execute(string panelName, string param);

    object GetObject2(string panelName, string param);

    List<string> ViewList { get; }
}

public abstract class UIControllerBase : IUIController
{
    
    public List<string> ViewList => throw new System.NotImplementedException();
    /// <summary>
    /// ui对应的页面
    /// </summary>
    protected Dictionary<string, IUIView> mUIDict = new Dictionary<string, IUIView>();

    public void Execute(string panelName, string param)
    {
        if (!mUIDict.ContainsKey(panelName))
        {
            return;
        }

        mUIDict[panelName].Execute(param);
    }

    public GObject GetGObject(string panelName, string param)
    {
        if (!mUIDict.ContainsKey(panelName))
        {
            return null;
        }

        return mUIDict[panelName].GetGObject(param);
    }

    public object GetObject2(string panelName, string param)
    {
        if (!mUIDict.ContainsKey(panelName))
        {
            return null;
        }

        return mUIDict[panelName].GetObject2(param);
    }

    public virtual void OnDestroy(string panelName)
    {
        mUIDict[panelName].OnDestroy();
        mUIDict[panelName] = null;
    }

    public virtual void OnHide(string panelName)
    {
        mUIDict[panelName].OnHide();
    }

    public virtual void OnShow(string panelName, object data)
    {
        mUIDict[panelName].OnShow(data);
    }

    public void OnViewCreated(GObject panelObj, string panelName, int sortingOrder)
    {
        OnViewGameObjectCreated(panelObj, panelName, sortingOrder);
    }
    protected virtual void OnViewGameObjectCreated(GObject obj, string panelName, int sortingOrder)
    {
        AddObjectCompent(obj, panelName, sortingOrder);
    }
    protected abstract void AddObjectCompent(GObject gobj, string panelName, int sortingOrder);
}
