using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// [][][][][]
/// 
/// 1ManagerCoroutineManager
/// 2CoroutineManagerTaskTaskManager
/// </summary>
public class CoroutineManager
{
    #region 
    private readonly List<Task> taskList;
    #endregion

    #region 
    public CoroutineManager()
    {
        taskList = new List<Task>();
        taskList.Clear();
    }
    #endregion

    #region 
    /// <summary>
    /// 
    /// </summary>
    /// <param name="routine"></param>
    /// <param name="autoStart"></param>
    /// <returns></returns>
    public Task StartCoroutine(IEnumerator routine, bool autoStart = true)
    {
        var t = new Task(routine, autoStart);
        taskList.Add(t);
        return t;
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    public void StopCoroutine(Task t)
    {
        if (taskList.Contains(t))
        {
            t.Stop();
            taskList.Remove(t);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    public void PauseCoroutine(Task t)
    {
        if (taskList.Contains(t))
        {
            t.Pause();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    public void ResumeCoroutine(Task t)
    {
        if (taskList.Contains(t))
        {
            t.Unpause();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void PauseAllCoroutine()
    {
        for (int i = 0; i < taskList.Count; ++i)
            taskList[i].Pause();
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResumeAllCoroutine()
    {
        for (int i = 0; i < taskList.Count; ++i)
            taskList[i].Unpause();
    }

    /// <summary>
    /// 
    /// </summary>
    public void StopAllCoroutine()
    {
        for (int i = 0; i < taskList.Count; ++i)
            taskList[i].Stop();
            taskList.Clear();
    }
}
