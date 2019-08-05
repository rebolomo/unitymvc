/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/07/28 20:44:00 
 * function:  tween播放管理接口
 * *******************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IPlay
{
	void Begin();
	List<EventDelegate> OnEnd{get;}
}
