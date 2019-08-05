#define UNITY_PRO_PROFILER
using UnityEngine;
using System.Collections;

public class MobaProfiler
{
    public static void StartProfile(string tag)
    {
        //  REBOL rmv for unity 2019
        //#if UNITY_PRO_PROFILER
        //        Profiler.BeginSample(tag);
        //#else

        //#endif
    }

    public static void EndProfile(string tag="")
    {
        //  REBOL rmv for unity 2019
        //#if UNITY_PRO_PROFILER
        //        Profiler.EndSample();
        //#else

        //#endif
    }
}
