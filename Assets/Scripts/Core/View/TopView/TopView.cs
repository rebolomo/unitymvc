////////////////////////////////////////////////////
//// File Name :        TopView.cs
//// Tables :              nothing
//// Autor :               yd
//// Create Date :         2015.9.6
//// Content :          UI
////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityMVC.Message;
using UnityMVC.Utils;

namespace UnityMVC.Core.View
{
	/// <summary>
	/// UICtrl
	/// </summary>
	public class TopView : BaseView<TopView>
	{
		#region View
		public override ViewLayer layerType { get { return ViewLayer.MiddleLayer; } }

		public override ViewRenderQueue queueType { get { return ViewRenderQueue.TopUIQueue; } }

		public override ViewZType zType { get { return ViewZType.TopZ; } }

		public override bool IsStatic { get { return true; } }

		public override bool IsFullUI { get { return false; } }  //UIStaticUI

		public override bool isDestroy { get { return true; } }
		
		public override string url { get { return "Prefab/UI/TopView"; } }
		
		public override string viewName { get { return "TopView"; } }
		
		public override bool isAssetbundle { get { return false; } }
		
		public override bool isLoadFromConfig { get { return false; } }
		
		public override ViewType viewType { get { return ViewType.HomeView; } }
		#endregion
		
		#region UI
		private UITopView mView;
		#endregion
		
		#region 
		/// <summary>
		/// roleId
		/// </summary>
		private int roleId;
		private CoroutineManager mCoroutineManager = new CoroutineManager ();

		#endregion
		
		#region 
		#endregion
		
		#region View
		//UI
		protected override void Init ()
		{
			ClientLogger.Info (" ==>" + gameObject.name + " Init");
			//View
			mView = gameObject.GetComponent<UITopView> ();

			mView.SetBackButtonOnClick (onClickBackButton);
			mView.SetChatButtonOnClick( onChatButton);
			Update ();
		}
		
		protected override void HandleAfterOpenView ()
		{
		}
		
		protected override void HandleBeforeCloseView ()
		{
		}
		
		public override void RegisterUpdateHandler ()
		{
			//Web
            //UI
            UIMessageManager.AddListener (UIMessageType.UI_TopView_UpdateTitle, GetMsg_UI_TopView);

			UIMessageManager.AddListener (UIMessageType.UI_TopView_Hide, GetMsg_UI_TopView);
			UIMessageManager.AddListener (UIMessageType.UI_TopView_Show, GetMsg_UI_TopView);
           
			Update ();
		}

	    public override void CancelUpdateHandler ()
		{
            UIMessageManager.RemoveListener (UIMessageType.UI_TopView_UpdateTitle, GetMsg_UI_TopView);

			UIMessageManager.RemoveListener (UIMessageType.UI_TopView_Hide, GetMsg_UI_TopView);
			UIMessageManager.RemoveListener (UIMessageType.UI_TopView_Show, GetMsg_UI_TopView);
		}
		
		public override void Update ()
		{
			UpdateRedBall ();

			UpdateTittle ();
		}

		public override void Destroy ()
		{
			base.Destroy ();
		}
		#endregion

        #region 

		/// <summary>
		/// 
		/// </summary>
		public void UpdateTittle ()
		{

		}

        #endregion
		
		#region 
		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg">Message.</param>
		private void GetMsg_UI_RedBall_Update (MobaMessage msg)
		{
			if (msg != null) {
				UpdateRedBall ();
			}
		}

		/// <summary>
		/// Gets the msg_ U i_ top view_ update title.
		/// </summary>
		/// <param name="msg">Message.</param>
		private void GetMsg_UI_TopView (MobaMessage msg)
		{
			if (msg != null) {
				UIMessageType msgType = (UIMessageType)msg.ID;
				switch (msgType) {
				case UIMessageType.UI_TopView_UpdateTitle:
				//case UIMessageType.UI_TiliJingli:
					{
						string title = (string)msg.Param;
						ClientLogger.Debug ("UIMsg", "====> TopView.ShowTitle title = " + title);
						mView.ShowTitle (title); 
					}
					break;
				case UIMessageType.UI_TopView_Hide:
					{
						ClientLogger.Debug ("UIMsg", "====> TopView.Hide ");
						mView.Hide ();
					}
					break;
				case UIMessageType.UI_TopView_Show:
					{
						ClientLogger.Debug ("UIMsg", "====> TopView.Show ");
						mView.Show ();
					}
					break;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="go">Go.</param>
		private void onClickBackButton (GameObject go)
		{
			//
			UIMessageManager.SendMsg (UIMessageType.UI_TopView_BackButton, null);
		}

		private void onChatButton (GameObject go)
		{
			SecondView.Instance.OpenView();
		}

		/// <summary>
		/// 
		/// </summary>
		private void UpdateRedBall ()
		{
			//ClientLogger.Info(" ----------------TopView.UpdateRedBall---------------- ");
			
			//mView.SetRedBallState (RedballManager.GetMailsState);
		}
		#endregion
	}
}