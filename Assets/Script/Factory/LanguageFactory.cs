using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LanguageType
{
    NULL = -1,


    CN,//简体中文


    COUNT//
}
public class LanguageFactory : FactoryBase
{
    public LanguageFactory(string fn) : base(fn)
    {

    }
    public string GetWords(int _nID)
    {
        string _str = "";
        if (IDToInfoDic.ContainsKey(_nID))
        {
            _str = Content[IDToInfoDic[_nID]][(int)GameManager.Record.systemRecord.Languagee];
        }
        return _str;
    }
}
