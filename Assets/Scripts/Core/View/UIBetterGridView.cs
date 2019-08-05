using UnityEngine;
using System.Collections.Generic;

namespace UnityMVC.Core.View
{
	//[RequireComponent(typeof(UIGrid))]
	//[RequireComponent(typeof(UICenterOnChild))]
	public class UIBetterGridView : MonoBehaviour
	{
		#region 
		public enum Arrangement
		{
			Horizontal,
			Vertical
		}

		/// <summary>
		/// 
		/// </summary>
		public Arrangement arrangement = Arrangement.Horizontal;

		/// <summary>
		/// Width or height of the child items for positioning purposes.
		/// </summary>
		public int itemWidth = 100;

		/// <summary>
		/// Width or height of the child items for positioning purposes.
		/// </summary>
		public int itemHeight = 100;

		/// <summary>
		/// 
		/// </summary>
		public int maxPerLine = 1;

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
		public bool mAuto = false;

		/// <summary>
		/// Whether the content will be automatically culled. Enabling this will improve performance in scroll views that contain a lot of items.
		/// </summary>
		private bool cullContent = false;

		/// <summary>
		/// The open log.
		/// </summary>
		public bool OpenLog = false;
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

		protected bool mCalculatedBounds = false;
		protected Bounds mBounds;

		public Bounds bounds {
			get {
				if (!mCalculatedBounds) {
					mCalculatedBounds = true;
					mBounds = CalculateBounds ();
				}
				return mBounds;
			}
		}

        #endregion

