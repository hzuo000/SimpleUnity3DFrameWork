using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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

