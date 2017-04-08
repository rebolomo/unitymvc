////////////////////////////////////////////////////
//// File Name :        BaseView.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :      2015.8.24
//// Content :           MonoBehaviour
////////////////////////////////////////////////////
//#define DEBUG_BASEVIEW
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using UnityMVC.Utils;
using System.Reflection;
using UnityMVC.Core.Model;
using UnityMVC.Message;

namespace UnityMVC.Core.View
{
	/// <summary>
	/// UIPanel Depth
	/// </summary>
	public enum ViewLayer
	{
		NoneLayer = -1, //,panel depth
		BaseLayer = 0,  //
		LowLayer = 1,// 
		MiddleLayer = 2,  // TopView
		HighLayer = 4,  // TopView
		GuideLayer = 5, //
		TopLayer = 6,  //
	}

	/// <summary>
	/// UIPanel
	/// 3000 = 
	/// </summary>
	public enum ViewRenderQueue
	{
		None,
		BackUIQueue = 2000, //
		ParticleQueue = 3000, //
		TopUIQueue = 4000, //
		TopTopUIQueue = 5000, //
	}

	/// <summary>
	/// UIPanelZ
	/// Z
	/// -XX XX
	/// </summary>
	public enum ViewZType
	{
		None, // 0
		BaseZ = 1, //  -100
		LowerZ = 2, //-300
		MiddleZ = 3, //-500
		HighZ = 4, //-800
		TopZ = 5, //-1000
	}

	/// <summary>
	/// 
	/// </summary>
	public enum ViewType
	{
		NormalView = 0, //UI-
		BattleView = 1,//UI
		HomeView = 3,//UI-
		SummonerView = 4,//VIP
		MenuView = 5,//
		LoginView = 6,//
	}

	// 
	public class BaseView<T> : Singleton<T>, IView where T : new()
	{
        #region implemented abstract members of Singleton

		public override void SingletonCreate ()
		{
			throw new NotImplementedException ();
		}

		public override void SingletonDestroy ()
		{
			throw new NotImplementedException ();
		}

        #endregion

		#region 

		/// <summary>
		/// The m game object.
		/// </summary>
		private GameObject mGameObject;

