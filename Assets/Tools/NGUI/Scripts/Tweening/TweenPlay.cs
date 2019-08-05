/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/07/28 20:44:00 
 * function:  tween播放类
 * *******************************************************/
using UnityEngine;
using AnimationOrTween;
using System.Collections.Generic;

/// <summary>
/// 根据TweenGroup设置播放队列
/// </summary>
using System;


/*
 * 动画播放队列控制，播放顺序由tweenGroup决定
 * 控制的粒度是单位是Tween，一个Tween周期是不有处理的
 * 
 */
using System.Collections;


[ExecuteInEditMode]
[AddComponentMenu("NGUI/Tween/Tween Play")]
public class TweenPlay : MonoBehaviour,IPlay
{
	/// <summary>
	/// Target on which there is one or more tween.
	/// </summary>
	
	public GameObject tweenTarget;
	
	/// <summary>
	/// If there are multiple tweens, you can choose which ones get activated by changing their group.
	/// </summary>
	
	//public int tweenGroup = 0;

	/// <summary>
	/// Direction to tween in.
	/// </summary>
	
	public Direction playDirection = Direction.Forward;
	
	/// <summary>
	/// Whether the tweens on the child game objects will be considered.
	/// </summary>
	
	public bool includeChildren = false;

	public List<UITweener> mTweens = new List<UITweener>();
	/// <summary>
	/// 目前没有实现在一个Tween内切换，只有当一个Tween播放完，才会其作用，后面又这个需求可以很容易扩展实现
	/// </summary>
	/// <value>The style.</value>
	public UITweener.Style style 
	{
		get;set;
	}

	private int index;//当前播放动画索引

	private int count;//动画播放计数

	public float delay;//动画延时

	public List<EventDelegate> onFinished = new List<EventDelegate>();
	public List<EventDelegate> OnEnd
	{
		get{
			return onFinished;
		}
	}
	public void Begin()
	{
		Play (playDirection);
	}
	
	void Awake ()
	{
		UpdateTween();
		ResetToBeginning();
	}

	void Reset()
	{
		tweenTarget = gameObject;
	}
	public void Play(Direction playDirection)
	{
		this.playDirection = playDirection;
		UpdateTween();
		ResetToBeginning();
		Play();
	}

	public void PlayForward()
	{
		Play (Direction.Forward);
	}

	public void PlayReverse()
	{
		Play (Direction.Reverse);
	}
	/// <summary>
	/// Play the specified playDirection.
	/// </summary>
	/// <param name="playDirection">Play direction. Toggle for Pause</param>
	private void Play()
	{
		if (mTweens == null || mTweens.Count == 0)
		{
			return ;
		}
		else if(playDirection == Direction.Toggle)
		{
			Toggle();
		}
		else
		{
			List<UITweener> temp = GetTweenerByGroup(mTweens[index].tweenGroup);
			count = temp.Count;
			if(playDirection == Direction.Forward)
				index += count -1;
			else if(playDirection == Direction.Reverse)
				index -= count -1;
			foreach( UITweener tw in temp)
			{
				Play(tw,playDirection);
			}

		}
	}
	public void Toggle()
	{
		List<UITweener> temp = GetTweenerByGroup(mTweens[index].tweenGroup);
		if(temp != null)
		{
			foreach(UITweener tw in temp)
			{
				tw.enabled = !tw.enabled;
			}
		}
	}
	public void Pause()
	{
		List<UITweener> temp = GetTweenerByGroup(mTweens[index].tweenGroup);
		if(temp != null)
		{
			foreach(UITweener tw in temp)
			{
				tw.enabled = false;
			}
		}
	}

	public void Resume()
	{
		List<UITweener> temp = GetTweenerByGroup(mTweens[index].tweenGroup);
		if(temp != null)
		{
			foreach(UITweener tw in temp)
			{
				tw.enabled = true;
			}
		}
		Play();
	}

	private void Play(UITweener tw,Direction playDirection)
	{

		if(playDirection == Direction.Forward)
		{
			tw.Replay(true);
		}
		else
		{
			tw.Replay(false);
		}
	}


