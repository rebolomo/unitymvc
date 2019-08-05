////////////////////////////////////////////////////
//// File Name :        ResourceManager.cs
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
    /// <summary>
    /// 
    /// </summary>
    public static class Constants
    {
        public const bool DEBUG = false;
        public const bool DEBUG_CONTROL = false;
        public const bool DEBUG_SKILL = false;
        public const bool DEBUG_MAP = false;
        public const bool DEBUG_BUILDING = false;
        public const bool DEBUG_OPEN = true;
        public const bool DEBUG_AI = true;

        //
        public const string TAG_MONSTER = "Monster";//
        public const string TAG_PLAYER = "Player";//
        public const string TAG_HERO = "Hero";//
		//	REBOL add
		public const string TAG_BOSS = "Boss";//boss
        public const string TAG_TOWER = "Building"; //
        public const string TAG_HOME = "Home"; //
        public const string TAG_MAP = "Map"; //
        public const string TAG_ITEM = "Item"; //
        public const string TAG_BUFF = "BuffItem";//
        public const string TAG_DangBan = "DangBan"; //
		//	REBOL note, TAG_SPAWNPOINT
        public const string TAG_SPAWNPOINT = "SpawnPoint"; //
        //
        public const string OBJ_SCREENPOINT = "Target";

        //
        public const int DEFAULT_TARGET_FPS = 30;//
        public const int BATTLE_TARGET_FPS = 30; //60CPU

        //
        public static int LAYER_HM = LayerMask.GetMask("Monster", "Unit");
        public static int LAYER_EntityelectObj = LayerMask.GetMask("EntityelectObj");

		public static string xmlBinDataPath = "Data/BinData_Client";
		public static string xmlFolder = Application.dataPath + "/Resources/Data/Xml/"; //xml;
		public static string jsonBinDataFolder = "Data2/BinData/";
		public static string jsonFolder = Application.dataPath + "/Resources/Data2/Json/"; //xml
    }
}
