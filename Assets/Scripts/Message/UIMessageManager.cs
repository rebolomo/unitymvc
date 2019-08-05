////////////////////////////////////////////////////
//// File Name :        UIMessageManager.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :    	2015.10.15
//// Content :           UI
////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityMVC.Core;
using UnityMVC.Message;

namespace UnityMVC.Message
{
	public class UIMessageManager :Singleton<UIMessageManager>
	{

		#region implemented abstract members of Singleton
		public override void SingletonCreate ()
		{
		}

		public override void SingletonDestroy ()
		{
		}
		#endregion

		#region 

		/// <summary>
		/// 
		/// </summary>
		/// <returns>The msg_to_ client message.</returns>
		/// <param name="code">Code.</param>
		private static ClientMsg UIMsg_to_ClientMsg (UIMessageType code)
		{
			return (ClientMsg)((int)code);
		}

		#endregion

		#region 

		/// <summary>
		/// Adds the listener.
		/// </summary>
		/// <param name="code">Code.</param>
		/// <param name="func">Func.</param>
		public static void AddListener (UIMessageType code, MobaMessageFunc func)
		{
			ClientLogger.Debug ("UIMsg", "==>UIMessageManager AddListener : " + code);

			MobaMessageManager.RegistMessage (UIMsg_to_ClientMsg (code), func);
		}

		/// <summary>
		/// Removes the listener.
		/// </summary>
		/// <param name="code">Code.</param>
		/// <param name="func">Func.</param>
		public static void RemoveListener (UIMessageType code, MobaMessageFunc func)
		{
			ClientLogger.Debug ("UIMsg", "==>UIMessageManager RemoveListener : " + code);

			MobaMessageManager.UnRegistMessage (UIMsg_to_ClientMsg (code), func);
		}

		/// <summary>
		/// UI
		/// </summary>
		/// <returns><c>true</c>, if c_ battle_ start was c2ed, <c>false</c> otherwise.</returns>
		public static void SendMsg (UIMessageType code, object param=null, float delayTime = 0.0f)
		{
			ClientLogger.Debug ("UIMsg", "==>UIMessageManager SendMsg : " + code + " " + (param != null ? param : ""));

			MobaMessage newMsg = MobaMessageManager.GetMessage ((ClientMsg)code, param, delayTime);
			MobaMessageManager.DispatchMsg (newMsg);
		}
		#endregion

	}
}
