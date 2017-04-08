using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityMVC.Utils;

public class LSDebug {

    public const bool PRINT_CmTIME   = false;
    private class FuncCall
    {
        public string func;
        public System.DateTime callTiming;
        public FuncCall(string func, System.DateTime callTiming)
        {
            this.func       = func;
            this.callTiming = callTiming;
        }
    }
    private static Stack<FuncCall> funcStacks = new Stack<FuncCall>();
    /// <summary>
    /// use in group with startFunc
    /// </summary>
    /// <param name="keyword"></param>
    public static void startFunc(string keyword)
    {
        funcStacks.Push(new FuncCall(keyword, System.DateTime.Now));
    }

    /// <summary>
    /// use in group with startfunc, print func call time.
    /// </summary>
    public static void finishFunc()
    {
        const string temp = "function call: {0,-50} takes time:{1,-10}";
        FuncCall call = funcStacks.Pop();
        log(string.Format(temp, call.func, (System.DateTime.Now - call.callTiming).TotalSeconds));
    }

    private const string prefix = "ls:-----------------------------------------------------------------> ";
    public static void log(string str)
    {
       ClientLogger.Info(prefix + str);	
    }

    public static void warn(string str)
    {
       ClientLogger.Warn(prefix + str);
    }

    public static void error(string str)
    {
        ClientLogger.Error(prefix + str);
    }

    private static Dictionary<string, System.DateTime> _timers = new Dictionary<string,System.DateTime>();

    public static void resetTimer(string mark)
    {
        if (_timers.ContainsKey(mark))
            _timers[mark] = System.DateTime.Now;
        else
            _timers.Add(mark, System.DateTime.Now);
    }

    public static void resetTimer(int mark)
    {
        resetTimer(mark.ToString());
    }

    public static double getInterval(string marker, bool clearTimer = false)
    {
        if (!_timers.ContainsKey(marker))
        {
            const string TEMP = ":{0}";
            ClientLogger.Error(string.Format(TEMP, marker));
            return 0;
        }
        double o = (System.DateTime.Now - _timers[marker]).TotalSeconds;
        if (clearTimer)
            _timers.Remove(marker);
        return o;
    }

    public static double getInterval(int marker, bool clearTimer = false)
    {
        return getInterval(marker.ToString(), clearTimer);
    }

    public static bool isTimer(int mark)
    {
        return _timers.ContainsKey(mark.ToString());
    }

    public static bool isTimer(string mark)
    {
        return _timers.ContainsKey(mark);
    }

}
