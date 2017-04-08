using UnityEngine;
using System.Collections;

public class HitObjInstantiatedMsg : GameMessage {
    private GameObject _go;
    public GameObject go { get { return _go; } }
    public HitObjInstantiatedMsg(GameObject go)
    {
        _go = go;
        MessageManager.dispatch(this);
    }
}
