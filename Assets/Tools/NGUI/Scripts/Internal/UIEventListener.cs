//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Event Hook class lets you easily add remote event listener functions to an object.
/// Example usage: UIEventListener.Get(gameObject).onClick += MyClickFunction;
/// </summary>

[AddComponentMenu("NGUI/Internal/Event Listener")]
public class UIEventListener : MonoBehaviour
{
	public delegate void VoidDelegate (GameObject go);
	public delegate void BoolDelegate (GameObject go, bool state);
	public delegate void FloatDelegate (GameObject go, float delta);
	public delegate void VectorDelegate (GameObject go, Vector2 delta);
	public delegate void ObjectDelegate (GameObject go, GameObject draggedObject);
	public delegate void KeyCodeDelegate (GameObject go, KeyCode key);

	public object parameter;

	public VoidDelegate onSubmit;
	public VoidDelegate onClick;
	public VoidDelegate onDoubleClick;
	public BoolDelegate onHover;
	public BoolDelegate onPress;
	public BoolDelegate onSelect;
	public FloatDelegate onScroll;
	public VectorDelegate onDrag;
	public ObjectDelegate onDrop;
	public KeyCodeDelegate onKey;

    public VoidDelegate onDragStart;
    public BoolDelegate onMobileHover;
    public VoidDelegate onDragEnd;

	public VoidDelegate LongPress; 

	float intialTime=1f;//初始按下去的时长

	float intervalTime=0.05f;//间隔的时长

	public bool isCanLongPress=false;//是否可以长按
	bool isPress=false;//记录是否按下

	void OnSubmit ()				{ if (onSubmit != null) onSubmit(gameObject); }
	void OnClick ()					{ if (onClick != null) onClick(gameObject); }
	void OnDoubleClick ()			{ if (onDoubleClick != null) onDoubleClick(gameObject); }
	void OnHover (bool isOver)		{ if (onHover != null) onHover(gameObject, isOver); }
	void OnPress (bool isPressed)	
	{
		if (onPress != null) 
		onPress(gameObject, isPressed); 
		if(isPressed)
			isPress=true;
		else
			isPress=false;

	}
	void OnSelect (bool selected)	{ if (onSelect != null) onSelect(gameObject, selected); }
	void OnScroll (float delta)		{ if (onScroll != null) onScroll(gameObject, delta); }
	void OnDrag (Vector2 delta)		{ if (onDrag != null) onDrag(gameObject, delta); }
	void OnDrop (GameObject go)		{ if (onDrop != null) onDrop(gameObject, go); }
	void OnKey (KeyCode key)		{ if (onKey != null) onKey(gameObject, key); }

    void OnDragStart() { }
    void OnDragOver(GameObject draggedObject) { if (onMobileHover != null) onMobileHover(gameObject, true); }
    void OnDragOut(GameObject draggedObject) { if (onMobileHover != null) onMobileHover(gameObject, false); }
    void OnDragEnd (){}

    //长按功能
	void Update()
	{
		if(isCanLongPress)
		{
			if(isPress)
			{
				if(intialTime>0)
				{
					intialTime=intialTime-Time.deltaTime;
				}else
				{
					LongPress(gameObject);
					intialTime=intialTime+intervalTime;
				}
			}else if(!isPress&&intialTime!=1f)
			{
				intialTime=1f;
			}
		}
	}

	/// <summary>
	/// Get or add an event listener to the specified game object.
	/// </summary>

	static public UIEventListener Get (GameObject go)
	{
		UIEventListener listener = go.GetComponent<UIEventListener>();
		if (listener == null) listener = go.AddComponent<UIEventListener>();
		return listener;
	}

	void OnDisable()
	{
		isPress=false;
	}
}
