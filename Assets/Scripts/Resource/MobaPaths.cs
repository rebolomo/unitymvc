////////////////////////////////////////////////////
//// File Name :        MobaPaths.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :      2015.8.24
//// Content :           MonoBehaviour
////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityMVC.Resource
{
/// <summary>
/// paths to all the resources
/// </summary>
    public class MobaPaths
    {
        public const string HEROES_RES_PATH = "Prefab/Actor/Hero/";
        public const string HEROES_PATH_NOASSET = "Resources/" + HEROES_RES_PATH;
        public const string HEROES_ASSET_PATH = "Assets/" + HEROES_PATH_NOASSET;
    }
}