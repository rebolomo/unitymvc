////////////////////////////////////////////////////
//// File Name :        PopView.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :      2015.8.24
//// Content :           MonoBehaviour
////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using UnityMVC.Message;
using UnityMVC.Utils;

namespace UnityMVC.Core.View
{
	public enum PopType
	{

	}
	//
	public class PopView : BaseView<PopView>
	{
		public override ViewLayer layerType { get { return ViewLayer.TopLayer; } }   //
		public override ViewZType zType { get { return ViewZType.TopZ; } }
		public override bool isDestroy { get { return true; } }  //UI
		public override bool IsStatic { get { return false; } } //
		public override string url { get { return "Prefab/UI/Public/PopView"; } } //
		public override string viewName { get { return "PopView"; } }//
		public override bool isAssetbundle { get { return false; } }//
		public override bool isLoadFromConfig { get { return false; } }//
		public override ViewType viewType { get { return ViewType.HomeView; } }

		#region UI
		private UIPopView mView;
		#endregion

		#region 
		private Callback onClickLeftButtonCallback;
		private Callback onClickRightButtonCallback;
		#endregion

		//UI
		protected override void Init ()
		{
			mView = gameObject.GetComponent<UIPopView> ();
		}

		void RefreshUI_Btn ()
		{
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
			//
			Update ();
		}

		//View
		public override void CancelUpdateHandler ()
		{
			mView.PopView_OpenOrClose (false, false, false);
		}

		//
		public override void Update ()
		{
		}

		//
		public override void Destroy ()
		{
			mView = null;
			base.Destroy ();
		}

		#region 
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="shuoMingStr"></param>
		/// <param name="oneButtonStr"> Label</param>
		/// <param name="callBack"></param>
		public void ShowOneButton (string shuoMingStr, string oneButtonStr, Callback callback)
		{
			OpenView ();

			onClickLeftButtonCallback = callback;

			mView.ShowOneButton (shuoMingStr, oneButtonStr, onClickLeftButton);      //Center
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="shuoMingStr"></param>
		/// <param name="LeftButtonStr"> Label</param>
		/// <param name="RightButtonStr"> Label</param>
		/// <param name="leftCallBack"></param>
		/// <param name="RightCallBack"></param>
		public void ShowTwoButton (string shuoMingStr, string LeftButtonStr, string RightButtonStr, Callback callback1, Callback callback2)
		{
			OpenView ();

			onClickLeftButtonCallback = callback1;

			onClickRightButtonCallback = callback2;

			mView.ShowTwoButton (shuoMingStr, LeftButtonStr, RightButtonStr, onClickLeftButton, onClickRightButton);  //Left  Right
		}

		#endregion

		#region 

		private void onClickLeftButton (GameObject go)
		{
			if (onClickLeftButtonCallback != null) {
				onClickLeftButtonCallback ();
			} else {
				CloseView ();
			}
		}

		private void onClickRightButton (GameObject go)
		{
			if (onClickRightButtonCallback != null) {
				onClickRightButtonCallback ();
			} else {
				CloseView ();
			}
		}

		#endregion
	}
}