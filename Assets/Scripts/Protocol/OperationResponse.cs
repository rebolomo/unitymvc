////////////////////////////////////////////////////
//// File Name :        OperationResponse.cs
//// Tables :           nothing
//// Autor :            rebolomo
//// Create Date :      2015.8.24
//// Content :          MonoBehaviour
////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityMVC.Protocol
{
    public class OperationResponse
    {
        public string DebugMessage;
        public short OperationCode;
        public object Parameters;
        public short ReturnCode;

        public override string ToString()
        {
            return string.Format("OperationResponse {0}: ReturnCode: {1}.", this.OperationCode, this.ReturnCode);
        }

        public string ToStringFull()
        {
            //return string.Format("OperationResponse {0}: ReturnCode: {1} ({3}). Parameters: {2}", new object[] { this.OperationCode, this.ReturnCode, SupportClass.DictionaryToString(this.Parameters), this.DebugMessage });
            return "";
        }
    }

}