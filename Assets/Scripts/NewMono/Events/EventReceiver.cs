using UnityEngine;
using System.Collections;

/// <summary>
/// send sender out when send msg 
/// </summary>
public class EventReceiver
{
    private GameObject _receiver;
    private string _func;

    public EventReceiver(GameObject receiver, string func)
    {
        this._receiver = receiver;
        this._func     = func;
    }

    public void send(GameObject sender)
    {
        if (_receiver && !string.IsNullOrEmpty(_func))
            _receiver.SendMessage(_func, sender, SendMessageOptions.DontRequireReceiver);
    }
}
