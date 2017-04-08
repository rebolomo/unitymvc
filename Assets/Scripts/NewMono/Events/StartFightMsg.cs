using UnityEngine;
using System.Collections;

/// <summary>
/// all Entity get ready to fight,init ai, start 2 move......
/// </summary>
public class StartFightMsg : GameMessage {
    public StartFightMsg()
    {
        MessageManager.dispatch(this);
    }
}
