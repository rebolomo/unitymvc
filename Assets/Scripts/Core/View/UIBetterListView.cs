using UnityEngine;

namespace UnityMVC.Core.View
{
	//[RequireComponent(typeof(UIGrid))]
	//[RequireComponent(typeof(UICenterOnChild))]
	public class UIBetterListView : MonoBehaviour
	{
		#region 
		/// <summary>
		/// Width or height of the child items for positioning purposes.
		/// </summary>
		public int itemSize = 100;
		/// <summary>
		/// 
		/// </summary>
		public int dataCount = -1;
		/// <summary>
		/// item
		/// </summary>
		public int itemCount = -1;
		/// <summary>
		/// 
		/// </summary>
		public bool mHorizontal = false;
		/// <summary>
		/// 
		/// </summary>
		public bool mAuto = false;

		/// <summary>
		/// Whether the content will be automatically culled. Enabling this will improve performance in scroll views that contain a lot of items.
		/// </summary>
		private bool cullContent = false;
		private bool OpenLog = false;
		#endregion

        #region 
		Transform m_Trans;

		public Transform mTrans {
			get {
				if (m_Trans == null) {
					m_Trans = transform;
				}
				return m_Trans;
			}
		}

		UIPanel m_Panel;

		public UIPanel mPanel {
			get {
				if (m_Panel == null) {
					m_Panel = NGUITools.FindInParents<UIPanel> (gameObject);
				}
				return m_Panel;
			}
		}

		UIScrollView m_Scroll;

		public UIScrollView mScroll {
			get {
				if (m_Scroll == null) {
					m_Scroll = mPanel.GetComponent<UIScrollView> ();
				}
				return m_Scroll;
			}
		}

		UIGrid m_Grid = null;
		UICenterOnChild m_Center;

		public UIGrid mGrid {
			get {
				if (m_Grid == null) {
					//m_Grid = transform.GetComponent<UIGrid>();
					//if (m_Grid == null)
					//{
					//    m_Grid = transform.gameObject.AddComponent<UIGrid>();
					//}
					//m_Center = transform.GetComponent<UICenterOnChild>();
					//if (m_Center == null)
					//{
					//    m_Center = transform.gameObject.AddComponent<UICenterOnChild>();
					//}
				}
				return m_Grid;
			}
		}
        #endregion

        #region Item
		/// <summary>
		/// Item
		/// </summary>
		BetterList<Transform> mChildren = new BetterList<Transform> ();
		/// <summary>
		/// ItemDataIndexItem
		/// </summary>
		BetterList<int> mChildrenDataIndex = new BetterList<int> ();
		/// <summary>  
		///   
		/// </summary>  
		private int m_startIndex = 0;
		/// <summary>  
		///   
		/// </summary>  
		private int m_MaxCount;
		/// <summary>
		/// 
		/// </summary>
		private int m_dataIndex = -1;
		/// <summary>
		/// 
		/// </summary>
		private int m_seekIndex = 0;
        #endregion

        #region 
		/// <summary>  
		///   
		/// </summary>  
		/// <param name="go"></param>  
		public delegate void OnItemChange (Transform go,int targetIndex);

		private OnItemChange m_pItemChangeCallBack;

		/// <summary>  
		/// item  
		/// </summary>  
		/// <param name="go"></param>  
		/// <param name="i"></param>  
		public delegate void OnClickItem (Transform go,int i);

		private OnClickItem m_pOnClickItemCallBack;
        #endregion

        #region 
//        private static Object mLock = new Object();
        #endregion

		/// <summary>
		/// Initialize everything and register a callback with the UIPanel to be notified when the clipping region moves.
		/// </summary>
		protected void Start ()
		{
			if (mAuto) {
				InitView (dataCount, itemCount, m_seekIndex);
			}
		}

		/// <summary>
		/// Callback triggered by the UIPanel when its clipping region moves (for example when it's being scrolled).
		/// </summary>
		protected virtual void OnMove (UIPanel panel)
		{
			WrapContent ();
		}

		/// <summary>
		/// 
		/// </summary>
		public void OnUpdate ()
		{
			WrapContent ();
		}

