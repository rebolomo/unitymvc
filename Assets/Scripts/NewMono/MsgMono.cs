using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// add msg implementation for newmono
/// </summary>
public class MsgMono : NewMono {

    /// <summary>
    /// children gameObjects with mono script attached
    /// </summary>
    private MsgMono[] _monoChildren = null;
    /// <summary>
    /// broad cast msg to all monoes
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="refreshMonoChildren"></param>
    public void broadCastMsg(string msg, bool refreshMonoChildren = false)
    {
        if (_monoChildren == null || refreshMonoChildren)
            _monoChildren = GetComponentsInChildren<MsgMono>();
        if (ArrayTool.isNullOrEmpty(_monoChildren)) return;
        foreach (MsgMono mono in _monoChildren)
        {
            if (mono == null) continue;
            mono.SendMessage(msg, SendMessageOptions.DontRequireReceiver);
        }
    }

    private Dictionary<System.Type, Action<GameMessage> > _msgLs;

    protected void addMsgLs(System.Type type, Action<GameMessage> action)
    {
        if (_msgLs == null) _msgLs = new Dictionary<Type, Action<GameMessage>>();
        if (_msgLs.ContainsKey(type))
        {
            LSDebug.error("add duplicated ls!");
            return;
        }
        _msgLs.Add(type, action);
        MessageManager.addMsgLs(type, action);
    }

    protected void dropMsgs()
    {
        if (_msgLs == null) return;
        foreach (Action<GameMessage> ls in _msgLs.Values)
        {
            MessageManager.dropLs(ls);
        }
    }


}
