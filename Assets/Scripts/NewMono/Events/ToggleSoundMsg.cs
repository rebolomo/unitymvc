using UnityEngine;
using System.Collections;

public class ToggleSoundMsg : GameMessage{
    private bool _on;
    public bool on { get { return _on; } }

    public ToggleSoundMsg(bool on)
    {
        this._on = on;
        MessageManager.dispatch(this);
    }

}