		/// <summary>
		/// Immediately reposition all children.
		/// </summary>

		[ContextMenu("Sort Based on Scroll Movement")]
		public void SortBasedOnScrollMovement (int seekIndex = 0)
		{
			//FPSView.Instance.BeginSample("SortBased");
			// Cache all children and place them in order
			mChildren.Clear ();
			mChildrenDataIndex.Clear ();
			for (int i = 0; i < mTrans.childCount; ++i) {
				if (i < itemCount) {
					mChildren.Add (mTrans.GetChild (i));
					mChildrenDataIndex.Add (seekIndex + i);
				} else {
					mTrans.GetChild (i).gameObject.SetActive (false); //item
				}
			}
			//FPSView.Instance.EndSample("SortBased");

			CachePanel ();
			CacheScrollView ();
			CacheGridView ();

			//
			ResetChildPositions ();

			//if (!CacheScrollView()) return;
			// UIGrid
			// //Sort the list of children so that they are in order
			//if (mHorizontal)
			//{
			//    mChildren.Sort(UIGrid.SortHorizontal);
			//}
			//else
			//{
			//    mChildren.Sort(UIGrid.SortVertical);
			//}
			//ResetChildPositions();
		}

		/// <summary>
		/// Cache the scroll view and return 'false' if the scroll view is not found.
		/// </summary>

		protected bool CachePanel ()
		{
			mPanel.onClipMove = OnMove;
			return true;
		}

		protected bool CacheScrollView ()
		{
			if (mScroll == null) {
				return false;
			}
			mScroll.restrictWithinPanel = true;
			mScroll.dragEffect = UIScrollView.DragEffect.MomentumAndSpring;
			if (mScroll.movement == UIScrollView.Movement.Horizontal) {
				mHorizontal = true;
			} else if (mScroll.movement == UIScrollView.Movement.Vertical) {
				mHorizontal = false;
			} else {
				return false;
			}
			return true;
		}

		protected bool CacheGridView ()
		{
			if (mGrid == null) {
				return false;
			}

			if (mHorizontal) {
				mGrid.arrangement = UIGrid.Arrangement.Horizontal;
				mGrid.cellWidth = itemSize;
			} else {
				mGrid.arrangement = UIGrid.Arrangement.Vertical;
				mGrid.cellHeight = itemSize;
			}
			return true;
		}

		/// <summary>
		/// Helper function that resets the position of all the children.
		/// </summary>
		public void ResetChildPositions ()
		{
			//FPSView.Instance.BeginSample("ResetChildPositions");
			for (int i = 0; i < mChildren.size; ++i) {
				Transform t = mChildren [i];
				t.localPosition = mHorizontal ? new Vector3 (i * itemSize, 0f, 0f) : new Vector3 (0f, -i * itemSize, 0f);
			}
			if (mScroll != null) {
				mScroll.ResetPosition ();
			}
			//FPSView.Instance.EndSample("ResetChildPositions");
			//GridReposition18ms
			//mGrid.Reposition(); 
		}

