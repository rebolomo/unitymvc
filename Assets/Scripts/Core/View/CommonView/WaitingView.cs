////////////////////////////////////////////////////
//// File Name :        WaitingView.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :      2015.11.15
//// Content :           
////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityMVC.Message;
using UnityMVC.Utils;
using UnityMVC.Protocol;
using UnityMVC.Net;

namespace UnityMVC.Core.View
{
	/// <summary>
	/// Waiting view.
	/// </summary>
	public class WaitingView : BaseView<WaitingView>
	{
		#region 
		public override ViewLayer layerType { get { return ViewLayer.TopLayer; } }   //
		public override ViewZType zType { get { return ViewZType.TopZ; } }

		public override bool isDestroy { get { return false; } }  //UI
		public override bool IsStatic { get { return true; } } //
		public override string url { get { return "Prefab/UI/Public/WaitingView"; } } //
		public override string viewName { get { return "WaitingView"; } }//
		public override bool isAssetbundle { get { return false; } }//
		public override bool isLoadFromConfig { get { return false; } }//
		public override ViewType viewType { get { return ViewType.LoginView; } }
		#endregion

		#region UI

		/// <summary>
		/// The m view.
		/// </summary>
		private UIWaitingView mView;

		#endregion

		#region 

		/// <summary>
		/// The text_str.
		/// </summary>
		private string mWaitingText;

		#endregion

		#region View Methods
		protected override void Init ()
		{
			mView = gameObject.GetComponent<UIWaitingView> ();
			gameObject.layer = 23;
			transform.localPosition = new Vector3 (0, 0, -1000);
			RegisterHandler ();
		}

		//UI  View  , 
		protected override void HandleAfterOpenView ()
		{
		}

		//UI View    ,CloseView
		protected override void HandleBeforeCloseView ()
		{
		}

		//OpenView
		public override void RegisterUpdateHandler ()
		{
		}

		//View
		public override void CancelUpdateHandler ()
		{
		}

		//
		public override void Update ()
		{
			mView.SetWaitingText (mWaitingText);
		}

		//
		public override void Destroy ()
		{
			UnRegisterHandler ();
			mWaitingText = "";
			mView.ShowPanel (false);
			base.Destroy ();
		}
	
		#endregion

		#region /

		/// <summary>
		/// Registers the handler.
		/// </summary>
		private void RegisterHandler ()
		{
			UIMessageManager.AddListener (UIMessageType.UI_WaitingView_WebRequest_Start, OnGetMsg_WebRequest_Start);
			UIMessageManager.AddListener (UIMessageType.UI_WaitingView_WebRequest_End, OnGetMsg_WebRequest_End);
			UIMessageManager.AddListener (UIMessageType.UI_WaitingView_WebRequest_Failed, OnGetMsg_WebRequest_Failed);
		}

		/// <summary>
		/// Uns the register handler.
		/// </summary>
		private void UnRegisterHandler ()
		{
			UIMessageManager.RemoveListener (UIMessageType.UI_WaitingView_WebRequest_Start, OnGetMsg_WebRequest_Start);
			UIMessageManager.RemoveListener (UIMessageType.UI_WaitingView_WebRequest_End, OnGetMsg_WebRequest_End);
			UIMessageManager.RemoveListener (UIMessageType.UI_WaitingView_WebRequest_Failed, OnGetMsg_WebRequest_Failed);
		}

		#endregion

		#region 

		/// <summary>
		/// Raises the get msg_ web request_ start event.
		/// </summary>
		/// <param name="msg">Message.</param>
		private void OnGetMsg_WebRequest_Start (MobaMessage msg)
		{
			if (msg != null) {
				if (!IsOpen)
					OpenView ();
				mView.ShowPanel (true);
				mWaitingText = (string)msg.Param;
				mView.SetWaitingText (mWaitingText);
			}
		}

		/// <summary>
		/// Raises the get msg_ web request_ start event.
		/// </summary>
		/// <param name="msg">Message.</param>
		private void OnGetMsg_WebRequest_End (MobaMessage msg)
		{
			if (msg != null) {
				mView.ShowPanel (false);
				if (msg.Param != null) {
					string tipText = (string)msg.Param;
					TipView.Instance.Show (tipText);
				}
//				ClientLogger.Info("====> WaitingView.OnGetMsg_WebRequest_End : " + msg.Param);
			}
		}

		/// <summary>
		/// Raises the get msg_ web request_ start event.
		/// </summary>
		/// <param name="msg">Message.</param>
		private void OnGetMsg_WebRequest_Failed (MobaMessage msg)
		{
			if (msg != null) {
				mView.ShowPanel (false);
				if (msg.Param != null) {
					string tipText = (string)msg.Param;
					TipView.Instance.Show (tipText);
				}
			}
		}

		#endregion

		#region 

//		/// <summary>
//		/// Label
//		/// </summary>
//		/// <param name="text_str">Text_str.</param>
//		public void Show (string text_str)
//		{
//			mWaitingText = text_str;
//			mView.SetWaitingText (mWaitingText);
//		}

		#endregion

	}

}