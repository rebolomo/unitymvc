////////////////////////////////////////////////////
//// File Name :        ResourceManager.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :      2015.8.24
//// Content :           MonoBehaviour
////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityMVC.Utils;

namespace UnityMVC.Resource
{
/// <summary>
///  cached resource interface (from asset bundle)
/// </summary>
    public class CachedRes
    {
        private static Dictionary<string, GameObject> _UnitStop = new Dictionary<string, GameObject>();
        /// <summary>
        /// get unit's prefab,if you want to get the instantiated unit, use getInstantiatedUnit(name)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject getUnitPrefab(string name)
        {
            if (_UnitStop.ContainsKey(name))
                return _UnitStop [name];

            GameObject prefab = Resources.Load(MobaPaths.HEROES_RES_PATH + name) as GameObject;
            if (prefab == null)
                throw new NotFoundException(name);
            _UnitStop.Add(name, prefab);
            return prefab;
        }

        private static Dictionary<string, GameObject> _resUnitStop = new Dictionary<string, GameObject>();

        public static GameObject getUnitAtResPath(string resPath)
        {
            if (_resUnitStop.ContainsKey(resPath))
                return _resUnitStop [resPath];
            GameObject prefab = Resources.Load(resPath) as GameObject;
            if (prefab == null)
                throw new NotFoundException(resPath);
            _resUnitStop.Add(resPath, prefab);
            return prefab;
        }

        private static Dictionary<string, Texture[]> _fullImgs = new Dictionary<string, Texture[]>();

        public static Texture[] getFullImgs(string heroName)
        {
            if (_fullImgs.ContainsKey(heroName))
                return _fullImgs [heroName];
            const string PATH = "Skins/FullImgs/";
            const int MAX_COUNT = 10;//maybe even more
            List<Texture> all = new List<Texture>();
            for (int i = 0; i < MAX_COUNT; ++i)
            {
                string p = PATH + heroName + i;
                Texture t = Resources.Load(p) as Texture;
                if (t != null)
                    all.Add(t);
                else
                    break;
            }
            _fullImgs.Add(heroName, all.ToArray());
            return _fullImgs [heroName];
        }

        private static Dictionary<string, GameObject> _uiTemps = new Dictionary<string, GameObject>();

        public static GameObject getUITemplate(string uiName)
        {
            if (_uiTemps.ContainsKey(uiName))
                return _uiTemps [uiName];
            const string PATH = "UITemplate/";
            string path = PATH + uiName;
            GameObject go = Resources.Load(path) as GameObject;
            if (go != null)
                _uiTemps.Add(uiName, go);
            else
                throw new NotFoundException(uiName);
            return _uiTemps [uiName];
        }




        /// <summary>
        /// get instantiated unit, if you want to get the unit's prefab, use getUnit(name)
        /// </summary>
        /// <param name="resPath"></param>
        /// <returns></returns>
        public static GameObject getClonedUnit(string resPath)
        {
            GameObject clone = MonoBehaviour.Instantiate(getUnitAtResPath(resPath)) as GameObject;
            return clone;
        }

        const string TEMP_SPELL_NOT_FOUND = "spellprefab:{0,-15} is not found!";
        private static Dictionary<string, GameObject> _spells = new Dictionary<string, GameObject>();
        /// <summary>
        /// There are 2 categories of spells, those under resources folder and the other not, pass the full assetpath of spell into this function, in 
        /// which the category will be checked.
        /// </summary>
        /// <param name="spellPath"></param>
        /// <returns></returns>
        public static GameObject getSpellPrefab(string spellPath)
        {
            if (_spells.ContainsKey(spellPath))
                return _spells [spellPath];

            if (spellPath.StartsWith("Assets/Resources/"))
            {
                string resPath = DirectoryTool.extractResPathFromAssetPath(spellPath);
                GameObject prefab = Resources.Load(resPath, typeof(GameObject)) as GameObject;
                if (prefab)
                {
                    _spells.Add(spellPath, prefab);
                    return prefab;
                }
            } else//bundle PATH
            {
                ClientLogger.Error("incomplete!");
                return null;
            }
            throw new NotFoundException(string.Format(TEMP_SPELL_NOT_FOUND, spellPath));
        }

        public static GameObject getClonedSpell(string spellPath)
        {
            GameObject prefab = getSpellPrefab(spellPath);
            if (!prefab)
                throw new NotFoundException(string.Format(TEMP_SPELL_NOT_FOUND, spellPath));

            GameObject clone = MonoBehaviour.Instantiate(prefab) as GameObject;
            if (clone.GetComponent<DestroySpellTimed>() == null)
                clone.AddComponent<DestroySpellTimed>();
            return clone;
        }

        private static Dictionary<string, Object> _res = null;

        public static Object loadRes(string path)
        {
            return getFrom(path, ref _res);
        }

        private static Object getFrom(string path, ref Dictionary<string, Object> cache)
        {
            if (_res == null)
                _res = new Dictionary<string, Object>();
            if (cache.ContainsKey(path))
                return cache [path];
            const string RES_NOT_FOUND = "resource:{0} is not found!";
            Object o = Resources.Load(path) as Object;
            if (o == null)
            {
#if UNITY_EDITOR
            ClientLogger.Error(string.Format(RES_NOT_FOUND, path));
#endif
                throw new NotFoundException(string.Format(RES_NOT_FOUND, path));
            }
            cache.Add(path, o);
            return o;
        }

    }
}
