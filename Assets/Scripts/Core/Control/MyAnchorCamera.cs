////////////////////////////////////////////////////
//// File Name :        MyAnchorCamera.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :      2015.8.24
//// Content :           
////////////////////////////////////////////////////

using UnityMVC.Core.View;
using UnityMVC.Message;


namespace UnityMVC.GameCamera
{
	using UnityEngine;
	using System.Collections;

	[ExecuteInEditMode]
	public class MyAnchorCamera : MonoBehaviour
	{
		public enum AnchorModel
		{
			None,
			Auto,
			Tall,
			Width,
		}

		public AnchorModel Model;
		public float resolutionScale = 1.0f;
		public float scale = 1.0F;
		public float suitableUI_width = 0.0f;
		public float suitableUI_height = 0.0f;
		public bool ShowResolution = false;
		public bool isNGUIHierarchy = false;
		public bool is3DCamera = false;

		/// <summary>
		/// The m camera.
		/// </summary>
		[SerializeField]
		public Camera
			mCamera;

		/// <summary>
		/// The m_ui_root.
		/// </summary>
		[SerializeField]
		private UIRoot
			m_ui_root;

		private UIRoot
			ui_root {
			get {
				if (m_ui_root == null) {
					if (ViewTree.Instance != null) {
						m_ui_root = ViewTree.Instance.ui_root;
					}
				}
				return m_ui_root;
			}
		}

		private Matrix4x4 m = new Matrix4x4 ();
		private Matrix4x4 originalProjection;
		private Rect originalPixelRect;
		private int screenWidth;
		private int screenHeight;

		#region Unity Method

		// Use this for initialization
		void Awake ()
		{
			if (mCamera == null)
				mCamera = GetComponent<Camera> ();
//			if (isNGUIHierarchy) {
//				if (ui_root == null) {
//					ui_root = MyAnchorCameraTool.FindInParents<UIRoot> (gameObject);
//				}
//			}
		}

		void Start ()
		{
			screenWidth = Screen.width;
			screenHeight = Screen.height;
			SelectMode ();
			RegisterMsgHandler();
		}

		void OnDestroy ()
		{
			mCamera = null;
			UnRegisterMsgHandler();
		}

		void Update ()
		{
			if (screenWidth != Screen.width || screenHeight != Screen.height) {
				SelectMode ();
				screenWidth = Screen.width;
				screenHeight = Screen.height;
			}
		}

		#endregion

		#region 

		void RegisterMsgHandler()
		{
			UIMessageManager.AddListener(UIMessageType.Reset_Resolution, OnGetMsg);
		}

		void UnRegisterMsgHandler(){
			UIMessageManager.RemoveListener(UIMessageType.Reset_Resolution, OnGetMsg);
		}

		void OnGetMsg(MobaMessage msg){
			if(msg != null){
				screenWidth = Screen.width;
				screenHeight = Screen.height;
				SelectMode ();
			}
		}

		#endregion

		#region 