		/// <summary>
		/// WrapContent
		/// : 1, 2, 3, 4, 5 ->(wrap content) -> 5, 1, 2, 3, 4
		/// : 1, 2, 3, 4, 5 -> (wrap content x) -> 5, 4, 1, 2, 3
		/// WrapItemItem
		/// </summary>
		public void WrapContent ()
		{
			//FPSView.Instance.BeginSample("WrapContent");
			//Debug.Log("==> WrapContent !!!");
			float extents = itemSize * mChildren.size * 0.5f;
			Vector3[] corners = mPanel.worldCorners;

			for (int i = 0; i < 4; ++i) {
				Vector3 v = corners [i];
				v = mTrans.InverseTransformPoint (v);
				corners [i] = v;
			}
			Vector3 center = Vector3.Lerp (corners [0], corners [2], 0.5f);

			if (mHorizontal) {
				float min = corners [0].x - itemSize;
				float max = corners [2].x + itemSize;

				for (int i = 0; i < mChildren.size; ++i) {
					Transform t = mChildren [i];
					float distance = t.localPosition.x - center.x;

					if (distance < -extents) { //
						if (OpenLog)
							Debug.Log ("==>UpdateItem 1 !!!");
						UpdateItem (false, t, i);
						distance = t.localPosition.x - center.x;
					} else if (distance > extents) { //
						if (OpenLog)
							Debug.Log ("==>UpdateItem 2 !!!");
						UpdateItem (true, t, i);
						distance = t.localPosition.x - center.x;
					}

					if (cullContent) {
						distance += mPanel.clipOffset.x - mTrans.localPosition.x;
						if (!UICamera.IsPressed (t.gameObject))
							NGUITools.SetActive (t.gameObject, (distance > min && distance < max), false);
					}
				}
			} else {
				float min = corners [0].y - itemSize;
				float max = corners [2].y + itemSize;

				for (int i = 0; i < mChildren.size; ++i) {
					Transform t = mChildren [i];
					float distance = t.localPosition.y - center.y;

					if (distance < -extents) { //
						if (OpenLog)
							Debug.Log ("==>UpdateItem 1 !!!");
						UpdateItem (true, t, i);
						distance = t.localPosition.y - center.y;
					} else if (distance > extents) { //
						if (OpenLog)
							Debug.Log ("==>UpdateItem 2 !!!");
						UpdateItem (false, t, i);
						distance = t.localPosition.y - center.y;
					}

					if (cullContent) {
						distance += mPanel.clipOffset.y - mTrans.localPosition.y;
						if (!UICamera.IsPressed (t.gameObject))
							NGUITools.SetActive (t.gameObject, (distance > min && distance < max), false);
					}
				}
			}
			//FPSView.Instance.EndSample("WrapContent");
		}

		/// <summary>
		/// Want to update the content of items as they are scrolled? Override this function.
		/// </summary>
		protected void UpdateItem (bool firstVislable, Transform item, int activeIndex)
		{
			int _sourceIndex = -1;
			int _targetIndex = -1;
			int _sign = 0;

			if (firstVislable) { //Item
				_sourceIndex = activeIndex;//mChildren.size - 1;
				_targetIndex = 0;
				_sign = 1;
			} else {  //Item
				_sourceIndex = activeIndex;//0;
				_targetIndex = mChildren.size - 1;
				_sign = -1;
			}
			if (OpenLog)
				Debug.Log ("==>UpdateItem : _sourceIndex = " + _sourceIndex + ", _targetIndex = " + _targetIndex);

			// ,  
			int realSourceIndex = mChildrenDataIndex [_sourceIndex];
			int realTargetIndex = mChildrenDataIndex [_targetIndex];

			if (OpenLog)
				Debug.Log ("==>UpdateItem : realSourceIndex = " + realSourceIndex + ", realTargetIndex = " + realTargetIndex);

			if ((firstVislable && realTargetIndex <= m_startIndex) || 
				(!firstVislable && realTargetIndex >= (m_MaxCount - 1))) {
				if (OpenLog)
					Debug.Log ("==>UpdateItem : FALSE 1");
				return; //, 
			}

			//
			int dataIndex = realSourceIndex > realTargetIndex ? (realTargetIndex - 1) : (realTargetIndex + 1);
			if (OpenLog)
				Debug.Log ("==>UpdateItem : m_dataIndex = " + m_dataIndex);
			//if (!checkDataIndex(dataIndex))
			//{
			//    if (OpenLog) Debug.Log("==>UpdateItem : FALSE 2");
			//    return; //
			//}

			m_dataIndex = dataIndex;
			//item
			if (mHorizontal) {
				if (_sign > 0) { //
					item.localPosition = mChildren [_targetIndex].localPosition - new Vector3 (itemSize, 0f, 0f);
					//item.localPosition -= new Vector3(extents * 2f, 0f, 0f);
				} else if (_sign < 0) {  //
					item.localPosition = mChildren [_targetIndex].localPosition + new Vector3 (itemSize, 0f, 0f);
					//item.localPosition += new Vector3(extents * 2f, 0f, 0f);
				}
			} else {
				if (_sign > 0) { //
					item.localPosition = mChildren [_targetIndex].localPosition + new Vector3 (0f, itemSize, 0f);
					//item.localPosition -= new Vector3(0f, extents * 2f, 0f);
				} else if (_sign < 0) { //
					item.localPosition = mChildren [_targetIndex].localPosition - new Vector3 (0f, itemSize, 0f);
					//item.localPosition += new Vector3(0f, extents * 2f, 0f);
				}
			}

			//Item
			Transform movedTarget = mChildren [_sourceIndex];
			mChildren.RemoveAt (_sourceIndex);
			mChildren.Insert (_targetIndex, movedTarget);
			if (OpenLog)
				Debug.Log ("==> " + this.name + "UpdateItem : item.localPosition   = " + item.localPosition);

			//Item
			mChildrenDataIndex.RemoveAt (_sourceIndex);
			mChildrenDataIndex.Insert (_targetIndex, m_dataIndex);

			//item
			if (m_pItemChangeCallBack != null) {
				if (checkDataIndex (m_dataIndex)) {
					m_pItemChangeCallBack (item, m_dataIndex);
				}
			}
		}

