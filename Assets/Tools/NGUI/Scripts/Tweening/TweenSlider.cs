//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's alpha.
/// </summary>

[AddComponentMenu("NGUI/Tween/Tween Slider")]
public class TweenSlider : UITweener
{
#if UNITY_3_5
	public float from = 1f;
	public float to = 1f;
#else
	[Range(0f, 1f)] public float from = 1f;
	[Range(0f, 1f)] public float to = 1f;
#endif

    UISlider m_Slider;

    public UISlider cachedSlider
	{
		get
		{
            if (m_Slider == null)
			{
                m_Slider = GetComponent<UISlider>();
                if (m_Slider == null) m_Slider = GetComponentInChildren<UISlider>();
			}
            return m_Slider;
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
            if (cachedSlider != null)
                return cachedSlider.value;
            return 0;
        } 
        set 
        {
            if (cachedSlider != null)
                cachedSlider.value = value;
        } 
    }

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished) 
    {
        //计算插值
        value = Mathf.Lerp(from, to, factor);
    }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

    static public TweenSlider Begin(GameObject go, float duration, float fill)
	{
        TweenSlider comp = UITweener.Begin<TweenSlider>(go, duration);
		comp.from = comp.value;
        comp.to = fill;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	public override void SetStartToCurrentValue () { from = value; }
	public override void SetEndToCurrentValue () { to = value; }
}