		public GameObject gameObject {
			get {
				return mGameObject;
			}
			set {
				//View  
				//Init
				//
				if (mGameObject == null && value != null) {
					mGameObject = value;
//					transform = mGameObject.transform;
					if (!isInit) { //
						Init ();
						isInit = true;
					}
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="BaseView{T}"/> is open view.
		/// </summary>
		/// <value><c>true</c> if is open view; otherwise, <c>false</c>.</value>
		public bool isOpenView {
			get {
				return gameObject != null && gameObject.activeInHierarchy;
			}
		}

		/// <summary>
		/// Gets the transform.
		/// </summary>
		/// <value>The transform.</value>
		public Transform transform {
			get {
				if (gameObject != null) {
					return gameObject.transform;
				}
				return null;
			}
		} 

		/// <summary>
		/// Gets a value indicating whether this instance is open.
		/// </summary>
		/// <value><c>true</c> if this instance is open; otherwise, <c>false</c>.</value>
		public bool IsOpen {
			get { return gameObject && gameObject.activeInHierarchy; }
		}

		#endregion

		#region 

		public virtual ViewLayer layerType { get { return ViewLayer.NoneLayer; } } //
		public virtual ViewRenderQueue queueType { get { return ViewRenderQueue.None; } } //
		public virtual ViewZType zType { get { return ViewZType.None; } } //
		public virtual bool IsFullUI { get { return false; } }  //UIStaticUI
		public virtual bool IsStatic { get { return false; } }  //UI
		public virtual bool isDestroy { get { return true; } }  //UI
		public virtual bool isUnloadDelay { get { return false; } } //
		public virtual bool isAsyncLoad { get { return true; } }// AssetBundle.LoadAsync
		public virtual bool playClosedSound { get { return false; } } //
		public virtual string url { get { return null; } } //
		public virtual string viewName { get { return null; } }//
		public virtual bool isAssetbundle { get { return true; } }//
		public virtual bool isLoadFromConfig { get { return true; } }//
		public virtual bool waiting { get { return false; } } //
		public virtual ViewType viewType { get { return ViewType.NormalView; } } //UI
		public virtual bool isWaitingBeforeCloseView { get { return false; } }  //

		#endregion

		#region 

		protected bool isInit = false; // 
		protected bool firstOpen = true;    //OpenViewView
		private bool isSecondOpen = false;
		private bool isForceClose = false; //
		private bool isPreload = false; //
		private TimeoutController timeout;
		private bool openState = false; //View  
		protected List<string> prefabCacheKeys = new List<string> ();
		protected float delayUnloadTime = 15f; //15
		protected EventDelegate.Callback onHandlerBeforeCloseView; //
		private TweenScale tweenShow;
		private TweenAlpha tweenHide;

		#endregion

		#region 

		/// <summary>
		///    ,AssetBundle
		/// </summary>
		protected virtual void Init ()
		{
		}

		/// <summary>
		/// UI  View  , 
		/// </summary>
		protected virtual void HandleAfterOpenView ()
		{
		}

		/// <summary>
		/// UI View    ,CloseView
		/// </summary>
		protected virtual void HandleBeforeCloseView ()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void PreloadView ()
		{
			isPreload = true;
			OpenView ();
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void OpenView ()
		{
			ClientLogger.Debug ("View", "====> View.OpenView... " + GetType ());
			if (gameObject == null) { //gameObject
				LoadView ();
			} else { //UI
				//UIMonitor.one.ui = gameObject.name;
				if (!gameObject.activeInHierarchy) {
					gameObject.SetActive (true);
					OpenViewHelp ();
				}
			}
		}

		/// <summary>
		/// Forces the close view.
		/// </summary>
		public virtual void ForceCloseView (bool showPrevFullUI)
		{
			isForceClose = true;
			openState = false;
			//	REBOL note,  CloseViewTopViewback button delegate
			//	back buttonview
			//	MobaMessageManagerRegisMessageUnRegisMessagelog
			//ViewManager.UnRegister (this);//View
			isInit = false;
			CloseView (showPrevFullUI);
		}

		//
		public virtual void CloseView (bool showPrevFullUI = true)
		{
			if (gameObject != null) { // && gameObject.activeInHierarchy
				ClientLogger.Debug ("View", "====> View.CloseView " + gameObject.name);
				openState = false;
				if (isWaitingBeforeCloseView) {
					//set callback
					onHandlerBeforeCloseView = CloseViewHelp;
				}
				HandleBeforeCloseView (); //
				CancelUpdateHandler (); //
				ViewManager.UnRegister (this, showPrevFullUI);//View
				if (isWaitingBeforeCloseView) {
					//wait for callback
				} else {
					CloseViewHelp ();
				}
				if (playClosedSound) {
					//SoundMgr.Instance.PlayUIAudio(SoundId.Sound_ConfirmClose);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void Update ()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void Destroy ()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void RegisterUpdateHandler ()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="code">Code.</param>
		public virtual void DataUpdated (object sender, int code)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns><c>true</c> if this instance cancel update handler; otherwise, <c>false</c>.</returns>
		public virtual void CancelUpdateHandler ()
		{
		}

		#endregion

		#region 

		/// <summary>
		/// Sets the layer recursively.
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="layer">Layer.</param>
		public void SetLayerRecursively (Transform parent, int layer)
		{
			parent.gameObject.layer = layer;
			foreach (Transform child in parent) {
				SetLayerRecursively (child, layer);
			}
		}

		/// <summary>
		/// Find a child with path in hierarchy T
		/// </summary>
		/// <returns>The in child.</returns>
		/// <param name="path">Path.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T FindInChild<T> (string path = null) where T : Component
		{
			if (string.IsNullOrEmpty (path)) {
				return gameObject.GetComponent<T> ();
			} else
				return NGUITools.FindInChild<T> (gameObject, path);
		}

		/// <summary>
		/// Finds the child.
		/// </summary>
		/// <returns>The child.</returns>
		/// <param name="path">Path.</param>
		public GameObject FindChild (string path)
		{
			if (transform.Find (path))
				return transform.Find (path).gameObject;
			else
				return null;
		}

		#endregion

		#region 

		/// <summary>
		/// View
		/// </summary>
		private void LoadView ()
		{
			if (isInit) {
				isSecondOpen = true;
				return;
			}
			isInit = true;
			if (waiting && !this.GetType ().Equals (Singleton<WaitingView>.Instance.GetType ())) {
				Singleton<WaitingView>.Instance.OpenView ();
			}
			//if (isLoadFromConfig) {
			//	LoadViewCallBack (ResourceManager.Load<GameObject> (url, true, isAssetbundle) as GameObject);
			//} else {
				LoadViewCallBack (Resources.Load (url) as GameObject); //
			//}
		}
		
		/// <summary>
		///  AssetBundle 
		/// </summary>
		/// <param name="prefab">Prefab.</param>
		protected void LoadViewCallBack (GameObject prefab)
		{
			if (viewType == ViewType.HomeView) {
				gameObject = NGUITools.AddChild (ViewTree.Instance.root, prefab); //Instantiate prefab
			} else if (viewType == ViewType.BattleView) {
				gameObject = NGUITools.AddChild (ViewTree.Instance.root, prefab); //Instantiate prefab
			} /*else if (viewType == ViewType.SummonerView) {
				if (ViewTree.Instance.summoner == null)
					ViewTree.Instance.summoner = ViewTree.Instance.root.transform.FindChild ("SummonerView/Anchor").gameObject;
				gameObject = NGUITools.AddChild (ViewTree.Instance.summoner, prefab); //Instantiate prefab
			} else if (viewType == ViewType.MenuView) {
				if (ViewTree.Instance.menu == null)
					ViewTree.Instance.menu = ViewTree.Instance.root.transform.FindChild ("MenuView").gameObject;
				gameObject = NGUITools.AddChild (ViewTree.Instance.menu, prefab); //Instantiate prefab
			} */else if (viewType == ViewType.LoginView) {
				gameObject = NGUITools.AddChild (ViewTree.Instance.root, prefab); //Instantiate prefab
			} else {
				gameObject = NGUITools.AddChild (ViewTree.Instance.root, prefab); //Instantiate prefab
			}
			//
			gameObject.name = viewName;
			//
			gameObject.layer = LayerMask.NameToLayer ("NGUI"); 
//			if (layerType == ViewLayer.HighLayer || layerType == ViewLayer.TopLayer) {
//				NGUITools.SetLayer (gameObject, LayerMask.NameToLayer ("Viewport"));
//			}
			ClientLogger.Debug ("View", "====> View.LoadViewCallBack " + gameObject.name);
			Init ();//View
			ViewManager.Add (this); //View
			
			//
			if (isSecondOpen) {
				firstOpen = false;
			} else {
				firstOpen = true;
			}
			//
			if (waiting && !this.GetType ().Equals (Singleton<WaitingView>.Instance.GetType ())) {
				Singleton<WaitingView>.Instance.CloseView ();
			}
			if (firstOpen && isPreload) {
				Update (); //View
				gameObject.SetActive (false); //
				return;
			} else {
				gameObject.SetActive (true);
			}
			//
			if (firstOpen) {
				OpenViewHelp ();
			}
		}

		/// <summary>
		/// UI
		/// </summary>
		/// <param name="go">Go.</param>
		/// <param name="showOrHide">If set to <c>true</c> show or hide.</param>
		/// <param name="callback">Callback.</param>
		private void PlayShowOrHideAnim (GameObject go, bool showOrHide, EventDelegate.Callback callback = null)
		{
			if (showOrHide) {
				tweenShow = go.GetComponent<TweenScale> ();
				if (tweenShow == null)
					tweenShow = go.AddComponent<TweenScale> ();
				tweenShow.from = Vector3.zero;
				tweenShow.to = Vector3.one;
				AnimationCurve curve = new AnimationCurve (new Keyframe (0, 0), new Keyframe (0.8f, 1.05f), new Keyframe (1, 1));
				tweenShow.animationCurve = curve;
				tweenShow.duration = 0.25f;
				tweenShow.Begin ();
				TweenAlpha.Begin (go, 0.2f, 1f);
				if (callback != null)
					EventDelegate.Add (tweenShow.onFinished, callback, true);
			} else {
				TweenScale.Begin (go, 0.1f, Vector3.one * 0.8f);
				tweenHide = TweenAlpha.Begin (go, 0.1f, 0);
				if (callback != null)
					EventDelegate.Add (tweenHide.onFinished, callback, true);
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		private void OpenViewHelp ()
		{
			openState = true;
			ViewManager.Register (this); //View
			RegisterUpdateHandler (); //
			HandleAfterOpenView (); //

			//
			if (isUnloadDelay) {
				StartOrStopDelayDestory ();
			}
			UIMessageManager.SendMsg(UIMessageType.UI_OpenView, viewName);
		}

		/// <summary>
		/// 
		/// </summary>
		private void CloseViewHelp ()
		{
			if (openState)  //  
				return;
			UIMessageManager.SendMsg(UIMessageType.UI_CloseView, viewName);
			gameObject.SetActive (false);
			if (isDestroy || isForceClose) {
				if (isUnloadDelay && !isForceClose) {
					StartDelayDestory ();
				} else {
					DoDestroy (); // 
				}
			}
		}

		/// <summary>
		/// Starts the or stop delay destory.
		/// </summary>
		private void StartOrStopDelayDestory ()
		{
			if (timeout == null) {
				timeout = gameObject.AddComponent<TimeoutController> ();
			} else {
				timeout.StopTimeOut ();
			}
		}
		
		/// <summary>
		/// Starts the delay destory.
		/// </summary>
		private void StartDelayDestory ()
		{
			if (timeout != null) {
				timeout.StartTimeOut (delayUnloadTime, DoDestroy); //
			}
		}
		
		/// <summary>
		/// Dos the destroy.
		/// </summary>
		private void DoDestroy ()
		{
			ClientLogger.Debug ("View", "====> View.Destroy...Destroy " + GetType ());
			ViewManager.Remove (this); //View
			Destroy (); //Views
			if (timeout != null) {
				GameObject.Destroy (timeout); //TimeOut
			}
			//UnloadPrefabCache (); //Prefab Cache
			if (gameObject != null) {
				GameObject.DestroyImmediate (gameObject); //
				gameObject = null; //
			}
			Resources.UnloadUnusedAssets ();//
			isInit = false;
			isForceClose = false;
			isPreload = false;
		}

		#endregion
	}
	
	/// <summary>
	/// 
	/// </summary>
	public interface IView
	{
		GameObject gameObject { get; set; }

		string viewName { get; }

		Transform transform { get; }

		ViewLayer layerType { get; }

		ViewRenderQueue queueType { get; }

		ViewZType zType { get; }

		bool IsFullUI { get; }

		bool IsStatic { get; }

		bool isUnloadDelay { get; }

		void ForceCloseView (bool showPrevFullUI);

		void CloseView (bool showPrevFullUI = true);

		void OpenView ();

		void PreloadView ();

		void Update ();

		void RegisterUpdateHandler ();

		void DataUpdated (object sender, int code);

		void CancelUpdateHandler ();
	}
	
	/// <summary>
	/// 
	/// </summary>
	public static class ViewManager
	{
		/// <summary>
		/// View
		/// </summary>
		public static List<IView> allViewList = new List<IView> ();

		/// <summary>
		/// View
		/// </summary>
		public static List<IView> openViewList = new List<IView> (); 

		/// <summary>
		/// view
		/// </summary>
		//public static List<Type> lastSceneViewTypeList = new List<Type> (); 

		/// <summary>
		/// View
		/// </summary>
		public static bool CheckOpenView(string viewName)
		{
			if(viewName != null){
				for (int i = openViewList.Count - 1; i >= 0; i--) {
					IView view = openViewList [i];
					if (view.viewName.Equals(viewName)) {
						return true;
					}
				}
			}
			return false;
		}
	
		/// <summary>
		/// Add the specified obj.
		/// </summary>
		/// <param name="obj">Object.</param>
		public static void Add (IView obj)
		{
			lock (allViewList) {
				AddView (obj);
			}
		}

		/// <summary>
		/// Remove the specified obj.
		/// </summary>
		/// <param name="obj">Object.</param>
		public static void Remove (IView obj)
		{
			lock (allViewList) {
				RemoveView (obj);
			}
		}

		/// <summary>
		/// Register the specified obj.
		/// </summary>
		/// <param name="obj">Object.</param>
		public static void Register (IView obj)
		{
			lock (openViewList) {
				//View,StaticViewUI
				if (obj.IsFullUI) {
					HidePrevFullUI (obj);
				}
				RegisterView (obj);
			}
		}

		/// <summary>
		/// Uns the register.
		/// </summary>
		/// <param name="obj">Object.</param>
		public static void UnRegister (IView obj, bool showPrevFullUI = true)
		{
			lock (openViewList) {
				UnRegisterView (obj);
				//View,StaticViewUI
				if (obj.IsFullUI && showPrevFullUI) {
					ShowPrevFullUI ();
				}
			}
		}

		/// <summary>
		/// UI
		/// </summary>
		/// <param name="topObj">Top object.</param>
		private static void HidePrevFullUI (IView topObj)
		{

			ClientLogger.Debug ("View", "====> HidePrevFullUI : " + openViewList.Count);

			for (int i = openViewList.Count - 1; i >= 0; i--) {
				IView view = openViewList [i];
				if (view != topObj && !view.IsStatic) {
					if (view.gameObject != null) {
						ClientLogger.Debug ("View", "==> HidePrevFullUI : " + view.gameObject.name);
						view.gameObject.SetActive (false);
					}
					view.CancelUpdateHandler ();
				}
			}
		}

		/// <summary>
		/// UI
		/// </summary>
		private static void ShowPrevFullUI ()
		{

			ClientLogger.Debug ("View", "====> ShowPrevFullUI : " + openViewList.Count);

			for (int i = openViewList.Count - 1; i >= 0; i--) {
				IView view = openViewList [i];
				//	REBOL note, view
				if (view.IsFullUI ) {
					if(!view.gameObject.activeSelf) {
						if (view.gameObject != null) {
							ClientLogger.Debug ("View", "==> ShowPrevFullUI : " + view.gameObject.name);
							view.gameObject.SetActive (true);
						}
						view.RegisterUpdateHandler ();
					}
					break;
				}

				if (!view.IsStatic) {
					if (view.gameObject != null && !view.gameObject.activeSelf) {
						ClientLogger.Debug ("View", "==> ShowPrevFullUI : " + view.gameObject.name);
						view.gameObject.SetActive (true);
					}
					view.RegisterUpdateHandler ();
				}
			}
		}

		/// <summary>
		/// UI
		/// </summary>
		public static void ClosePreAll ()
		{
			lock (openViewList) {

				ClientLogger.Debug ("View", "====> ClosePreAll : " + openViewList.Count);

				for (int i = openViewList.Count - 2; i >= 0; i--) {
					IView view = openViewList [i];
					if (view != null) {
						view.CloseView ();
					}
				}
			}
		}

		/// <summary>
		/// view
		/// </summary>
		public static void HideAllOpenViewWithoutExept (List<IView> exeptList = null)
		{
			lock (allViewList) {
				//openViewListBeforeCameraChange.Clear();
				//	REBOL note, view
				for (int i = 0; i < openViewList.Count; i++) {
					bool found = false;
					if(exeptList != null) {
						for(int j = 0 ; j < exeptList.Count; j++) {
							if(openViewList[i].gameObject == exeptList[j].gameObject) {
								found = true;
								break;
							}
						}
					}

					if(!found) {
						openViewList[i].gameObject.SetActive(false);
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void ShowAllOpenViewList ()
		{
			lock (allViewList) {
				//openViewListBeforeCameraChange.Clear();
				//	REBOL note, view
				for (int i = 0; i < openViewList.Count; i++) {
					openViewList[i].gameObject.SetActive(true);
				}
			}
		}

		/// <summary>
		/// Forces the close all.
		/// </summary>
		public static void CloseAllWithoutStatic (int sceneIndex)
		{
			lock (allViewList) {
				/*
				if (sceneIndex >= (int)SceneIndex.Map) {
					//	REBOL note, view
					for (int i = 0; i < openViewList.Count; i++) {
						Type viewType = openViewList [i].GetType ();
						lastSceneViewTypeList.Add (viewType);
					}
				}*/

				ClientLogger.Debug ("View", "====> CloseAllWithoutStatic : " + allViewList.Count);
				for (int i = allViewList.Count - 1; i >= 0; i--) {
					IView view = allViewList [i];
					if (view != null && !view.IsStatic) { //StaticView
						ClientLogger.Debug ("View", "==> CloseAllWithoutStatic : " + view.GetType ());
						//	REBOL note, viewfullui view
						view.ForceCloseView (false);
					}
				}
			}
		}

		/*
		/// <summary>
		/// view
		/// </summary>
		public static void RebuildUI ()
		{
			for (int i = 0; i < lastSceneViewTypeList.Count; i++) {
				Type viewType = lastSceneViewTypeList [i];
				//	REBOL note, parent inheritedFlattenHierarchy
				var methodInstance = viewType
					.GetProperty ("Instance", BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static);
				IView obj = (IView)methodInstance.GetValue (null, null);
				obj.OpenView ();
			}

			lastSceneViewTypeList.Clear ();
		}*/

		//	REBOL note, view
		public static void FlushViewsInLastScene() {
			//lastSceneViewTypeList.Clear ();
		}

		/// <summary>
		/// 
		/// </summary>
		public static void CloseAllOpenViews ()
		{
			lock (openViewList) {
				ClientLogger.Debug ("View", "====> CloseAllOpenViews : " + openViewList.Count);
				for (int i = 0; i < openViewList.Count; i++) {
					ClientLogger.Debug ("openViewList" + "[" + i + "]====>" + openViewList [i]);
					openViewList [i].CloseView ();
				}
				openViewList.Clear ();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void CloseAll ()
		{
			lock (allViewList) {
				ClientLogger.Debug ("View", "====> CloseAll : " + allViewList.Count);
				for (int i = allViewList.Count - 1; i >= 0; i--) {
					IView view = allViewList [i];
					if (view != null) { //StaticView
						ClientLogger.Debug ("View", "==> CloseAll : " + view.GetType ());
						//	REBOL note, viewfullui view
						view.ForceCloseView (false);
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void Update ()
		{
			lock (openViewList) {
				int length = openViewList.Count;
				for (int i = 0; i < length; i++) {
					int curLength = openViewList.Count;
					openViewList [i].Update ();
					length = openViewList.Count;
					if (curLength - length > 0) {
						i -= curLength - length;
						if (i < 0) {
							i = 0;
						}
					}
				}
			}
		}

		/// <summary>
		/// View
		/// </summary>
		/// <param name="view">View.</param>
		private static void RegisterView (IView view)
		{
			UIPanel[] panels = null;
			//depth
			if (view.layerType != ViewLayer.NoneLayer) {
				int depth = GetMaxDepth (view.layerType);
				depth++;
				if (panels == null)
					panels = view.gameObject.GetComponentsInChildren<UIPanel> (true);
				Array.Sort (panels, DepthCompareFunc);
				int lastDepth = -9999;
				foreach (UIPanel panel in panels) {
					if (panel.depth == lastDepth) {
						panel.depth = depth - 3;
					} else {
						lastDepth = panel.depth;
						panel.depth = depth;
						depth += 3;
					}
				}
			}
			//
			if (view.queueType != ViewRenderQueue.None) {
				UIPanel panel = view.gameObject.GetComponent<UIPanel> ();
				if (panel != null) {
					int queue = GetMaxRenderQueue (view.queueType);
					queue++;
					panel.renderQueue = UIPanel.RenderQueue.StartAt;
					panel.startingRenderQueue = queue;
				}
			}
			//Z
			if (view.zType != ViewZType.None) {
				switch (view.zType) {
				case ViewZType.BaseZ:
					view.transform.localPosition = new Vector3 (0, 0, -100);
					break;
				case ViewZType.LowerZ:
					view.transform.localPosition = new Vector3 (0, 0, -300);
					break;
				case ViewZType.MiddleZ:
					view.transform.localPosition = new Vector3 (0, 0, -500);
					break;
				case ViewZType.HighZ:
					view.transform.localPosition = new Vector3 (0, 0, -800);
					break;
				case ViewZType.TopZ:
					view.transform.localPosition = new Vector3 (0, 0, -1000);
					break;
				}
			}
			//	REBOL note, viewslevelview
			//	REBOL note, view
			if (openViewList.Contains (view)) {
				openViewList.Remove(view);
			}
			openViewList.Add (view);
		}

		/// <summary>
		/// Uns the register view.
		/// </summary>
		/// <param name="view">View.</param>
		private static void UnRegisterView (IView view)
		{
			if (openViewList.Contains (view)) {
				openViewList.Remove (view);
			}
		}

		/// <summary>
		/// Adds the view.
		/// </summary>
		/// <param name="view">View.</param>
		private static void AddView (IView view)
		{
			if (!allViewList.Contains (view)) {
				allViewList.Add (view);
			}
		}

		/// <summary>
		/// Removes the view.
		/// </summary>
		/// <param name="view">View.</param>
		private static void RemoveView (IView view)
		{
			if (allViewList.Contains (view)) {
				allViewList.Remove (view);
			}
		}

		/// <summary>
		/// Depths the compare func.
		/// </summary>
		/// <returns>The compare func.</returns>
		/// <param name="left">Left.</param>
		/// <param name="right">Right.</param>
		private static int DepthCompareFunc (UIPanel left, UIPanel right)
		{
			if (left.depth == right.depth)
				return 0;
			else if (left.depth > right.depth)
				return 1;
			else
				return -1;
		}

		/// <summary>
		/// 
		/// +1
		/// </summary>
		/// <returns>The max depth.</returns>
		/// <param name="layer">Layer.</param>
		private static int GetMaxDepth (ViewLayer layer)
		{
			int depth = 100 * (int)layer;
			int minDepth = 1000;
			int maxDepth = depth;
			foreach (IView view in openViewList) {
				if (view.layerType == layer && view.gameObject != null) {
					UIPanel[] panels = view.gameObject.GetComponentsInChildren<UIPanel> (true);
					foreach (UIPanel panel in panels) {
						if (maxDepth < panel.depth)
							maxDepth = panel.depth;
						if (minDepth > panel.depth)
							minDepth = panel.depth;
					}
				}
			}
			//
			foreach (IView view in openViewList) {
				if (view.layerType == layer && view.gameObject != null) {
					UIPanel[] panels = view.gameObject.GetComponentsInChildren<UIPanel> (true);
					foreach (UIPanel panel in panels) {
						panel.depth = panel.depth - minDepth + depth;
					}
				}
			}
			if (maxDepth - minDepth + depth < depth)
				return depth;
			else
				return maxDepth - minDepth + depth;
		}

		/// <summary>
		/// 
		/// +1
		/// </summary>
		/// <returns>The max depth.</returns>
		/// <param name="layer">Layer.</param>
		private static int GetMaxRenderQueue (ViewRenderQueue queue)
		{
			int depth = (int)queue;
			int minDepth = 0;
			int maxDepth = depth;
			foreach (IView view in openViewList) {
				if (view.queueType == queue) {
					UIPanel[] panels = view.gameObject.GetComponentsInChildren<UIPanel> (true);
					foreach (UIPanel panel in panels) {
						if (maxDepth < (int)panel.renderQueue)
							maxDepth = (int)panel.renderQueue;
						if (minDepth > (int)panel.renderQueue)
							minDepth = (int)panel.renderQueue;
					}
				}
			}
			return Mathf.Max (minDepth, maxDepth);
		}
	}
}
