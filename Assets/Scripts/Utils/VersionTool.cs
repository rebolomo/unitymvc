using UnityEngine;
using System.Collections;
using System.IO;
using System;

/// <summary>
/// Version tool.
/// </summary>
public class VersionTool : MonoBehaviour
{
	/// <summary>
	/// The local version.
	/// </summary>
	[SerializeField]
	private string
		localVersion = "0.0.1";
	public static VersionTool Instance;

	/// <summary>
	/// Gets the version.
	/// </summary>
	/// <returns>The version.</returns>
	public static string GetVersion ()
	{
		return Instance.localVersion;
	}

	void Start ()
	{
		Instance = this;
		LoadVersion ();
	}

	/// <summary>
	/// Gets the version.
	/// </summary>
	/// <returns>The version.</returns>
	public string LoadVersion ()
	{
		try {
#if UNITY_ANDROID && !UNITY_EDITOR
			StartCoroutine(LoadWWW());
#elif UNITY_IPHONE && !UNITY_EDITOR
			string filepath = Application.dataPath + "/Raw" + "/BundleVersion.txt";
			StreamReader reader = File.OpenText(filepath);
			if (reader != null) {
					localVersion = reader.ReadLine ();
			} 
			ClientLogger.Info ("GetVersion : " + filepath);
			ClientLogger.Info ("GetVersion : " + localVersion);
#else
			string filepath = Application.streamingAssetsPath + "/BundleVersion.txt";
			StreamReader reader = File.OpenText (filepath);
			if (reader != null) {
				localVersion = reader.ReadLine ();
			} 
			ClientLogger.Info ("GetVersion : " + filepath);
			ClientLogger.Info ("GetVersion : " + localVersion);
#endif
		} catch (Exception e) {
//			ClientLogger.Error ("GetVersion Error : " + e.Message);
			return "0.0.1";
		}
		return localVersion;
	}

	IEnumerator LoadWWW ()
	{  
		string filepath = "jar:file://" + Application.dataPath + "!/assets" + "/BundleVersion.txt";
		WWW www = new WWW (filepath);  
		yield return www;  
		localVersion = www.text;
		ClientLogger.Info ("GetVersion : " + filepath);
		ClientLogger.Info ("GetVersion : " + localVersion);
	}  

}
