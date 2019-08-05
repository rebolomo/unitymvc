////////////////////////////////////////////////////
//// File Name :        ViewTree.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :      2015.8.24
//// Content :           View
////////////////////////////////////////////////////
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityMVC.Message;

namespace UnityMVC.Core.View
{
	/// <summary>
	/// View
	/// </summary>
	[AddComponentMenu("NGUI/UI/ViewTree")]
	public class ViewTree : MonoBehaviour
	{
		#region 
		//private GameObject go; 
		public GameObject root; // Root for all UI views
		public UIRoot ui_root; //UI Root
		public Camera uiCamera;
		public Camera uiEffectCamera;
		#endregion
		
		#region 

		#endregion
		
		#region 
		public static ViewTree Instance;
		#endregion

		#region Unity Methods

		void Awake ()
		{
			Instance = this;
			//go = gameObject;

			//
			DontDestroyOnLoad (gameObject); 
		}

		void Start(){
		}
		
		void OnDestroy ()
		{

		}

		#endregion

	
		#region /

		public Callback<int> OnLoadingViewEndCallback;


		private void OnGetMsg_Battle_Show_GUI (MobaMessage msg)
		{
			//	UI
			bool show = (bool)msg.Param;
			uiCamera.gameObject.SetActive(show);
		}

		private void OnGetMsg_SelectedBuilding (MobaMessage msg)
		{
		}

		#endregion
	}
}