	public void UpdateTween()
	{
		GameObject go = (tweenTarget == null) ? gameObject : tweenTarget;
		mTweens.Clear();	
		// Gather the tweening components
	    UITweener[] tweens = includeChildren ? go.GetComponentsInChildren<UITweener>() : go.GetComponents<UITweener>();
	    foreach (UITweener tween in tweens)  //tweenGroup 小于0 的不添加进去
	    {
	        if (tween.tweenGroup >= 0)
	        {
	            mTweens.Add(tween);
	        }
	    }
		mTweens.Sort(CompareByGropFunc);
		for(int i =0,leng = mTweens.Count;i<leng;i++)
		{
			InitTween(i,PlayNext);
		}
	}

    public void SetStartToCurrentValue()
    {
        foreach (UITweener tweener in mTweens)
        {
            tweener.SetStartToCurrentValue();
        }
    }

    public void SetEndToCurrentValue()
    {
        foreach (UITweener tweener in mTweens)
        {
            tweener.SetEndToCurrentValue();
        }
    }

   

	public void ResetToBeginning()   //没有开始播放，只是恢复到开始状态,goods job
	{
		if(mTweens == null || mTweens.Count == 0)
			return ;
		if(index >= mTweens.Count)
			index = mTweens.Count-1;
		else if(index <0)
			index = 0;
		List<UITweener> temp = GetTweenerByGroup(mTweens[index].tweenGroup);
		if(temp != null)
		{
			foreach(UITweener tw in temp)
			{
				tw.ResetToBeginning();
			}
		}
		if(playDirection == Direction.Forward)
			index = 0;
		else if(playDirection == Direction.Reverse)
			index = mTweens.Count-1;

	}

	private int CompareByGropFunc(UITweener left,UITweener right)
	{
		if(left.tweenGroup< right.tweenGroup)
			return -1;
		else if(left.tweenGroup> right.tweenGroup)
			return 1;
		else 
			return 0;
	}

	public List<UITweener> GetTweenerByGroup(int group)
	{
		List<UITweener> tweens = new List<UITweener>();
		foreach(UITweener tw in mTweens)
		{
			if(tw.tweenGroup == group)
			{
				tweens.Add(tw);
			}
		}
		return tweens;
	}
	private void InitTween(int index , EventDelegate.Callback callBack)
	{
		UITweener tw;
		if(index < mTweens.Count && index >= 0)
		{
			tw = mTweens[index];
			tw.ResetToBeginning();
			tw.style = UITweener.Style.Once;
			tw.enabled = false;
			EventDelegate.Add(tw.onFinished,callBack ,false);
		}
	}
	private void PlayNext()
	{
		count --;
		if(count ==0)
		{
			if(playDirection == Direction.Forward)
			{
				index ++ ;
				if(index>= mTweens.Count)
				{
					if(style == UITweener.Style.Once)
					{
						// Notify the listener delegates
						index = mTweens.Count - 1;
						EventDelegate.Execute(onFinished);
						return;
					}
					else if(style == UITweener.Style.Loop)
					{
						index =0;
					}
					else   //PingPong
					{
						index = mTweens.Count - 1;
						this.playDirection = Direction.Reverse;
					}
				}
			}
			else if(playDirection == Direction.Reverse)
			{
				index -- ;
				if(index < 0)
				{
					if(style == UITweener.Style.Once)
					{
						// Notify the listener delegates
						index = 0;
						EventDelegate.Execute(onFinished);
						return;
					}
					else if(style == UITweener.Style.Loop)
					{
						index = mTweens.Count - 1;
					}
					else   //PingPong
					{
						index = 0;
						this.playDirection = Direction.Forward;
					}
				}
			}
			Play();
		}

	}
	public void AddTweenEvent(UITweener tw ,EventDelegate.Callback callBack)
	{
		if(mTweens.Contains(tw))
		{
			EventDelegate.Add(tw.onFinished,callBack);
		}
	}
}
