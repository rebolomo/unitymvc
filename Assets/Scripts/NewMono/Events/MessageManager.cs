using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MessageManager
{
    private static Dictionary<System.Type, List< Action<GameMessage> > > _lses = new Dictionary<Type, List< Action<GameMessage> > >();

    public static void addMsgLs(System.Type type, Action<GameMessage> ls)
    {
        if (!_lses.ContainsKey(type))
            _lses.Add(type, new List<Action<GameMessage>>());
        getLs(type).Add(ls);
    }

    private static List<Action<GameMessage>> getLs(System.Type type)
    {
        if (_lses.ContainsKey(type))
            return _lses[type];
        List<Action<GameMessage>> newLses = new List<Action<GameMessage>>();
        _lses.Add(type, newLses);
        return newLses;
    }

    public static void dispatch(GameMessage msg)
    {
        List< Action<GameMessage> > lses = getLs(msg.GetType());
        foreach (Action<GameMessage> ls in lses)
        {
            ls(msg);
        }
    }

    public static void dropLs(Action<GameMessage> ls)
    {
        foreach (System.Type key in _lses.Keys)
        {
            if (_lses[key].Contains(ls))
            {
                _lses[key].Remove(ls);
                break;
            }
        }
    }

}
