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

    OnSceneChange,//场景改变


    COUNT
}
/// <summary>
/// 这个消息是center发往各个组件的消息类型【比如屏幕变红】
/// </summary>
public enum ComponentMessage
{
    Null=-1,

    ClearCurrentUI,//清理当前场景所有UI
    LoadNewSceneUI,//载入新场景的第一个UI

    COUNT
}

public class MessageCenter : GameInterface
{
    private static object LockObj = new object();//注意多线程不要又加又删
    private Dictionary<ComponentMessage, Action<Message>> ComponentRegisterFuncs;//消息中心向外分发组件消息回调
    private Dictionary<LocalMessage, Action<Message>> StageRegisterFuncs;//消息中心接收消息过后的回调
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
            { LocalMessage.OnSceneChange,HandelMSG_OnSceneChange },
        };

    }
    /// <summary>
    /// 订阅消息[对象销毁时记得退订]
    /// </summary>
    public void RegisterHandler(Dictionary<ComponentMessage,Action<Message>> registerMessages)
    {
        lock (LockObj)
        {
            var it = registerMessages.GetEnumerator();
            while (it.MoveNext())
            {
                var cur = it.Current;
                ComponentRegisterFuncs[cur.Key] += cur.Value;
            }
        }
    }
    /// <summary>
    /// 退订消息
    /// </summary>
    public void UnsubscribeHandler(Dictionary<ComponentMessage, Action<Message>> registerMessages)
    {
        lock (LockObj)
        {
            var it = registerMessages.GetEnumerator();
            while (it.MoveNext())
            {
                var cur = it.Current;
                if (ComponentRegisterFuncs[cur.Key] != null)
                {
                    ComponentRegisterFuncs[cur.Key] -= cur.Value;
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
    /// <summary>
    /// 场景改变
    /// 1.清空当前场景的UI
    /// </summary>
    /// <param name="message"></param>
    private void HandelMSG_OnSceneChange(Message message)
    {
        ComponentRegisterFuncs[ComponentMessage.ClearCurrentUI]?.Invoke(message);
        ComponentRegisterFuncs[ComponentMessage.LoadNewSceneUI]?.Invoke(message);
    }
}
