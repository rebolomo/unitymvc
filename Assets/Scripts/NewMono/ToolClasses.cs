using UnityEngine;
using System.Collections;
using System;
using System.Text;
using UnityMVC.Utils;

public class DirectoryTool {
    /// <summary>
    /// get the file name (with postfix) from path(with slashes and folders), eliminating all folders
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string extractFileFromPath(string path)
    {
        int lastSlashIndex = path.LastIndexOf("/");
        return path.Substring(lastSlashIndex + 1, path.Length - lastSlashIndex - 1);//remove paths;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string extractPath(string path)
    {
        int lastSlashIndex = path.LastIndexOf("/");
        if (lastSlashIndex <= 0) return "";
        return path.Substring(0, lastSlashIndex);//remove paths;
    }

    public static string extractResPathFromAssetPath(string assetPath)
    {
        const string ASSETS_RESOURCES = "Assets/Resources/";
        if(!assetPath.Contains(ASSETS_RESOURCES))
        {
            ClientLogger.Error("asset is not in Resources folder! inappropriate call ");
            return "";
        }
        int start = ASSETS_RESOURCES.Length;
        int end   = 0;
        if (assetPath.Contains("."))
            end = assetPath.LastIndexOf('.');
        else
            end = assetPath.Length - 1;
        return assetPath.Substring(start, end - start);
    }
}

public class ArrayTool
{
    public static bool isNullOrEmpty(object[] arr)
    {
        return arr == null || arr.Length == 0;
    }

    /// <summary>
    /// return the size of arr, return 0 if the arr is null
    /// </summary>
    /// <param name="arr"></param>
    /// <returns></returns>
    public static int getSize(object[] arr)
    {
        return isNullOrEmpty(arr) ? 0 : arr.Length;
    }

    /// <summary>
    /// return the total size of every arrary in arrs
    /// </summary>
    /// <param name="arrs"></param>
    /// <returns></returns>
    public static int getTotalSize(params object[] arrs)
    {
        int count = 0;
        foreach(object[] arr in arrs)
        {
            count += getSize(arr);
        }
        return count;
    }
}
