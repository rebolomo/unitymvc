////////////////////////////////////////////////////
//// File Name :        UIFairylandView.cs
//// Tables :              nothing
//// Autor :               zlx
//// Create Date :         2015.11.2
//// Content :          
////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityMVC.Core.View
{
	/// <summary>
	///  View
	/// TODO:
	/// </summary>
	public class UIFirstView : MonoBehaviour
	{
		#region UI

		/// <summary>
		/// 
		/// </summary>
		[SerializeField]
		private UIButton
			mButton;

		#endregion

		#region 

		#endregion

		#region 

		/// <summary>
		/// 
		/// </summary>
		/// <param name="callBack">Call back.</param>
		public void SetButtonsClick (UIEventListener.VoidDelegate callBack)
		{
			UIEventListener.Get (mButton.gameObject).onClick = callBack;
		}

		#endregion
	}
}