		/// <summary>
		/// Selects the mode.
		/// </summary>
		private void SelectMode ()
		{
//			ClientLogger.Info (mCamera.name + "-----------------> SelectMode " + screenWidth + " " + screenHeight);
			originalProjection = GetComponent<Camera>().projectionMatrix;  
			originalPixelRect = GetComponent<Camera>().pixelRect;
//			Debug.Log (mCamera.name + "-----------------> original projection = " + camera.projectionMatrix);
//			Debug.Log (mCamera.name + "-----------------> original pixelRect = " + camera.pixelRect);
//			Debug.Log (mCamera.name + "-----------------> original rect = " + camera.rect);
			switch (Model) {
			case AnchorModel.Auto:
				if (suitableUI_height != 0 && suitableUI_width != 0) {
					if (Mathf.Abs (Screen.width - suitableUI_width) > Mathf.Abs (Screen.height - suitableUI_height)) {
						scale = Screen.width / suitableUI_width;
					} else {
						scale = Screen.height / suitableUI_height;
					}
				} else
					scale = 1.0f;
				break;
			case AnchorModel.Tall:
				if (suitableUI_height != 0)
					scale = Screen.height / suitableUI_height;
				else
					scale = 1.0f;
				break;
			case AnchorModel.Width:
				if (suitableUI_width != 0)
					scale = Screen.width / suitableUI_width;
				else
					scale = 1.0f;
				break;
			default:
				return;
			}
			resolutionScale = scale;
			if (isNGUIHierarchy) {
				float uirootScale = ui_root.transform.localScale.x;
				scale = scale * (1 / uirootScale);
			}
//			Debug.Log (mCamera.name + "-----------------> resolutionScale = " + resolutionScale);
//			Debug.Log (mCamera.name + "-----------------> scale = " + scale);
			if (is3DCamera) {
				//pixelRectTODO
				Rect targetPixelRect;
				float height_diff = 0;
				float width_diff = 0;
				switch (Model) {
				case AnchorModel.Auto:
					targetPixelRect = originalPixelRect;
					height_diff = (originalPixelRect.height - suitableUI_height * scale) / 2;
					targetPixelRect.height = originalPixelRect.height - height_diff * 2;
					targetPixelRect.y = targetPixelRect.y + height_diff;
					width_diff = (originalPixelRect.width - suitableUI_width * scale) / 2;
					targetPixelRect.height = originalPixelRect.width - width_diff * 2;
					targetPixelRect.x = targetPixelRect.x + width_diff;
					GetComponent<Camera>().pixelRect = targetPixelRect;
					break;
				case AnchorModel.Tall:
					targetPixelRect = originalPixelRect;
					width_diff = (originalPixelRect.width - suitableUI_width * scale) / 2;
					targetPixelRect.height = originalPixelRect.width - width_diff * 2;
					targetPixelRect.x = targetPixelRect.x + width_diff;
					GetComponent<Camera>().pixelRect = targetPixelRect;
					break;
				case AnchorModel.Width:
					targetPixelRect = originalPixelRect;
					height_diff = (originalPixelRect.height - suitableUI_height * scale) / 2;
					targetPixelRect.height = originalPixelRect.height - height_diff * 2;
					targetPixelRect.y = targetPixelRect.y + height_diff;
					GetComponent<Camera>().pixelRect = targetPixelRect;
					break;
				}
//				Debug.Log (mCamera.name + "-----------------> target projection = " + camera.projectionMatrix);
//				Debug.Log (mCamera.name + "-----------------> target pixelRect = " + camera.pixelRect);
//				Debug.Log (mCamera.name + "-----------------> target rect = " + camera.rect);
			} else {
				//
				mCamera.projectionMatrix = OrthographicOffCenter (0, mCamera.pixelWidth, 0, mCamera.pixelHeight, mCamera.nearClipPlane, mCamera.farClipPlane);
//				Debug.Log (mCamera.name + "-----------------> projectionMatrix = " + mCamera.projectionMatrix);
			}
		}

		/// <summary>
		/// Orthographics the off center.
		/// </summary>
		/// <returns>The off center.</returns>
		/// <param name="left">Left.</param>
		/// <param name="right">Right.</param>
		/// <param name="bottom">Bottom.</param>
		/// <param name="top">Top.</param>
		/// <param name="near">Near.</param>
		/// <param name="far">Far.</param>
		private Matrix4x4 OrthographicOffCenter (float left, float right, float bottom, float top, float near, float far)
		{
//			Debug.Log (mCamera.name + "-----------------> left = " + left);
//			Debug.Log (mCamera.name + "-----------------> right = " + right);
//			Debug.Log (mCamera.name + "-----------------> bottom = " + bottom);
//			Debug.Log (mCamera.name + "-----------------> top = " + top);
//			Debug.Log (mCamera.name + "-----------------> near = " + near);
//			Debug.Log (mCamera.name + "-----------------> far = " + far);
			float x = (2.0f) / (right - left) * scale;
			float y = (2.0f) / (top - bottom) * scale;
			float z = -2.0f / (far - near);
			float a = 0;
			float b = 0;
			//float c = -2.0f * far * near / (far - near);
			float c = 0;
			if (isNGUIHierarchy)
				c = 0;
			else
				c = -1;			
//			Debug.Log (mCamera.name + "-----------------> x = " + x);
//			Debug.Log (mCamera.name + "-----------------> y = " + y);
//			Debug.Log (mCamera.name + "-----------------> z = " + z);
//			Debug.Log (mCamera.name + "-----------------> a = " + a);
//			Debug.Log (mCamera.name + "-----------------> b = " + b);
//			Debug.Log (mCamera.name + "-----------------> c = " + c);
            
			Matrix4x4 m = new Matrix4x4 ();
			m [0, 0] = x;
			m [0, 1] = 0;
			m [0, 2] = 0;
			m [0, 3] = a;
			m [1, 0] = 0;
			m [1, 1] = y;
			m [1, 2] = 0;
			m [1, 3] = b;
			m [2, 0] = 0;
			m [2, 1] = 0;
			m [2, 2] = z;
			m [2, 3] = c;
			m [3, 0] = 0;
			m [3, 1] = 0;
			m [3, 2] = 0;
			m [3, 3] = 1;
			return m;
		}

