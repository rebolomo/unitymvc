////////////////////////////////////////////////////
//// File Name :        FairylandView.cs
//// Tables :              nothing
//// Autor :               zlx
//// Create Date :         2015.11.2
//// Content :          
////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System;
using UnityMVC.Message;
using UnityMVC.Utils;


namespace UnityMVC.Core.View
{
	/// <summary>
	/// Ctrl
	/// </summary>
	public class SecondView : BaseView<SecondView>
	{
		#region View
		public override ViewLayer layerType { get { return ViewLayer.BaseLayer; } }
		
		public override bool IsStatic { get { return false; } }
		
		public override bool IsFullUI { get { return true; } }  //UIStaticUI
		
		public override bool isDestroy { get { return true; } }
		
		public override string url { get { return "Prefab/UI/SecondView"; } }
		
		public override string viewName { get { return "SecondView"; } }
		
		public override bool isAssetbundle { get { return false; } }
		
		public override bool isLoadFromConfig { get { return false; } }
		
		public override ViewType viewType { get { return ViewType.HomeView; } }
		#endregion

		#region UI
		private UISecondView mView;
		#endregion

		#region View
		protected override void Init ()
		{
//			ClientLogger.Info (" ==>" + gameObject.name + " Init");
			//UI
			mView = gameObject.GetComponent<UISecondView> ();

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
			UIMessageManager.SendMsg (UIMessageType.UI_TopView_UpdateTitle, "Second Page");
			UIMessageManager.AddListener (UIMessageType.UI_TopView_BackButton, OnGetMsg_TopView_BackButton);

			Update ();
		}
		
		//View
		public override void CancelUpdateHandler ()
		{
			UIMessageManager.SendMsg (UIMessageType.UI_TopView_UpdateTitle, "");
			UIMessageManager.RemoveListener (UIMessageType.UI_TopView_BackButton, OnGetMsg_TopView_BackButton);


		}

		//
		public override void Update ()
		{

		}
		
		//
		public override void Destroy ()
		{
			base.Destroy ();
		}

		#endregion

		#region 

		#endregion

		#region 

		
		private void OnGetMsg_TopView_BackButton (MobaMessage msg)
		{
			if (msg != null) {
				ClientLogger.Debug ("UIMsg", "====> TeamView : GetMsg_UI_TopView_Par_BackButton !!");
				//TODO : /
				CloseView ();
			}
		}

		#endregion

	}
}