		protected void ClickItem (Transform item, int index)
		{
			if (m_pOnClickItemCallBack != null) {
				m_pOnClickItemCallBack (item, index);
			}
		}

        #region 

		/// <summary>
		/// Items
		/// </summary>
		/// <param name="_dataCount">_data count.</param>
		/// <param name="_itemCount">_item count.</param>
		/// <param name="_seekIndex">_seek index.</param>
		public void InitView (int _dataCount, int _itemCount, int _seekIndex = 0) //ListView
		{
			if (_dataCount < 0 || _itemCount < 0 || _itemCount > _dataCount) {
				if (OpenLog)
					Debug.Log ("==> InitListView !! !");
				return;
			}

			if (OpenLog)
				Debug.Log ("==> InitListView !! dataCount = " + _dataCount + " _seekIndex = " + _seekIndex);
			m_dataIndex = -1;
			itemCount = _itemCount;
			dataCount = _dataCount;
			m_MaxCount = dataCount;
			if (_seekIndex > 0) {
				Seek (_seekIndex);
			} else {
				Seek (0);
			}
		}

		public int GetItemDataIndex (int targetIndex)
		{
			if (targetIndex >= 0 && targetIndex < mChildrenDataIndex.size) {
				int dataIndex = mChildrenDataIndex [targetIndex];
				return dataIndex;
			}
			return -1;
		}
		/// <summary>  
		///   
		/// </summary>  
		/// <param name="_onItemChange"></param>  
		/// <param name="_onClickItem"></param>  
		public void SetDelegate (OnItemChange _onItemChange,
                                             OnClickItem _onClickItem)
		{
			m_pItemChangeCallBack = _onItemChange;

			if (_onClickItem != null) {
				m_pOnClickItemCallBack = _onClickItem;
			}
		}
		/// <summary>
		/// ListViewdataIndex, ListItem
		/// </summary>
		/// <param name="index"></param>
		public void Seek (int seekIndex)
		{
			if (seekIndex < 0 || seekIndex > m_MaxCount - 1) {
				if (OpenLog)
					Debug.Log ("==> Seek : !!!");
				return; //
			}

			mAuto = false; //
			//
			m_seekIndex = seekIndex;
			if (OpenLog)
				Debug.Log ("==> Seek : m_seekIndex = " + m_seekIndex + ", itemCount = " + itemCount + ", m_MaxCount = " + m_MaxCount);
			SortBasedOnScrollMovement (m_seekIndex);         
			WrapContent ();
			//
			if (m_pItemChangeCallBack != null) {
				for (int i = 0; i < mChildren.size; i++) {
					if (checkDataIndex (mChildrenDataIndex [i])) {
						m_pItemChangeCallBack (mChildren [i], mChildrenDataIndex [i]);
					}
				}
			}
			//mAuto = true;
		}

		protected bool checkDataIndex (int dataIndex)
		{
			if (dataIndex >= 0 && dataIndex <= m_MaxCount - 1) {
				return true;
			} else {
				return false;
			}
		}
        #endregion
	}
}
