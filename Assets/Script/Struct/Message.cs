
using System.Collections.Generic;


public class Message 
{
    private Dictionary<string, object> MgsDict;

    /// <summary>
    /// 构造消息包【格式为："参数名1",参数对象1,"参数名2",参数对象2 .......】
    /// </summary>
    /// <param name="args">可变长度参数</param>
    public Message(params object[] args)
    {
        MgsDict = new Dictionary<string, object>();
        for (int i = 1; i < args.Length; i += 2) 
        {
            MgsDict.Add((string)args[i - 1], args[i]);
        }
    }
 
    public Message()
    {
        MgsDict = new Dictionary<string, object>();
    }
    /// <summary>
    /// 是否存在某参数[构造后使用]
    /// </summary>
    /// <param name="key">参数名</param>
    /// <returns></returns>
    public bool Contains(string key)
    {
        return MgsDict.ContainsKey(key);
    }
    public void Add(string key, object value)
    {
        MgsDict.Add(key, value);
    }
    public void Clear()
    {
        MgsDict.Clear();
    }
    /// <summary>
    /// 通过索引获取参数[构造后使用]
    /// </summary>
    /// <param name="key">参数名</param>
    /// <returns></returns>
    public object this[string key]
    {
        get => MgsDict.ContainsKey(key) ? MgsDict[key] : null;
        set
        {
            if (MgsDict.ContainsKey(key))
            {
                MgsDict[key] = value;
            }
            else
            {
                Add(key, value);
            }
        }
    }
}
