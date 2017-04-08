using UnityEngine;
using System.Collections;

public class HitObjDiedMsg : GameMessage {
    private GameObject _go;
    public GameObject go { get { return _go; } }

    public HitObjDiedMsg(GameObject go)
    {
        _go = go;
        MessageManager.dispatch(this);
    }
}
