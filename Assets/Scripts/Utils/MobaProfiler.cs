#define UNITY_PRO_PROFILER
using UnityEngine;
using System.Collections;

public class MobaProfiler
{
    public static void StartProfile(string tag)
    {
#if UNITY_PRO_PROFILER
        Profiler.BeginSample(tag);
#else

#endif
    }

    public static void EndProfile(string tag="")
    {
#if UNITY_PRO_PROFILER
        Profiler.EndSample();
#else

#endif
    }
}
