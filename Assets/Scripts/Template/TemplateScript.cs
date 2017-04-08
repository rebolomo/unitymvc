////////////////////////////////////////////////////
//// File Name :        TemplateScript.cs
//// Tables :           	nothing
//// Autor :            	rebolomo
//// Create Date :    	2015.8.24
//// Content :          MonoBehaviour
////////////////////////////////////////////////////
using UnityMVC.Utils;


namespace UnityMVC.Template
{
	using UnityEngine;
	using System.Collections;

	/// <summary>
	/// 
	/// </summary>
	public class TemplateScript : MonoBehaviour
	{
        #region Members

        #region Protected & Internal Members
		/// <summary>
		/// 
		/// </summary>
		enum EnumMode
		{
			Append1,
			Append2
		}

		/// <summary>
		/// public
		/// </summary>
		private string fieldName1;
		/// <summary>
		/// 
		/// </summary>
		[SerializeField]
		private string
			fieldName2;

        #endregion

        #region Public Members
		/// <summary>
		/// 
		/// </summary>
		public string FieldName1;
		/// <summary>
		/// 
		/// </summary>
		public string FieldName2;
        
        #endregion

        #region Event Members
		/// <summary>
		/// 
		/// </summary>
		public delegate void EventHandler ();
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler ChangedEvent;

        #endregion

        #endregion

        #region Unity Methods

		/// <summary>
		/// Use this for initialization
		/// </summary>
		void Start ()
		{
		}

		/// <summary>
		/// Update is called once per frame
		/// </summary>
		void Update ()
		{
		}

        #endregion

        #region Custom Methods

        #region Protected & Internal Methods

		/// <summary>
		/// 
		/// </summary>
		void UpdateData ()
		{
#if UNITY_EDITOR
           ClientLogger.Info("XXXXXXXXX");
#endif
		}
        #endregion

        #region Public Methods

		/// <summary>
		/// 
		/// </summary>
		public void SetData ()
		{
#if UNITY_EDITOR
           ClientLogger.Info("XXXXXXXXX");
#endif
		}
        #endregion

        #endregion
	}
}
