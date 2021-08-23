using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 一个简单的事件回调
/// </summary>
public class EventCallBack 
{
    //类型可以自行扩展

    private event Action onComplete;

    /// <summary>
    /// 不带参数的action
    /// </summary>
    public void OnComplete(Action _delegate) => onComplete += _delegate;
    /// <summary>
    /// 触发事件
    /// </summary>
    public void Excute()
    {
        onComplete?.Invoke();
        //触发完之后删除订阅
        Delegate[] dels = onComplete.GetInvocationList();
        for (int i = 0; i < dels.Length; i++)
        {
            onComplete -= (Action)dels[i];
        }
    }
}
