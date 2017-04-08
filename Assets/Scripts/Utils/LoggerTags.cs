////////////////////////////////////////////////////
//// File Name :        GlobalObject.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :    	2015.8.24
//// Content :           MonoBehaviour
////////////////////////////////////////////////////
namespace UnityMVC.Utils
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class LoggerTags : MonoBehaviour
    {
        public const string BUFF_LOGS = "Buff";
        public const string HIGHEFF_LOGS = "HighEffect";
        public const string SKILL_LOGS = "Skill";
        public const string PVPEVENT_LOGS = "PvpEvent";
        public const string DATA_LOGS = "DataUpdate";
        public const string DEATH_LOGS = "Death";
        public const string CREATE_LOGS = "Create";

        [System.Serializable]
        public class LoggerEntry
        {
            public string Tag;
            public bool Enable;
            public string Color;
        }

        public List<LoggerEntry> Tags;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Apply();
        }

        public void Refresh()
        {
            foreach (string tag in ClientLogger.Tags)
            {
                var info = ClientLogger.GetTagInfo(tag);
                if (info != null)
                {
                    var found = Tags.Find(x => x.Tag == tag);
                    if (found == null)
                    {
                        Tags.Add(new LoggerEntry
                        {
                            Color = info.Color,
                            Enable = info.Enable,
                            Tag = tag
                        });
                    }
                    else
                    {
                        found.Color = info.Color;
                        found.Enable = info.Enable;
                    }
                }
            }
        }

        public void Apply()
        {
            foreach (var tag in Tags)
            {
#if UNITY_EDITOR
                //Debug.Log(" =====> Apply : " + tag.Tag + " " + tag.Enable);
#endif
                ClientLogger.SetTagInfo(tag.Tag, tag.Color, tag.Enable);
            }
        }
    }
}
