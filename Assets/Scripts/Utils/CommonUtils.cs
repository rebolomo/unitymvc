////////////////////////////////////////////////////////////////////////////////////////////////////////
//// File Name :        LevelManager.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :      2015.8.24
//// Content :           
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;


using UnityEngine;

namespace UnityMVC.Utils
{
    public static class CommonExtension
    {
        public static TDefault GetDictValue<TKey, TValue, TDefault>(this Dictionary<TKey, TValue> dict, TKey key, TDefault def) where TDefault : TValue
        {
            System.Diagnostics.Debug.Assert(dict != null);
            if (dict.ContainsKey(key))
            {
                return (TDefault)dict[key];
            }
            return def;
        }
    }

    public static class CommonUtils
    {
//        public static bool IsGold(this RewardData model)
//        {
//            return model.Type == 1 && model.Id == "1";
//        }
//
//        public static bool IsDiamond(this RewardData model)
//        {
//            return model.Type == 1 && model.Id == "2";
//        }
//
//        public static void MergeRewards(List<RewardData> rewards)
//        {
//            int golds = 0;
//            int gems = 0;
//            for (int i = 0; i < rewards.Count; i++)
//            {
//                if (rewards[i].IsGold())
//                {
//                    golds += rewards[i].Count;
//                }
//                if (rewards[i].IsDiamond())
//                {
//                    gems += rewards[i].Count;
//                }
//            }
//            rewards.RemoveAll((x) => x.IsGold() || x.IsDiamond());
//            rewards.Add(new RewardData { Count = golds, Id = "1", Type = 1 });
//            rewards.Add(new RewardData { Count = gems, Id = "2", Type = 1 });
//        }
    }

    public class GridHelper
    {
        public delegate void GridBinder<T>(int idx, T comp) where T : Component;

        public static void FillGrid<T>(UIGrid grid, T sample, int count, GridBinder<T> binder) where T : Component
        {
            if (!grid)
            {
                throw new System.ArgumentNullException("UIGrid is null");
            }

            if (!sample)
            {
                throw new System.ArgumentNullException("Item is null");
            }

            int allocCount = count - grid.transform.childCount;
            for (int i = 0; i < allocCount; i++)
            {
                NGUITools.AddChild(grid.gameObject, sample.gameObject);
            }

            if (binder != null)
            {
                for (int i = 0; i < count; i++)
                {
                    var child = grid.transform.GetChild(i);
                    binder(i, child.GetComponent<T>());
                }

                //
                for (int i = count; i < grid.transform.childCount; i++)
                {
                    var child = grid.transform.GetChild(i);
                    NGUITools.SetActive(child.gameObject, false);
                }
            }
        }
    }
}
