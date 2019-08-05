////////////////////////////////////////////////////
//// File Name :        UITipView.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :      2015.8.24
//// Content :           View
////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityMVC.Core.Model;
using UnityMVC.Message;

namespace UnityMVC.Core.View
{
	public class UITipView : MonoBehaviour
	{
		/// <summary>
		/// The tip text.
		/// </summary>
		[SerializeField]
		private UILabel
			TipText; 

		/// <summary>
		/// The time_out.
		/// </summary>
		[SerializeField]
		private TimeoutController time_out;

		void Start(){
		}

		void OnDestroy(){
			time_out.StopTimeOut ();
		}

		public void SetLabelText (string labeltext)
		{
			TipText.text = labeltext;
		}

		public void StartTimeOut (float value, Callback callback)
		{
			time_out.StartTimeOut (value, callback);
		}

		public void StopTip(){
			time_out.StopTimeOut ();
		}
	}
}
