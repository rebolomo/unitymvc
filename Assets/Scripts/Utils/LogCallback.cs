//////////////////////////////////////////////////////
////// File Name :        LogCallback.cs
////// Tables :              nothing
////// Autor :               rebolomo
////// Create Date :      2015.8.24
////// Content :           LOG
//////////////////////////////////////////////////////
//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System;
//using System.Text;
//
//namespace XYClient.Utils
//{
//    public class LogCallback : MonoBehaviour
//    {
//
//        string logPath;
//        bool hasLog;
//        string version = "";
//
//        void Start()
//        {
//            logPath = Application.persistentDataPath + "/log.txt";
//            Debug.Log("Log Text:" + logPath + "  Time=" + Time.realtimeSinceStartup);
//            Application.RegisterLogCallback(HandleLogCallback);
//            hasLog = false;
//            TextAsset textVer = Resources.Load("BundleVersion") as TextAsset;
//            if (textVer != null)
//                version = textVer.text;
//        }
//
//        void OnDestroy()
//        {
//            Application.RegisterLogCallback(null);
//        }
//
//        void Update()
//        {
////            if (hasLog)
////            {
////                SendLogToServer();
////                hasLog = false;
////            }
//
//#if UNITY_IOS
//        UploadCrash();
//#endif
//        }
//
//#if UNITY_IOS
//    bool sendReport = false;
//    void UploadCrash()
//    {
//        if(CrashReport.reports.Length > 0 && !sendReport)
//        {
//            Debug.Log("UploadCrash()");
//            
//            sendReport = true;
//            string data = "";
//            for (int i = 0; i < CrashReport.reports.Length; i++) 
//            {
//                var report = CrashReport.reports[i];
//                if(report != null)
//                {
//                    data += "time: " + report.time + "|" + report.text + ";";
//                }
//            }
//            StartCoroutine(UploadCrash_Coroutine(data));
//        }
//    }
//    
//    IEnumerator UploadCrash_Coroutine(string data)
//    {
//        var www = CreateWWW(data, "crash", LogType.Error);
//        yield return www;
//        if(www.error == null)
//        {
//            Debug.Log("upload crash ok.");
//            CrashReport.RemoveAll();
//        }
//        sendReport = false;
//    }
//    
//#endif
//
//        WWW CreateWWW(string log, string stackTrace, LogType type)
//        {
//            string accountId = "";
//            string userId = "";
//            string nickName = "";
//            string deviceUID = "";
//            string message = "";
//            string dateTime = "";
//
////            if (NetWorkHelper.Instance != null)
////            {
////                MobaProtocol.Data.AccountData account = null;
////                MobaProtocol.Data.UserData userData = ModelManager.Instance.Get_userData_X();
////            
////                if (account != null)
////                {
////                    accountId = account.id;
////                    userId = account.UserName;
////                }
////            
////                if (userData != null)
////                {
////                    nickName = userData.NickName;
////                }
////            }
//
//            DateTime dt = DateTime.Now;
//            dateTime = dt.ToString();
//            message = log + "\n" + stackTrace;
//            deviceUID = SystemInfo.deviceUniqueIdentifier;
//
//            //
//            WWWForm form = new WWWForm();
//            form.AddField("UserId", userId);
//            form.AddField("AccountId", accountId);
//            form.AddField("NickName", nickName);
//            form.AddField("DeviceCode", deviceUID);
//            message = ReplaceDanger(message);
//            form.AddField("Message", message);
//            form.AddField("DateTime", dateTime);
//            form.AddField("Versions", version);
//            form.AddField("Device", SystemInfo.deviceModel);
//            WWW www = new WWW("http://" + MasterServer.IP + ":35789/UploadException.ashx", form);
//            return www;
//        }
//
//        private string ReplaceDanger(string str)
//        {
//            str = str.Replace(">", "&gt;");
//            str = str.Replace("<", "&lt;");
//            char ch;
//            //  ch = (char)32;
//            // str = str.Replace(ch.ToString(), "&nbsp;");
//            ch = (char)34;
//            str = str.Replace(ch.ToString(), "&quot;");
//            ch = (char)39;
//            str = str.Replace(ch.ToString(), "&#39;");
//            ch = (char)13;
//            str = str.Replace(ch.ToString(), " ");
//            ch = (char)10;
//            str = str.Replace(ch.ToString(), "br");
//            return str;
//        }
//
//        void WriteLog(string log, string stackTrace)
//        {
//            using (StreamWriter writer = File.AppendText(logPath))
//            {
//                writer.WriteLine(GetCurTime());
//                writer.WriteLine(version);
//                writer.WriteLine(SystemInfo.deviceModel);
//                writer.WriteLine(log + "\n" + stackTrace);
//                writer.WriteLine();
//                hasLog = true;
//            }
//        }
//
//        void HandleLogCallback_Android(string log, string stackTrace, LogType type)
//        {
//            if (type == LogType.Error || type == LogType.Exception)
//            {
////                var www = CreateWWW(log, stackTrace, type);
////                while (!www.isDone)
////                {
////                } //
////                Debug.LogWarning("Get UploadException Result" + www.text);
////                www.Dispose();
////                WriteLog(log, stackTrace);
//            }
//        }
//
//        IEnumerator HandleLogCallback_AndroidWait(string log, string stackTrace, LogType type)
//        {
//            if (type == LogType.Error || type == LogType.Exception)
//            {
////                var www = CreateWWW(log, stackTrace, type);
////                while (!www.isDone)
////                {
////                    yield return new WaitForSeconds(0.1f);
////                } //
////                Debug.LogWarning("Get UploadException Result" + www.text);
////                www.Dispose();
////                WriteLog(log, stackTrace);
//            }
//        }
//
//        void HandleLogCallback_IOS(string log, string stackTrace, LogType type)
//        {
//            if (type == LogType.Error || type == LogType.Exception)
//            {
//                StartCoroutine(IosReportError_Coroutine(log, stackTrace, type));
//            }
//        }
//
//        IEnumerator IosReportError_Coroutine(string log, string stackTrace, LogType type)
//        {
////            var www = CreateWWW(log, stackTrace, type);
////            yield return www;
////            www.Dispose();
////            //      WriteLog();
//            yield break;
//        }
//
//        /// <summary>
//        /// Unity
//        /// </summary>
//        /// <param name="logString">Log string.</param>
//        /// <param name="stackTrace">Stack trace.</param>
//        /// <param name="type">Type.</param>
//        void HandleLogCallback(string log, string stackTrace, LogType type)
//        {
//            //   StartCoroutine(HandleLogCallback_AndroidWait(log, stackTrace, type));
//            // HandleLogCallback_Android(log, stackTrace, type);
//#if UNITY_ANDROID && !UNITY_EDITOR
//        HandleLogCallback_Android(log, stackTrace, type);
//#elif UNITY_IOS && !UNITY_EDITOR 
//        HandleLogCallback_IOS(log, stackTrace, type);
//#endif
//        }
//
//        string GetCurTime()
//        {
//            DateTime time = DateTime.Now;
//            return time.ToString("yyyy-MM-dd HH:mm");
//        }
//
//        /// <summary>
//        /// 
//        /// </summary>
//        public void SendLogToServer()
//        {
//            //string
//            if (!File.Exists(logPath))
//                return;
//
//            FileStream fs = new FileStream(logPath, FileMode.Open);
//            System.IO.BinaryReader br = new BinaryReader(fs);
//            byte[] bt = br.ReadBytes(System.Convert.ToInt32(fs.Length));
//            string base64String = System.Convert.ToBase64String(bt);
//            br.Close();
//            fs.Close();
//
//            string userId = "aa";
////            //UID
////            if (NetWorkHelper.Instance != null)
////            {
////                MobaProtocol.Data.AccountData account = ModelManager.Instance.Get_accountData_X();
////           
////                if (account != null)
////                {
////                    userId = account.id + "(" + account.UserName + ")";
////                } else
////                {
////                    userId = SystemInfo.deviceUniqueIdentifier;
////                }
////            } else
////            {
////                userId = SystemInfo.deviceUniqueIdentifier;
////            }
//
//            //
//            WWWForm form = new WWWForm();
//            form.AddField("UserId", userId);
//            form.AddField("Data", base64String);
//            WWW www = new WWW("http://" + MasterServer.IP + ":1988/UploadException.ashx", form);
//            while (!www.isDone)
//            {
//            } //
//
//            //
//            if (www.text.Equals("Hello WorldOK!"))
//            {
//                Debug.Log("Upload success    Time=" + Time.realtimeSinceStartup);
//                File.Delete(logPath);
//            } else
//            {
//                Debug.Log("Upload failed");
//                Debug.Log(www.text);
//            }
//            www.Dispose();
//        }
//
//        //  void OnGUI(){
//        //      if(GUILayout.Button("crash!!!!!!!!!!!!")){
//        //          throw new IOException("test exception");
//        //      }
//        //      
//        //      if(GUILayout.Button("Exception")){
//        //          Debug.LogException(new IOException("test exception"));
//        //      }
//        //      if(GUILayout.Button("Error")){
//        //          Debug.LogError("test Error");
//        //      }
//        //      GUILayout.Label(logPath);
//        //  }
//    }
//}
