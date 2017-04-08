////////////////////////////////////////////////////
//// File Name :        UIWaitingView.cs
//// Tables :              nothing
//// Autor :               zlx
//// Create Date :         2015.10.15
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
	/// </summary>
	public class UIWaitingView : MonoBehaviour
	{
		#region UI
		
		/// <summary>
		/// 
		/// </summary>
		[SerializeField]
		private UILabel
			layer2_TiShiLabel;

		/// <summary>
		/// The panel.
		/// </summary>
		[SerializeField]
		private GameObject
			Panel;
		
		#endregion

		void Start(){
		}

		#region 

		/// <summary>
		/// 
		/// </summary>
		/// <param name="str">String.</param>
		public void SetWaitingText (string text)
		{
			layer2_TiShiLabel.text = text;
		}

		/// <summary>
		/// Shows the panel.
		/// </summary>
		/// <param name="is_show">If set to <c>true</c> is_show.</param>
		public void ShowPanel (bool is_show)
		{
			Panel.SetActive (is_show);
		}

		#endregion
	}
}