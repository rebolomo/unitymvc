////////////////////////////////////////////////////
//// File Name :        MobaMessageManager.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :     2015.8.24
//// Content :           
////////////////////////////////////////////////////
using UnityMVC.Net;
using System;
using UnityMVC.Protocol;

namespace UnityMVC.Message
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using UnityMVC.Protocol;

	/// <summary>
	/// 
	/// </summary>
	public enum MobaMessageType
	{
//		Phonto2Client,  // 
//		MasterCode,     // MasterPeer
//		GameCode,       // GamePeer
//		PvpCode,        // PvpPeer/LobbyPeer
//		ChatCode,       // ChatPeer
		Client,         // 
		WebCode, //
	}

	/// <summary>
	/// 
	/// </summary>
	public class MobaMessage
	{
        #region 
		protected MobaMessageType mType;
		protected int mID;
		protected int mUnitID; //id
		protected object mParam;
		protected float mDelayTime;
		protected float mDelayElapsedTime;
		public bool isFinal; //View
        #endregion

        #region 
		public MobaMessage (MobaMessageType type, int id, object param)
		{
			mType = type;
			mID = id;
			mParam = param;
			mDelayTime = 0.0f;
			mDelayElapsedTime = 0.0f;
		}

		public MobaMessage (MobaMessageType type, int id, object param, float delayTime)
		{
			mType = type;
			mID = id;
			mParam = param;
			mDelayTime = delayTime;
			mDelayElapsedTime = 0.0f;
		}

		public MobaMessage (MobaMessageType type, int id, object param, float delayTime, int unit_id, bool isfinal)
		{
			mType = type;
			mID = id;
			mUnitID = unit_id;
			mParam = param;
			mDelayTime = delayTime;
			mDelayElapsedTime = 0.0f;
			isFinal = isfinal;
		}
        #endregion

        #region Delay Check
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsDelayExec ()
		{
			bool ret = false;

			mDelayElapsedTime += Time.deltaTime;
			if (mDelayElapsedTime < mDelayTime) {
				ret = true;
			}

			return ret;
		}
        #endregion

        #region get
		public MobaMessageType MessageType {
			get { return mType; }
		}

		public int ID {
			get { return mID; }
		}

		public object Param {
			get { return mParam; }
		}

		public int UnitID {
			get{ return mUnitID;}
		}
        #endregion
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="msg"></param>
	public delegate void MobaMessageFunc (MobaMessage msg);

	/// <summary>
	/// 
	/// </summary>
	public class MobaMessageManager : MonoBehaviour
	{
        #region 
		private static Dictionary<MobaMessageType, Dictionary<int, MobaMessageFunc>> mMessageFuncMap = new Dictionary<MobaMessageType, Dictionary<int, MobaMessageFunc>> ();
		private static Queue<MobaMessage> mMessageQueue = new Queue<MobaMessage> ();
        #endregion

        #region Awake & Start
		void Awake ()
		{

		}

		void Start ()
		{
			//mMessageFuncMap.Clear();
			//mMessageQueue.Clear();
		}
        #endregion

        #region 
        #endregion

        #region 

		/// <summary>
		/// Regists the message.
		/// </summary>
		/// <param name="msgID">Message I.</param>
		/// <param name="msgFunc">Message func.</param>
		public static void RegistMessage (WebMessageCode msgID, MobaMessageFunc msgFunc)
		{
			RegistMessage (MobaMessageType.WebCode, (int)msgID, msgFunc);
		}

		/// <summary>
		/// Uns the regist message.
		/// </summary>
		/// <param name="msgID">Message I.</param>
		/// <param name="msgFunc">Message func.</param>
		public static void UnRegistMessage (WebMessageCode msgID, MobaMessageFunc msgFunc)
		{
			UnRegistMessage (MobaMessageType.WebCode, (int)msgID, msgFunc);
		}

		/// <summary>
		/// Regists the message.
		/// </summary>
		/// <param name="msgID">Message I.</param>
		/// <param name="msgFunc">Message func.</param>
		public static void RegistMessage (ClientMsg msgID, MobaMessageFunc msgFunc)
		{
			RegistMessage (MobaMessageType.Client, (int)msgID, msgFunc);
		}

		/// <summary>
		/// Uns the regist message.
		/// </summary>
		/// <param name="msgID">Message I.</param>
		/// <param name="msgFunc">Message func.</param>
		public static void UnRegistMessage (ClientMsg msgID, MobaMessageFunc msgFunc)
		{
			UnRegistMessage (MobaMessageType.Client, (int)msgID, msgFunc);
		}

		public static int CallbackCount(int msgID) {
			Dictionary<int, MobaMessageFunc> funcMap = mMessageFuncMap [MobaMessageType.Client];
			
			if (funcMap.ContainsKey (msgID)) {
				if (funcMap [msgID] != null && funcMap [msgID].GetInvocationList ().Length > 0) {
					return funcMap [msgID].GetInvocationList ().Length;
				}
			}

			return 0;
		}

		#endregion

		#region 

		private static void RegistMessage (MobaMessageType type, int msgID, MobaMessageFunc msgFunc)
		{
			if (mMessageFuncMap.ContainsKey (type) == false) {
				mMessageFuncMap [type] = new Dictionary<int, MobaMessageFunc> ();
			}

			Dictionary<int, MobaMessageFunc> funcMap = mMessageFuncMap [type];

			if (funcMap.ContainsKey (msgID)) {
				funcMap [msgID] += msgFunc;
				//	REBOL note, message
				/*
				if (msgID == (int)UIMessageType.UI_RedBall_Update) {
					Debug.Log ("Add:" + msgFunc.Target);
					if (funcMap [msgID].GetInvocationList ().Length > 0) {
						foreach (Delegate dele in funcMap [msgID].GetInvocationList()) {
							//
							MobaMessageFunc d = (MobaMessageFunc)dele;
							
							ClientLogger.Debug ("Message", d.Target);
						}
					}
				}*/
			} else {
				funcMap [msgID] = msgFunc;
				//	REBOL note, message
				/*
				if (msgID == (int)UIMessageType.UI_RedBall_Update) {
					ClientLogger.Debug ("Message", "Create:" + msgFunc.Target);
					if (funcMap [msgID].GetInvocationList ().Length > 0) {
						foreach (Delegate dele in funcMap [msgID].GetInvocationList()) {
							//
							MobaMessageFunc d = (MobaMessageFunc)dele;
							
							ClientLogger.Debug ("Message", d.Target);
						}
					}
				}
				*/
			}
		}

		private static void UnRegistMessage (MobaMessageType msgType, int msgID, MobaMessageFunc msgFunc)
		{
			if (mMessageFuncMap.ContainsKey (msgType) == false)
				return;

			Dictionary<int, MobaMessageFunc> funcMap = mMessageFuncMap [msgType];
			if (funcMap.ContainsKey (msgID)) {

				funcMap [msgID] -= msgFunc;
				//	REBOL note, message
				/*
				if (msgID == (int)UIMessageType.UI_RedBall_Update) {
					ClientLogger.Debug ("Message", "Delete:" + msgFunc.Target);
					if (funcMap [msgID] != null && funcMap [msgID].GetInvocationList ().Length > 0) {
						foreach (Delegate dele in funcMap [msgID].GetInvocationList()) {
							//
							MobaMessageFunc d = (MobaMessageFunc)dele;
							
							ClientLogger.Debug ("Message", d.Target);
						}
					}
				}
				*/
			}
		}
        #endregion

        #region 
		/// <summary>
		/// 
		/// 
		/// </summary>
		/// <param name="msg"></param>
		public static void DispatchMsg (MobaMessage msg)
		{
			mMessageQueue.Enqueue (msg);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		public static void ExecuteMsg (MobaMessage msg)
		{
			ClientLogger.Debug ("MsgHandler", "==>MobaMessageManager ExecuteMsg : type = " + msg.MessageType + " id = " + msg.ID);

			if (mMessageFuncMap.ContainsKey (msg.MessageType)) {
				if (mMessageFuncMap [msg.MessageType].ContainsKey (msg.ID)) {
					MobaMessageFunc func = mMessageFuncMap [msg.MessageType] [msg.ID];
					if (func != null) {
						func.Invoke (msg);
					}
				}
			}
		}
        #endregion

        #region 

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <returns>The message.</returns>
		/// <param name="msgID">Message I.</param>
		/// <param name="msgParam">Message parameter.</param>
		public static MobaMessage GetMessage (ClientMsg msgID, object msgParam)
		{
			return GetMessage (MobaMessageType.Client, (int)msgID, msgParam, 0.0f);
		}

        /// <summary>
		/// Gets the message.
		/// </summary>
		/// <returns>The message.</returns>
		/// <param name="msgID">Message I.</param>
		/// <param name="msgParam">Message parameter.</param>
		/// <param name="unit_id">Unit_id.</param>
		public static MobaMessage GetMessage (ClientMsg msgID, object msgParam, int unit_id)
		{
			return GetMessage (MobaMessageType.Client, (int)msgID, msgParam, 0.0f, unit_id);
		}

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <returns>The message.</returns>
		/// <param name="msgID">Message I.</param>
		/// <param name="msgParam">Message parameter.</param>
		/// <param name="delayTime">Delay time.</param>
		public static MobaMessage GetMessage (ClientMsg msgID, object msgParam, float delayTime)
		{
			return GetMessage (MobaMessageType.Client, (int)msgID, msgParam, delayTime);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns>The message.</returns>
		/// <param name="msgID">Message I.</param>
		/// <param name="msgParam">Message parameter.</param>
		public static MobaMessage GetMessage (WebMessageCode msgID, object msgParam, bool isfinal = false)
		{
			return GetMessage (MobaMessageType.WebCode, (int)msgID, msgParam, 0.0f, 0, isfinal);
		}

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <returns>The message.</returns>
		/// <param name="msgType">Message type.</param>
		/// <param name="msgID">Message I.</param>
		/// <param name="msgParam">Message parameter.</param>
		/// <param name="msgDelayTime">Message delay time.</param>
		/// <param name="unit_id">Unit_id.</param>
		private static MobaMessage GetMessage (MobaMessageType msgType, int msgID, object msgParam, float msgDelayTime, int unit_id=0, bool isfinal=false)
		{
			MobaMessage msg = new MobaMessage (msgType, msgID, msgParam, msgDelayTime, unit_id, isfinal);
			return msg;
		}

        #endregion

        #region Update
		void Update ()
		{
			int msgCount = mMessageQueue.Count;
			for (int i = 0; i < msgCount; i++) {
				MobaMessage msg = mMessageQueue.Dequeue ();
				if (msg.IsDelayExec ()) {
					mMessageQueue.Enqueue (msg);
				} else {
					ExecuteMsg (msg);
				}
			}
		}
        #endregion
	}

}
