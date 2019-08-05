////////////////////////////////////////////////////
//// File Name :        UITopView.cs
//// Tables :              nothing
//// Autor :               yd
//// Create Date :         2015.9.6
//// Content :          UI
////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace UnityMVC.Core.View
{
	/// <summary>
	/// UIView
	/// </summary>
	public class UITopView : MonoBehaviour
	{
		#region UI

		/// <summary>
		/// 
		/// </summary>
		[SerializeField]
		private UILabel
			layer2_PopUpNameLabel;

		/// <summary>
		/// 
		/// </summary>
		[SerializeField]
		private UIButton
			layer2_BackButton;

		[SerializeField]
		private UIButton
			chatButton;

		/// <summary>
		/// 
		/// </summary>
		[SerializeField]
		private GameObject
			layer2_PopUpobj;

        #endregion

        #region 
        /// <summary>
        /// orTopUI
        /// 
        /// </summary>
        /// <param name="ismainMenu">If set to <c>true</c> ismain menu.</param>
        /// <param name="name">Name.</param>
        public void ShowTitle (string name) //TopView.TopState state,
		{
			layer2_PopUpNameLabel.text = name;
			float vectorX = 390;
			if (name.Equals ("")) {
				//Title
				layer2_PopUpobj.SetActive (false);  


			} else {
				//Title
				layer2_PopUpobj.SetActive (true);      

			}
		}

		/// <summary>
		/// Show this instance.
		/// </summary>
		public void Show ()
		{
			gameObject.SetActive (true);
		}

		public void Hide ()
		{
			gameObject.SetActive (false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void SetBackButtonOnClick (UIEventListener.VoidDelegate callback)
		{
			UIEventListener.Get (layer2_BackButton.gameObject).onClick = callback;
		}

		public void SetChatButtonOnClick (UIEventListener.VoidDelegate callback)
		{
			UIEventListener.Get (chatButton.gameObject).onClick = callback;
		}


		#endregion 
	}
}