		/// <summary>
		/// Perspectives the off center.
		/// </summary>
		/// <returns>The off center.</returns>
		/// <param name="left">Left.</param>
		/// <param name="right">Right.</param>
		/// <param name="bottom">Bottom.</param>
		/// <param name="top">Top.</param>
		/// <param name="near">Near.</param>
		/// <param name="far">Far.</param>
		private Matrix4x4 PerspectiveOffCenter (float left, float right, float bottom, float top, float near, float far)
		{
//			Debug.Log (mCamera.name + "-----------------> left = " + left);
//			Debug.Log (mCamera.name + "-----------------> right = " + right);
//			Debug.Log (mCamera.name + "-----------------> bottom = " + bottom);
//			Debug.Log (mCamera.name + "-----------------> top = " + top);
//			Debug.Log (mCamera.name + "-----------------> near = " + near);
//			Debug.Log (mCamera.name + "-----------------> far = " + far);
			float x = 2.0F * near / (right - left);
			float y = 2.0F * near / (top - bottom);
			float a = (right + left) / (right - left);
			float b = (top + bottom) / (top - bottom);
			float c = -(far + near) / (far - near);
			float d = -(2.0F * far * near) / (far - near);
			float e = -1.0F;
//			Debug.Log (mCamera.name + "-----------------> x = " + x);
//			Debug.Log (mCamera.name + "-----------------> y = " + y);
//			Debug.Log (mCamera.name + "-----------------> a = " + a);
//			Debug.Log (mCamera.name + "-----------------> b = " + b);
//			Debug.Log (mCamera.name + "-----------------> c = " + c);
//			Debug.Log (mCamera.name + "-----------------> d = " + d);
//			Debug.Log (mCamera.name + "-----------------> e = " + e);
			Matrix4x4 m = new Matrix4x4 ();
			m [0, 0] = x;
			m [0, 1] = 0;
			m [0, 2] = a;
			m [0, 3] = 0;
			m [1, 0] = 0;
			m [1, 1] = y;
			m [1, 2] = b;
			m [1, 3] = 0;
			m [2, 0] = 0;
			m [2, 1] = 0;
			m [2, 2] = c;
			m [2, 3] = d;
			m [3, 0] = 0;
			m [3, 1] = 0;
			m [3, 2] = e;
			m [3, 3] = 0;
			return m;
		}

		/// <summary>
		/// Gets the test matrix.
		/// </summary>
		/// <returns>The test matrix.</returns>
		private Matrix4x4 GetTestMatrix ()
		{
			Matrix4x4 m = new Matrix4x4 ();
			m [0, 0] = 2.10245f; //x 
			m [0, 1] = 0;
			m [0, 2] = 0;
			m [0, 3] = 0; //a
			m [1, 0] = 0;
			m [1, 1] = 3.73205f; //y=3.73205f
			m [1, 2] = 0; //b
			m [1, 3] = 0;
			m [2, 0] = 0;
			m [2, 1] = 0;
			m [2, 2] = -1.00602f; //c
			m [2, 3] = -0.60181f; //d
			m [3, 0] = 0;
			m [3, 1] = 0;
			m [3, 2] = -1; //e
			m [3, 3] = 0;
			return m;
		}

		#endregion
	}
}