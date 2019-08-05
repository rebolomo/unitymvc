////////////////////////////////////////////////////
//// File Name :        TimeoutController.cs
//// Tables :           nothing
//// Autor :            rebolomo
//// Create Date :    	2015.8.24
//// Content :          MonoBehaviour
////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using UnityMVC.Message;

namespace UnityMVC.Core.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class TimeoutController : MonoBehaviour
    {
        public Callback OnTimeoutCallback;
        float timeout;

        void OnDestroy()
        {
            CancelInvoke();
            OnTimeoutCallback = null;
        }

        public void StartTimeOut(float timeout, Callback timeout_callback)
        {
            if (timeout > 0)
            {
                this.OnTimeoutCallback = timeout_callback;
                Invoke("Timeout", timeout);
            }
        }
        public void StopTimeOut()
        {
            CancelInvoke();
            OnTimeoutCallback = null;
        }

        void Timeout()
        {
            CancelInvoke();
            if (OnTimeoutCallback != null)
            {
                OnTimeoutCallback();
                OnTimeoutCallback = null;
            }
        }
    }
}
