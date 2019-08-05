////////////////////////////////////////////////////
//// File Name :          Singleton.cs
//// Tables :                nothing
//// Autor :                rebolomo
//// Create Date :      2015.8.24
//// Content :            
////////////////////////////////////////////////////
using System;

namespace UnityMVC.Core
{
	public abstract class Singleton<T> where T : new()
	{
		private static T instance = (default(T) == null) ? Activator.CreateInstance<T> () : default(T);

		public static T Instance {
			get {
				return Singleton<T>.instance;
			}
		}

		protected Singleton ()
		{
		}

		public abstract void SingletonCreate (); //
		public abstract void SingletonDestroy (); //
	}
}

