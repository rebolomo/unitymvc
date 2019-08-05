using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityMVC.Utils;

public class NotFoundException : System.Exception
{
    public string errInfo = "";
    public NotFoundException(string msg)
    {
#if UNITY_EDITOR
        ClientLogger.Error(msg + " !");
#endif
        this.errInfo = msg;
    }
}

public class NewMono : MonoBehaviour {
    private Transform _trans;
    public Transform trans
    {
        get
        {
            if (gameObject != null)
            {
                if (_trans == null) _trans = GetComponent<Transform>();
                return _trans;
            }
            return null;
        }
    }

    private AudioSource _as;
    public AudioSource audioSource
    {
        get
        {
            if (_as == null) _as = getComponent<AudioSource>(true);
            return _as;
        }
    }

    private Animation _anim;
    public Animation anim
    {
        get
        {
            if (_anim == null) _anim = getComponent<Animation>(true);
            return _anim;
        }
    }

    private Animator _mator;
    public Animator mator
    {
        get
        {
            if (_mator == null) _mator = getComponent<Animator>(true);
            return _mator;
        }
    }

    private Rigidbody _rb;
    public Rigidbody rb
    {
        get
        {
            if (_rb == null) _rb = getComponent<Rigidbody>(true);
            return _rb;
        }
    }

    private lsAnim _lsAnim;
    public lsAnim lsAnim
    {
        get
        {
            if (_lsAnim == null) _lsAnim = getComponent<lsAnim>(true);
            return _lsAnim;
        }
    }

    private Renderer _rd;

    public Renderer rd
    {
        get
        {
            if (_rd == null) _rd = getComponent<Renderer>(true);
            return _rd;
        }
    }

    /// <summary>
    /// call this in gui
    /// </summary>
    protected void guiLog(int width, int height, object log)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(trans.position);
        GUI.TextArea(new Rect(screenPos.x - width / 2, Screen.height - screenPos.y - height / 2, width, height), log.ToString());
    }

    /// <summary>
    /// get component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="addIfNull"></param>
    /// <returns></returns>
    public T getComponent<T>(bool addIfNull = true) where T : Component
    {
        T t = GetComponent<T>();
        if (t == null && addIfNull)
            t = gameObject.AddComponent<T>();
        return t;
    }

    public T getComponentInParent<T>() where T : Component
    {
        Transform p = trans;
        while (p != null)
        {
            T t = p.GetComponent<T>();
            if (t != null) return t;
            p = p.parent;
        }
        return null;
    }

    private Dictionary<string, GameObject> _cachedChildren = null;

    /// <summary>
    /// find the child gameObject, make sure it exists
    /// </summary>
    /// <param name="childName"></param>
    /// <returns></returns>
    protected GameObject getChild(string childName)
    {
        if (_cachedChildren == null) _cachedChildren = new Dictionary<string, GameObject>();
        if (_cachedChildren.ContainsKey(childName)) return _cachedChildren[childName];

        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform t in children)
        {
            if (t.name == childName)
            {
                _cachedChildren.Add(childName, t.gameObject);
                return t.gameObject;
            }
        }
        //not found at last, throw new exception
        const string TEMP = "{0,-15}'s child gameObject:{1,-15} is not found!";
        throw new NotFoundException(string.Format(TEMP, trans.parent.name, childName));
    }

    /// <summary>
    /// return mathf.ceil(time * frameRate)
    /// </summary>
    /// <param name="time"></param>
    /// <param name="frameRate"></param>
    /// <returns></returns>
    public static int time2Frame(float time, float frameRate)
    {
        return Mathf.CeilToInt(time * frameRate);
    }

    public static float frame2Time(int frame, float frameRate)
    {
        return frame / frameRate;
    }
}
