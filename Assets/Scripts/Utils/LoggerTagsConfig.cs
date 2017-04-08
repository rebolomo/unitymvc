////////////////////////////////////////////////////
//// File Name :        LoggerTagsConfig.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :      2015.8.24
//// Content :           MonoBehaviour
////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityMVC.Utils
{
    [ExecuteInEditMode]
    public class LoggerTagsConfig : MonoBehaviour
    {
        void Start()
        {
            Config();
        }

        /// <summary>
        /// Debugrefresh
        /// </summary>
        public void Config()
        {
            ClientLogger.Debug(LoggerTags.SKILL_LOGS, "--- config log : " + LoggerTags.SKILL_LOGS + " -------");
            ClientLogger.Debug(LoggerTags.BUFF_LOGS, "--- config log : " + LoggerTags.BUFF_LOGS + " -------");
            ClientLogger.Debug(LoggerTags.HIGHEFF_LOGS, "--- config log : " + LoggerTags.HIGHEFF_LOGS + " -------");
            ClientLogger.Debug(LoggerTags.PVPEVENT_LOGS, "--- config log : " + LoggerTags.PVPEVENT_LOGS + " -------");
            ClientLogger.Debug(LoggerTags.DATA_LOGS, "--- config log : " + LoggerTags.DATA_LOGS + " -------");
        }
    }
}
