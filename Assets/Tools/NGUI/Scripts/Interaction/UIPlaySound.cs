//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------
////////////////////////////////////////////////////
//// File Name :        UIPlaySound.cs
//// Tables :              nothing
//// Autor :               kid
//// Create Date :      2016.1.5
//// Content :           修改NGUI音效播放
////////////////////////////////////////////////////
using UnityEngine;


[AddComponentMenu("NGUI/Interaction/Play Sound")]
public class UIPlaySound : MonoBehaviour
{
	public enum Trigger
	{
		OnClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease,
		Custom,
	}

	public AudioClip audioClip;
	//public UISoundType soundType = UISoundType.ButtonClick;
	public Trigger trigger = Trigger.OnClick;
	bool mIsOver = false;

#if UNITY_3_5
	public float volume = 1f;
	public float pitch = 1f;
#else
	[Range(0f, 1f)]
	public float volume = 1f;
	[Range(0f, 2f)]
	public float pitch = 1f;
#endif

	void OnHover (bool isOver)
	{
		if (trigger == Trigger.OnMouseOver) {
			if (mIsOver == isOver)
				return;
			mIsOver = isOver;
		}

		if (enabled && ((isOver && trigger == Trigger.OnMouseOver) || (!isOver && trigger == Trigger.OnMouseOut))/*&& audioClip != null*/) {
			//AudioMgr.PlayUI (soundType, volume, pitch);
		}
		//    NGUITools.PlaySound(audioClip, volume, pitch);
	}

	void OnPress (bool isPressed)
	{
		if (trigger == Trigger.OnPress) {
			if (mIsOver == isPressed)
				return;
			mIsOver = isPressed;
		}

		if (enabled && ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease))/*&& audioClip != null*/) {
			//AudioMgr.PlayUI (soundType, volume, pitch);
		}
		//    NGUITools.PlaySound(audioClip, volume, pitch);
	}

	void OnClick ()
	{
		if (enabled && trigger == Trigger.OnClick/*&& audioClip != null*/) {
			//AudioMgr.PlayUI (soundType, volume, pitch);
		}
		//    NGUITools.PlaySound(audioClip, volume, pitch);
	}

	void OnSelect (bool isSelected)
	{
		if (enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
			OnHover (isSelected);
	}

	public void Play ()
	{
		//NGUITools.PlaySound(audioClip, volume, pitch);
		if (true/*&& audioClip != null*/) {
			//AudioMgr.PlayUI (soundType, volume, pitch);
		}
	}
}
