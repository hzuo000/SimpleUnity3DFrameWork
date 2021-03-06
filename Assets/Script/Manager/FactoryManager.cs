using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class FactoryManager : GameInterface
{
    /// <summary>
    /// 存放工厂
    /// </summary>
    private List<FactoryBase> FactoryList;
    public override void StartUp()
    {
        FactoryList = new List<FactoryBase>();
        InitFactory();
        base.StartUp();
    }
    private void InitFactory()
    {
        FactoryList.Add( new LanguageFactory("Language.csv"));
    }
 
    public T GetFactory<T>() where T : FactoryBase
    {
        for (int i = 0; i < FactoryList.Count; ++i)
        {
            var f = FactoryList[i];
            if (f.GetType() == typeof(T))
            {
                return f as T;
            }
        }
        return default;
    }
}
