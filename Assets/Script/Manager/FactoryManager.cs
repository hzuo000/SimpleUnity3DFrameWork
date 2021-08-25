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
 

}
