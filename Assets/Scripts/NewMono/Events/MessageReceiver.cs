using UnityEngine;
using System.Collections;

/// <summary>
/// dont send sender out when send msg
/// </summary>
public class MessageReceiver 
{
    private string _func;
    private GameObject _receiver;

    public MessageReceiver(GameObject receiver, string func)
    {
        this._receiver = receiver;
        this._func     = func;
    }

    public void send()
    {
        if (_receiver && !string.IsNullOrEmpty(_func))
            _receiver.SendMessage(_func, SendMessageOptions.DontRequireReceiver);
    }
}
