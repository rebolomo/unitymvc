////////////////////////////////////////////////////
//// File Name :        TipView.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :    	2015.8.24
//// Content :           MonoBehaviour
////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Threading;

using UnityMVC.Core.Model;
using UnityMVC.Utils;

namespace UnityMVC.Core.View
{
	//
	public class TipView : BaseView<TipView>
	{
        #region View
		public override ViewLayer layerType { get { return ViewLayer.TopLayer; } } 
		public override ViewZType zType { get { return ViewZType.TopZ; } } 
		public override bool isDestroy { get { return false; } }  
		public override string url { get { return "Prefab/UI/TipView"; } } 
		public override string viewName { get { return "TipView"; } }
		public override bool isAssetbundle { get { return false; } }
		public override bool isLoadFromConfig { get { return false; } }
		public override ViewType viewType { get { return ViewType.LoginView; } }
        #endregion

        #region 
		UITipView mTipView;
		TweenAlpha TweenAnim;
        #endregion

		#region 
		public string TipText;
		public float TipTime;
		#endregion

        #region View
		protected override void Init ()
		{
			//transform.gameObject.layer = LayerMask.NameToLayer ("UIEffect");
			mTipView = transform.GetComponent<UITipView> ();
			TweenAnim = transform.GetComponent<TweenAlpha> ();
			TweenAnim.ResetToBeginning ();
		}

		//	Open Effects, Play animation, etc
		protected override void HandleAfterOpenView ()
		{
			if (TweenAnim != null) {
				TweenAnim.Begin ();
			}
		}

		//	Hide effects, stop animation, etc
		protected override void HandleBeforeCloseView ()
		{
		}

		//	Register data change event listener
		public override void RegisterUpdateHandler ()
		{
			Update();
		}

		//	Cancel data change event listener
		public override void CancelUpdateHandler ()
		{
		}

		//	Update
		public override void Update ()
		{
			Show_Internal (TipText, TipTime);
		}

		public override void Destroy ()
		{
			ClientLogger.Info ("==> TipView.Update");
			mTipView.StopTip ();
			TipText = "";
			base.Destroy ();
		}

		/// <summary>
		/// Sets the text.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <param name="timeValue">Time value.</param>
		private void Show_Internal (string text, float timeValue)
		{
			if (timeValue == 0)
				timeValue = 2f;
			mTipView.SetLabelText (text);
			if (timeValue != 0) {
				StartTimeOut (timeValue);
			}
		}

		private void StartTimeOut (float value)
		{
			mTipView.StartTimeOut (value, CloseView);
		}

		/// <summary>
		/// TipView
		/// </summary>
		/// <param name="text">Text.</param>
		public void Show (string text, float timeValue = 2f)
		{
			OpenView ();
			TipText = text;
			TipTime = timeValue;
			Update ();
		}

        #endregion
	}
}