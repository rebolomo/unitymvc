////////////////////////////////////////////////////
//// File Name :        UIPopView.cs
//// Tables :              nothing
//// Autor :               zlx
//// Create Date :         2015.10.15
//// Content :          
////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityMVC.Resource;

namespace UnityMVC.Core.View
{
	/// <summary>
	///  
	/// </summary>
	public class UIPopView : MonoBehaviour
	{
		#region UI

		/// <summary>
		/// Left
		/// </summary>
		[SerializeField]
		private UIButton popView_LeftButton;

		/// <summary>
		/// Right
		/// </summary>
		[SerializeField]
		private UIButton popView_RightButton;

		/// <summary>
		/// Center
		/// </summary>
		[SerializeField]
		private UIButton popView_CenterButton;

		/// <summary>
		/// 
		/// </summary>
		[SerializeField]
		private GameObject[] popView_GameObjects;

		/// <summary>
		/// number
		/// </summary>
		[SerializeField]
		private UILabel popView_ShuoMingLabel;

		/// <summary>
		/// One number
		/// </summary>
		[SerializeField]
		private UILabel popView_OneLabel;

		/// <summary>
		/// Tow  number
		/// </summary>
		[SerializeField]
		private UILabel popView_TwoLabel;

		/// <summary>
		/// Three  number
		/// </summary>
		[SerializeField]
		private UILabel popView_ThreeLabel;
		#endregion

		#region 
		#endregion

		#region 
		/// <summary>
		///   ShuoMing  buttonShuoMing
		/// </summary>
		/// <param name="ShuoMing">Shuo ming.</param>
		/// <param name="buttonShuoMing">Button shuo ming.</param>
		/// <param name="callback">Callback.</param>
		public void ShowOneButton(string ShuoMing,string buttonShuoMing,UIEventListener.VoidDelegate callback)
		{
			PopView_OpenOrClose (false,false,true);
			popView_ThreeLabel.text = buttonShuoMing;
			popView_ShuoMingLabel.text = ShuoMing;

			UIEventListener.Get (popView_CenterButton.gameObject).onClick = callback;
		}

		/// <summary>
		/// Label  Label  Label  Button  Button
		/// 
		/// </summary>
		/// <param name="ShuoMing"> Label</param>
		/// <param name="buttonLeftShuoMing"> Label</param>
		/// <param name="buttonRightShuoMing"> Label</param>
		/// <param name="callbackLeft"> Button</param>
		/// <param name="callbackRight"> Button</param>
		public void ShowTwoButton(string ShuoMing,string buttonLeftShuoMing,string buttonRightShuoMing,UIEventListener.VoidDelegate callbackLeft,UIEventListener.VoidDelegate callbackRight)
		{
			PopView_OpenOrClose (true,true,false);
			popView_ShuoMingLabel.text = ShuoMing;
			popView_OneLabel.text = buttonLeftShuoMing;
			popView_TwoLabel.text = buttonRightShuoMing;

			UIEventListener.Get (popView_LeftButton.gameObject).onClick = callbackLeft;
			UIEventListener.Get (popView_RightButton.gameObject).onClick = callbackRight;
		}
		#endregion

		/// <summary>
		/// Left Right Center
		/// </summary>
		/// <param name="oneGameObject">If set to <c>true</c> one game object.</param>
		/// <param name="twoGameObject">If set to <c>true</c> two game object.</param>
		/// <param name="threeGameObject">If set to <c>true</c> three game object.</param>
		public void PopView_OpenOrClose(bool oneGameObject,bool twoGameObject,bool threeGameObject)
		{
			popView_GameObjects [0].SetActive (oneGameObject);
			popView_GameObjects [1].SetActive (twoGameObject);
			popView_GameObjects [2].SetActive (threeGameObject);
		}
	}
}