using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// 这个消息是组件发往center的消息类型【比如player被打了】
/// </summary>
public enum LocalMessage
{
    Null=-1,

    COUNT
}
/// <summary>
/// 这个消息是center发往各个组件的消息类型【比如屏幕变红】
/// </summary>
public enum ComponentMessage
{
    Null=-1,

    COUNT
}

public class MessageCenter : GameInterface
{
    private static object LockObj = new object();//注意多线程不要又加又删
    private Dictionary<ComponentMessage, Action<Message>> ComponentRegisterFuncs;//本地组件回调
    private Dictionary<LocalMessage, Action<Message>> StageRegisterFuncs;//客户端接收消息过后的回调
    public override void StartUp()
    {
        InitMessageHandler();
        ComponentRegisterFuncs = new Dictionary<ComponentMessage, Action<Message>>();
        for (int i = 0; i < (int)ComponentMessage.COUNT; ++i)
        {
            ComponentRegisterFuncs.Add((ComponentMessage)i, null);
        }


        base.StartUp();
    }
    /// <summary>
    /// 注册消息句柄[大消息拆解,添加拆解的方法]
    /// </summary>
    private void InitMessageHandler()
    {
        StageRegisterFuncs = new Dictionary<LocalMessage, Action<Message>>() {
            
        };

    }
    /// <summary>
    /// 订阅消息[对象销毁时记得退订]
    /// </summary>
    /// <param name="_msg"></param>
    /// <param name="_func"></param>
    public void RegisterHandler(List<ComponentMessage>_msgs, Action<Message> _func)
    {
        lock (LockObj)
        {
            foreach (var msg in _msgs)
            {
                ComponentRegisterFuncs[msg] += _func;
            }
        }
    }
    /// <summary>
    /// 退订消息
    /// </summary>
    /// <param name="_msg"></param>
    /// <param name="_func"></param>
    public void UnsubscribeHandler(List<ComponentMessage> _msgs, Action<Message> _func)
    {
        lock (LockObj)
        {
            foreach (var msg in _msgs)
            {
                if (ComponentRegisterFuncs[msg] != null)
                {
                    ComponentRegisterFuncs[msg] -= _func;
                }
            }

        }

    }
    /// <summary>
    /// 对消息本身进行处理，拆解成不同component的不同指令
    /// </summary>
    /// <param name="_msg"></param>
    /// <param name="smsg"></param>
    public void SendMessage(LocalMessage _msg, Message smsg)
    {
        lock (LockObj)
        {
            StageRegisterFuncs[_msg]?.Invoke(smsg);
        }
    }
}
