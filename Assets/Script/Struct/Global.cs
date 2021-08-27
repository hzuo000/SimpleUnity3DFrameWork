using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 一个简单的对象池
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPool<T> where T : new()
{
    private readonly Stack<T> m_Stack = new Stack<T>();
    private readonly Action<T> m_ActionOnGet;
    private readonly Action<T> m_ActionOnRelease;

    public int CountAll { get; private set; }
    public int CountActive { get { return CountAll - CountInactive; } }
    public int CountInactive { get { return m_Stack.Count; } }

    public ObjectPool(Action<T> actionOnGet, Action<T> actionOnRelease)
    {
        m_ActionOnGet = actionOnGet;
        m_ActionOnRelease = actionOnRelease;
    }

    //出栈
    public T Get()
    {
        T element;
        if (m_Stack.Count == 0)
        {
            element = new T();
            CountAll++;
        }
        else
        {
            element = m_Stack.Pop();
        }
        m_ActionOnGet?.Invoke(element);
        return element;
    }

    //释放-入栈
    public void Release(T element)
    {
        m_ActionOnRelease?.Invoke(element);
        m_Stack.Push(element);
    }
}


public static class Explore//扩展方法
{
    /// <summary>
    /// 把字符串中所有的数字转整型[过滤掉除数字字符外的所有字符]
    /// </summary>
    /// <param name="_tString"></param>
    /// <returns></returns>
    public static int ToInt(this string _tString)
    {
        if (_tString == null) return 0;
        int id = 0;
        int index = 0;
        for (int i = _tString.Length - 1; i >= 0; --i)
        {
            char t = _tString[i];
            int temp = t - '0';
            if (temp >= 0 && temp <= 9)
            {
                id += temp * (int)Math.Pow(10, index++);
            }
        }
        return id;
    }

    public static List<int> ToInts(this string _tString)
    {
        List<int> res = new List<int>();
        _tString += ",";
        string _strTemp = "";
        for (int i = 0; i < _tString.Length; i++)
        {
            if (_tString[i] >= '0' && _tString[i] <= '9')
            {
                _strTemp += _tString[i];
            }
            else if (_strTemp.Length != 0)
            {
                res.Add(int.Parse(_strTemp));
                _strTemp = "";
            }
            else
            {
                continue;
            }
        }
        return res;
    }

    /// <summary>
    /// 是否在范围内[坐标系在左下角]
    /// </summary>
    /// <param name="area"></param>
    /// <param name="vector"></param>
    /// <param name="includeBorder">是否包含边界</param>
    /// <returns></returns>
    public static bool IsRange(this Area2D area, Vector2 vector, bool includeBorder)
    {
        if (!includeBorder)
        {
            return vector.x > area.Left && vector.x < area.Right && vector.y < area.Top && vector.y > area.Buttom;
        }
        else
        {
            return vector.x >= area.Left && vector.x <= area.Right && vector.y <= area.Top && vector.y >= area.Buttom;
        }

    }
    public static List<T> RandomList<T>(this List<T> t)//洗牌
    {
        for (int i = t.Count - 1; i >= 0; i--)
        {
            int ran = UnityEngine.Random.Range(0, i + 1);
            T temp = t[ran];
            t[ran] = t[i];
            t[i] = temp;
        }
        return t;
    }
}
public static class GlobalData
{

}
/// <summary>
/// 音乐ID
/// </summary>
public enum MusicID
{
    NULL = -1,



    COUNT,
}
/// <summary>
/// 音效ID
/// </summary>
public enum SoundID
{
    NULL = -1,

    

    COUNT,
}
/// <summary>
/// 存常用文件路径
/// </summary>
public static class FilePath
{
    
}
/// <summary>
/// 存放ui的名字【包名_组件名】
/// </summary>
public static class UIComponentName
{
    public const string LoadingMainView = "Loading_LoadMainView";//开始游戏加载界面ui
    public const string MainUIMainView = "MainUI_MainView";//主界面ui
    public const string StageHUD = "Stage_StageHUD";//战斗场景主ui
    public const string MainHUD = "MainUI_MainHUD";
    public const string MainSelect = "MainUI_MainSelectLevel";
    public const string MainAbout = "MainUI_MainAbout";
    public const string MainSetting = "MainUI_MainSettingPanel";
}
/// <summary>
/// ui控制器名称
/// </summary>
public static class UIControllerName
{
    public const string LoadingCtr = "LoadingController";//载入场景控制器
    public const string MainUICtr = "MainUIController";//主ui控制器
    public const string StageHUDCtr = "StageHUDController";//stageHUD控制器
}
/// <summary>
/// 2D空间范围
/// </summary>
public struct Area2D
{
    public float Left;
    public float Right;
    public float Top;
    public float Buttom;

    public Area2D(float _left, float _right, float _top, float _buttom)
    {
        Left = _left;
        Right = _right;
        Top = _top;
        Buttom = _buttom;
    }
    public void Reset()
    {
        Left = .0f;
        Right = .0f;
        Top = .0f;
        Buttom = .0f;
    }
}