        #region Item
		/// <summary>
		/// Item
		/// </summary>
		List<Transform> mChildren = new List<Transform> ();
		/// <summary>
		/// Item
		/// </summary>
		Dictionary<int, List<Transform>> mChildrenRow = new Dictionary<int, List<Transform>> ();
		/// <summary>
		/// ItemDataIndexItem
		/// </summary>
		List<int> mChildrenDataIndex = new List<int> ();
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
			mChildren.Clear ();
			mChildrenDataIndex.Clear ();
			mChildrenRow.Clear ();
			int count = itemCount;
			if(dataCount < itemCount)
				count = dataCount;
			for (int i = 0; i < mTrans.childCount; ++i) {
				Transform child = mTrans.GetChild (i);
				if (i < count) {
					mChildren.Add (child);
					mChildrenDataIndex.Add (seekIndex + i);
					int row = i / maxPerLine;
					if (i % maxPerLine == 0) {
						mChildrenRow.Add (row, new List<Transform> ());
					}
					mChildrenRow [row].Add (child);
				} else {
					child.gameObject.SetActive (false); //item
				}
			}
			if (OpenLog) {
				Debug.Log ("===================> Sort Start ");
				for (int n=0; n<mChildren.Count; n++) {
					Debug.Log ("mChildren : " + n + " " + mChildren [n].name);
				}
				for (int n=0; n<mChildrenRow.Count; n++) {
					Debug.Log ("mChildrenRow : " + n + " " + mChildrenRow [n][0].name);
                }
                Debug.Log ("===================> Sort End ");
			}

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
				arrangement = Arrangement.Horizontal;
			} else if (mScroll.movement == UIScrollView.Movement.Vertical) {
				arrangement = Arrangement.Vertical;
			} else {
				return false;
			}
			return true;
		}

		protected bool CacheGridView ()
		{
			return true;
		}

		/// <summary>
		/// Helper function that resets the position of all the children.
		/// </summary>
		public void ResetChildPositions ()
		{
			for (int i = 0; i < mChildren.Count; ++i) {
				Transform t = mChildren [i];
				//
				int row_offset_x = i / maxPerLine; 
				int row_offset_y = i % maxPerLine;
				t.localPosition = (arrangement == Arrangement.Horizontal) ? new Vector3 (row_offset_x * itemWidth, -row_offset_y * itemHeight, 0f) : new Vector3 (row_offset_y * itemWidth, -row_offset_x * itemHeight, 0f);
				if (OpenLog)
					Debug.Log ("==>ResetChildPositions : " + t.name + " " + row_offset_x + " " + row_offset_y + " " + t.localPosition);
			}
			if (mScroll != null) {
				mScroll.ResetPosition ();
			}
		}

		/// <summary>
		/// WrapContent
		/// : 1, 2, 3, 4, 5 ->(wrap content) -> 5, 1, 2, 3, 4
		/// : 1, 2, 3, 4, 5 -> (wrap content x) -> 5, 4, 1, 2, 3
		/// WrapItemItem
		/// </summary>
		List<Transform> t = new List<Transform> ();
        public void WrapContent ()
		{
			if (dataCount <= itemCount) {
				// do nothing... 
			} else {
				// 
				if (arrangement == Arrangement.Horizontal) {
					//
					float extents = itemWidth * mChildren.Count / maxPerLine * 0.5f;
					Vector3[] corners = mPanel.worldCorners;

					for (int i = 0; i < 4; ++i) {
						Vector3 v = corners [i];
						v = mTrans.InverseTransformPoint (v);
						corners [i] = v;
					}

					Vector3 center = Vector3.Lerp (corners [0], corners [2], 0.5f);
					float min = corners [0].x - itemWidth;
					float max = corners [2].x + itemWidth;
					for (int i = 0; i < mChildren.Count; i=i+maxPerLine) {
						t.Clear();
						for (int n = 0; n < maxPerLine; n++) {
							if (i + n < mChildren.Count) {
								t.Add (mChildren [i + n]);
                            }
                        }
                        //
                        float distance = t[0].localPosition.x - center.x;
						if (distance < -extents) { //
//						if (OpenLog)
//							Debug.Log ("======>UpdateItem Left : row = " + i + " distance = " + distance + " extents = " + -extents);
							UpdateItem (false, t, i);
							distance = t[0].localPosition.x - center.x;
						} else if (distance > extents) { //
//						if (OpenLog)
//							Debug.Log ("======>UpdateItem Right : row = " + i + " distance = " + distance + " extents = " + extents);
							UpdateItem (true, t, i);
							distance = t[0].localPosition.x - center.x;
						}
					}
				} else {
					// 
					float extents = itemHeight * (mChildren.Count / maxPerLine) * 0.5f;
					Vector3[] corners = mPanel.worldCorners;
					for (int i = 0; i < 4; ++i) {
						Vector3 v = corners [i];
						v = mTrans.InverseTransformPoint (v);
						corners [i] = v;
//					if (OpenLog)
//						Debug.Log ("-------------->corners +" + i +"  : " + corners[i]);
					}

					//Panel
					Vector3 center = Vector3.Lerp (corners [0], corners [2], 0.5f);
//				Vector3 center = new Vector3(221.0f, -141.0f, 0.0f);
//				if (OpenLog)
//					Debug.Log ("-------------->UpdateItem Vertical Center  : " + center);
					float min = corners [0].y - itemHeight;
					float max = corners [2].y + itemHeight;
					for (int i = 0; i < mChildren.Count; i=i+maxPerLine) {
						t.Clear();
						for (int n = 0; n < maxPerLine; n++) {
							if (i + n < mChildren.Count) {
                                t.Add (mChildren [i + n]);
                            }
                        }
						//
						float distance = t[0].localPosition.y - center.y;
//					if (OpenLog)
//						Debug.Log ("==>UpdateItem Vertical : " + i + " " + t [0].name + " " + t [0].position + " distance = " + distance + " extents = " + extents);
						if (distance < -extents) { 
							//item
//						if (OpenLog)
//							Debug.Log ("======>UpdateItem Down : row = " + i + " distance = " + distance + " extents = " + -extents);
							UpdateItem (true, t, i);
							distance = t[0].localPosition.y - center.y;
						} else if (distance > extents) { 
							//item
//						if (OpenLog)
//							Debug.Log ("======>UpdateItem Up :row = " + i + " distance = " + distance + " extents = " + extents);
							UpdateItem (false, t, i);
							distance = t[0].localPosition.y - center.y;
						}
					}
				}
			}
//			if (OpenLog)
//				Debug.Log ("------------------------WrapContent End-----------------------------");
		}

		/// <summary>
		/// Want to update the content of items as they are scrolled? Override this function.
		/// </summary>
		protected void UpdateItem (bool firstVislable, List<Transform> item, int activeIndex)
		{
//			if (OpenLog)
//				Debug.Log ("=====================>UpdateItem : " + firstVislable + " " + item.Count + " " + activeIndex);
			int _sourceIndex = -1;
			int _targetIndex = -1;
			int _sign = 0;

			if (firstVislable) { //Item
				_sourceIndex = activeIndex;//mChildren.size - 1;
				_targetIndex = 0;
				_sign = 1;
			} else {  //Item
				_sourceIndex = activeIndex;//0;
				_targetIndex = mChildren.Count - maxPerLine;
				_sign = -1;
			}
//			if (OpenLog)
//				Debug.Log ("==>UpdateItem : _sourceIndex = " + _sourceIndex + ", _targetIndex = " + _targetIndex);

			int realSourceIndex = 0;
			// ,  
			if (_sourceIndex >= 0 && _sourceIndex < mChildrenDataIndex.Count) {
				realSourceIndex = mChildrenDataIndex [_sourceIndex];
			}
			int realTargetIndex = 0;
			if (_targetIndex >= 0 && _targetIndex < mChildrenDataIndex.Count) {
				realTargetIndex = mChildrenDataIndex [_targetIndex];
			}

//			if (OpenLog)
//				Debug.Log ("==>UpdateItem : realSourceIndex = " + realSourceIndex + ", realTargetIndex = " + realTargetIndex);

			if ((firstVislable && realTargetIndex <= m_startIndex) || 
				(!firstVislable && realTargetIndex >= (m_MaxCount - 1))) {
//				if (OpenLog)
//					Debug.Log ("==>UpdateItem : FALSE 1");
				return; //, 
			}

			//
			int dataIndex = realSourceIndex > realTargetIndex ? (realTargetIndex - maxPerLine) : (realTargetIndex + maxPerLine);
//			if (OpenLog)
//				Debug.Log ("==>UpdateItem : m_dataIndex = " + m_dataIndex);
			if (!checkDataIndex (dataIndex)) {
//				if (OpenLog)
//					Debug.Log ("==>UpdateItem : FALSE 2");
				return; //, 
			}

			if (OpenLog)
				Debug.Log ("=============>UpdateItem : TRUE !!!");

			if (OpenLog)
				Debug.Log ("==>UpdateItem : m_dataIndex = " + m_dataIndex);

			if (OpenLog)
				Debug.Log ("==>UpdateItem : _sourceIndex = " + _sourceIndex + ", _targetIndex = " + _targetIndex);

			m_dataIndex = dataIndex;
			//item
			if (arrangement == Arrangement.Horizontal) {
				if (_sign > 0) { //
					for (int i=0; i<item.Count; i++) {
						if (_targetIndex + i < mChildren.Count) {
							item [i].localPosition = mChildren [_targetIndex + i].localPosition - new Vector3 (itemWidth, 0f, 0f);
							if (OpenLog) {
								Debug.Log ("==>UpdateItem : " + (_sourceIndex + i) + " changed by " + (_targetIndex + i));
								Debug.Log ("==>UpdateItem : " + item [i].name + " " + item [i].localPosition);
							}
						}
					}
				} else if (_sign < 0) {  //
					for (int i=0; i<item.Count; i++) {
						if (_targetIndex + i < mChildren.Count) {
							item [i].localPosition = mChildren [_targetIndex + i].localPosition + new Vector3 (itemWidth, 0f, 0f);
							if (OpenLog) {
								Debug.Log ("==>UpdateItem : " + (_sourceIndex + i) + " changed by " + (_targetIndex + i));
								Debug.Log ("==>UpdateItem : " + item [i].name + " " + item [i].localPosition);
							}
						}
					}
				}
			} else {
				if (_sign > 0) { //
					for (int i=0; i<item.Count; i++) {
						if (_targetIndex + i < mChildren.Count) {
							item [i].localPosition = mChildren [_targetIndex + i].localPosition + new Vector3 (0f, itemHeight, 0f);
							if (OpenLog) {
								Debug.Log ("==>UpdateItem : " + (_sourceIndex + i) + " changed by " + (_targetIndex + i));
								Debug.Log ("==>UpdateItem : " + item [i].name + " " + item [i].localPosition);
							}
						}
					}
				} else if (_sign < 0) { //
					for (int i=0; i<item.Count; i++) {
						if (_targetIndex + i < mChildren.Count) {
							item [i].localPosition = mChildren [_targetIndex + i].localPosition - new Vector3 (0f, itemHeight, 0f);
							if (OpenLog) {
								Debug.Log ("==>UpdateItem : " + (_sourceIndex + i) + " changed by " + (_targetIndex + i));
								Debug.Log ("==>UpdateItem : " + item [i].name + " " + item [i].localPosition);
							}
						}
					}
				}
			}

			//-----------------------item----------------------
			//TODO : 
			//1.ChildrenMoveitem
			//2.DataIndexMovedata
			List<Transform> movedTarget = new List<Transform> ();
			List<int> movedDataIndex = new List<int> ();
			for (int i=0; i<item.Count; i++) {
				movedTarget.Add (mChildren [_sourceIndex + i]);
				movedDataIndex.Add (mChildrenDataIndex [_sourceIndex + i]);
			}
			//3-1ChildrenMoveItem
			for (int i=0; i<movedTarget.Count; i++) {
				mChildren.Remove (movedTarget [i]);
			}
			//3-2ChildrenMoveItem
			for (int i=0; i<movedTarget.Count; i++) {
				mChildren.Insert (_targetIndex + i, movedTarget [i]);
			}
			//4-1.Item
			for (int i=0; i<movedDataIndex.Count; i++) {
				mChildrenDataIndex.Remove (movedDataIndex [i]);
			}
			//4-2.Item
			for (int i=0; i<movedDataIndex.Count; i++) {
				mChildrenDataIndex.Insert (_targetIndex + i, m_dataIndex + i);
			}

			//6. 
			if (m_pItemChangeCallBack != null) {
				for (int i=0; i<item.Count; i++) {
					if (checkDataIndex (m_dataIndex + i)) {
						item [i].gameObject.SetActive (true);
//						ClientLogger.Info (" UpdateItem : onItemChange : " + (_sourceIndex + i) + " " + (_targetIndex + i) + " " + (m_dataIndex + i));
						m_pItemChangeCallBack (item [i], m_dataIndex + i);
					} else {
						item [i].gameObject.SetActive (false);
					}
				}
			}


			if (OpenLog) {
				Debug.Log ("===================> Update Children Start");
				for (int n=0; n<mChildren.Count; n++) {
					Debug.Log ("mChildren : " + n + " " + mChildren [n].name);
				}
				Debug.Log ("===================> Update Children End");
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
			if (_dataCount < 0 || _itemCount < 0 ) { //|| _itemCount > _dataCount
				Debug.Log ("==> InitListView !! !");
				return;
			}
			if (OpenLog) {
				ClientLogger.Info ("==> InitView !! dataCount = " + _dataCount + " _seekIndex = " + _seekIndex + " arrangement = " + arrangement);
			}
			m_dataIndex = -1;
			itemCount = _itemCount;
			dataCount = _dataCount;
			m_MaxCount = dataCount;
			mScroll.SetBounds (bounds);
			if (_seekIndex > 0) {
				Seek (_seekIndex);
			} else {
				Seek (0);
			}
		}

		/// <summary>
		/// Gets the index of the item data.
		/// </summary>
		/// <returns>The item data index.</returns>
		/// <param name="targetIndex">Target index.</param>
		public int GetItemDataIndex (int targetIndex)
		{
			if (targetIndex >= 0 && targetIndex < mChildrenDataIndex.Count) {
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
				for (int i = 0; i < mChildren.Count; i++) {
					if (checkDataIndex (mChildrenDataIndex [i])) {
						m_pItemChangeCallBack (mChildren [i], mChildrenDataIndex [i]);
					}
				}
			}
		}

		#endregion

		#region 

		/// <summary>
		/// Calculates the bounds.
		/// </summary>
		private Bounds CalculateBounds ()
		{
			int vCount = dataCount / maxPerLine + dataCount % maxPerLine;
			int hCount = maxPerLine;
//			Debug.Log (" ==> CalculateBounds : " + dataCount + " " + vCount + " " + hCount);
			int height = vCount * itemHeight;
			int width = hCount * itemWidth;
			//TODO : 
			Vector3 center = CalculateRelativeWidgetBounds (mTrans, mTrans, true) + new Vector3 (0, itemHeight, 0); 
			Bounds r = new Bounds (center + new Vector3 (width / 2, -height / 2, 0), new Vector3 (width, height, 0));
//			Debug.Log (" ==> CalculateBounds : " + center + " " + r);
			return r;
		}

		private static Vector3 CalculateRelativeWidgetBounds (Transform relativeTo, Transform content, bool considerInactive)
		{
			if (content != null) {
				UIWidget[] widgets = content.GetComponentsInChildren<UIWidget> (considerInactive);
				
				if (widgets.Length > 0) {
					Vector3 vMin = new Vector3 (float.MaxValue, float.MaxValue, float.MaxValue);
					Vector3 vMax = new Vector3 (float.MinValue, float.MinValue, float.MinValue);
					
					Matrix4x4 toLocal = relativeTo.worldToLocalMatrix;
					bool isSet = false;
					Vector3 v;
					
					for (int i = 0, imax = widgets.Length; i < imax; ++i) {
						UIWidget w = widgets [i];
						if (!considerInactive && !w.enabled)
							continue;
						
						Vector3[] corners = w.worldCorners;
						
						for (int j = 0; j < 4; ++j) {
							//v = root.InverseTransformPoint(corners[j]);
							v = toLocal.MultiplyPoint3x4 (corners [j]);
							vMax = Vector3.Max (v, vMax);
							vMin = Vector3.Min (v, vMin);
						}
						isSet = true;
					}
					
					if (isSet) {
						return vMin;
					}
				}
			}
			return Vector3.zero;
		}

		/// <summary>
		/// Checks the index of the data.
		/// </summary>
		/// <returns><c>true</c>, if data index was checked, <c>false</c> otherwise.</returns>
		/// <param name="dataIndex">Data index.</param>
		protected bool checkDataIndex (int dataIndex)
		{
			if (dataIndex >= 0 && dataIndex < m_MaxCount) {
				return true;
			} else {
				return false;
			}
		}

        #endregion
	}
}
