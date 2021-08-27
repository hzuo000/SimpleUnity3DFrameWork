using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FactoryBase
{
    /// <summary>
    /// 储存csv读出来的原始数据[行][列]
    /// </summary>
    protected string[][] Content;
    /// <summary>
    /// 用id去对应数据的行数
    /// </summary>
    protected Dictionary<int, int> IDToInfoDic;
    /// <summary>
    /// 读表路径
    /// </summary>
    public string FileName { get; protected set; }

    public FactoryBase(string fileName)
    {
        FileName = "/"+fileName;
        IDToInfoDic = new Dictionary<int, int>();
        InitFactory();
    }

    /// <summary>
    /// 初始化工厂
    /// </summary>
    protected virtual void InitFactory()
    {
        string _content = CsvParser2.GetStreamingPathStr(Application.streamingAssetsPath + FileName);
        Content = CsvParser2.Parse(_content);

        for (int i = 1; i < Content.Length; i++)
        {
            if (Content[i][0] == "") continue;

            IDToInfoDic.Add(int.Parse(Content[i][0]), i);
        }
    }
     
    
}
