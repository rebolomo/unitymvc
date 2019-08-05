//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's alpha.
/// </summary>

[AddComponentMenu("NGUI/Tween/Tween Fill")]
public class TweenFill : UITweener
{
#if UNITY_3_5
	public float from = 1f;
	public float to = 1f;
#else
	[Range(0f, 1f)] public float from = 1f;
	[Range(0f, 1f)] public float to = 1f;
#endif

    UISprite m_Sprite;
    UILabel m_Label;

    public UISprite cachedSprite
	{
		get
		{
            if (m_Sprite == null)
			{
                m_Sprite = GetComponent<UISprite>();
                if (m_Sprite == null) m_Sprite = GetComponentInChildren<UISprite>();
			}
            return m_Sprite;
		}
	}

    public UILabel cachedLabel
    {
        get
        {
            if (m_Label == null)
            {
                m_Label = GetComponent<UILabel>();
                if (m_Label == null) m_Label = GetComponentInChildren<UILabel>();
            }
            return m_Label;
        }
    }

	[System.Obsolete("Use 'value' instead")]
	public float fill { get { return this.value; } set { this.value = value; } }

	/// <summary>
	/// Tween's current value.
	/// </summary>

    public float value 
    { 
        get 
        { 
            if(cachedSprite!=null)
                return cachedSprite.fillAmount;
            return 0;
        } 
        set 
        {
            if (cachedSprite != null)
                cachedSprite.fillAmount = value;
        } 
    }

    public string value2
    {
        get
        {
            return cachedLabel.text;
        }
        set
        {
            cachedLabel.text = value;
        }
    }

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished) 
    {
        if (isPause) return;
        //计算插值
        value = Mathf.Lerp(from, to, factor);
        float time = value * duration;
        if (cachedLabel != null)
        {
            if (time >= 1)
            {
                value2 = (value * duration).ToString("#0");
            }
            else
            {
                value2 = (value * duration).ToString("#0.0");
				if(value2=="0.0"){value2="";}
            }
        }
    }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

    static public TweenFill Begin(GameObject go, float duration, float fill)
	{
        TweenFill comp = UITweener.Begin<TweenFill>(go, duration);
		comp.from = comp.value;
        comp.to = fill;
        comp.isPause = false;
		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

    public void Pause()
    {
        isPause = true;
    }
    public void Resume()
    {
        isPause = false;
    }

	public override void SetStartToCurrentValue () { from = value; }
	public override void SetEndToCurrentValue () { to = value; }
}
