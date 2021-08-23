using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum FactoryType
{
    Null=-1,

    Language,

    COUNT
}
public class FactoryManager : GameInterface
{
    /// <summary>
    /// 存放工厂
    /// </summary>
    private Dictionary<FactoryType, FactoryBase> FactoryDic;
    public override void StartUp()
    {
        FactoryDic = new Dictionary<FactoryType, FactoryBase>();
        InitFactory();
        base.StartUp();
    }
    private void InitFactory()
    {
        FactoryDic.Add(FactoryType.Language, new LanguageFactory("Language.csv"));
    }
    /// <summary>
    /// 获取工厂
    /// </summary>
    /// <typeparam name="T">工厂类型</typeparam>
    /// <param name="type">工厂类型</param>
    /// <returns></returns>
    public T GetFactory<T>(FactoryType type) where T:FactoryBase
    {
        if (FactoryDic.ContainsKey(type))
        {
            return FactoryDic[type] as T;
        }
        else
        {
            Debug.LogError("没有找到对应的工厂");
            return null;
        }
    }

}
