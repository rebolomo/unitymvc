using UnityEngine;
using System.Collections;

/// <summary>
/// Task
/// 1TaskState
/// 2TaskState!!!Task
/// 3MonoU3DAPIMonoBehaviour
/// </summary>
public class TaskManager : MonoBehaviour
{
    #region TaskState
	/// <summary>
	/// Unity
	/// </summary>
	public class TaskState
	{
        #region Delegate
		/// Delegate Declaration
		public delegate void FinishedHandler (bool manual);

		/// 
		public event FinishedHandler Finished;
        #endregion

        #region 
		/// 
		private IEnumerator mEntity;    
		/// 
		private Coroutine mCoroutine;

		/// 
		private bool mRunning;
		/// 
		private bool mPaused;
		/// 
		private bool mManualStopped;
        #endregion

        #region 
		public TaskState (IEnumerator c)
		{
			mEntity = c;

			mRunning = false;
			mPaused = false;
			mManualStopped = false;
		}
        #endregion

        #region 
		/// <summary>
		/// 
		/// </summary>
		public void Start ()
		{
			if (singleton == null)
				return;
			mRunning = true;
			mCoroutine = singleton.StartCoroutine (CallWrapper ());
		}

		/// <summary>
		/// 
		/// </summary>
		public void Pause ()
		{
			mPaused = true;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Unpause ()
		{
			mPaused = false;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Stop ()
		{
			mManualStopped = true;
			mRunning = false;
		}

		/// <summary>
		/// Task
		/// IEnumator
		/// IEnumator
		/// </summary>
		/// <returns></returns>
		private IEnumerator CallWrapper ()
		{
			yield return null;
			IEnumerator e = mEntity;
			while (mRunning) {
				if (mPaused)
					yield return null;
				else {
					if (e != null && e.MoveNext ()) {
						yield return e.Current;
					} else {
						mRunning = false;
					}
				}
			}

			FinishedHandler handler = Finished;
			if (handler != null)
				handler (mManualStopped);
		}
        #endregion

        #region get
		/// <summary>
		/// 
		/// </summary>
		public bool Running {
			get {
				return mRunning;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool Paused {
			get {
				return mPaused;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Coroutine Routine {
			get {
				return mCoroutine;
			}
		}
        #endregion
	}

    #endregion

    #region 
	static TaskManager singleton;
    #endregion

    #region Mono
	void Awake ()
	{
		singleton = this;
		GameObject.DontDestroyOnLoad (gameObject);
	}

	void OnDestroy ()
	{
		singleton = null;
	}
    #endregion

    #region 
	/// <summary>
	/// TaskState
	/// Task
	///       
	///       tggtds
	/// </summary>
	/// <param name="coroutine"></param>
	/// <returns></returns>
	public static TaskState CreateTask (IEnumerator coroutine)
	{
		return new TaskState (coroutine);
	}
    #endregion
}