
/// TaskManager.cs
/// Copyright (c) 2011, Ken Rockot  <k-e-n-@-REMOVE-CAPS-AND-HYPHENS-oz.gs>.  All rights reserved.
/// Everyone is granted non-exclusive license to do anything at all with this code.
///
/// This is a new coroutine interface for Unity.
///
/// The motivation for this is twofold:
///
/// 1. The existing coroutine API provides no means of stopping specific
///    coroutines; StopCoroutine only takes a string argument, and it stops
///    all coroutines started with that same string; there is no way to stop
///    coroutines which were started directly from an enumerator.  This is
///    not robust enough and is also probably pretty inefficient.
///
/// 2. StartCoroutine and friends are MonoBehaviour methods.  This means
///    that in order to start a coroutine, a user typically must have some
///    component reference handy.  There are legitimate cases where such a
///    constraint is inconvenient.  This implementation hides that
///    constraint from the user.
///
/// Example usage:
///
/// ----------------------------------------------------------------------------
/// IEnumerator MyAwesomeTask()
/// {
///     while(true) {
///        ClientLogger.Info("Logcat iz in ur consolez, spammin u wif messagez.");
///         yield return null;
////    }
/// }
///
/// IEnumerator TaskKiller(float delay, Task t)
/// {
///     yield return new WaitForSeconds(delay);
///     t.Stop();
/// }
///
/// void SomeCodeThatCouldBeAnywhereInTheUniverse()
/// {
///     Task spam = new Task(MyAwesomeTask());
///     new Task(TaskKiller(5, spam));
/// }
/// ----------------------------------------------------------------------------
///
/// When SomeCodeThatCouldBeAnywhereInTheUniverse is called, the debug console
/// will be spammed with annoying messages for 5 seconds.
///
/// Simple, really.  There is no need to initialize or even refer to TaskManager.
/// When the first Task is created in an application, a "TaskManager" GameObject
/// will automatically be added to the scene root with the TaskManager component
/// attached.  This component will be responsible for dispatching all coroutines
/// behind the scenes.
///
/// Task also provides an event that is triggered when the coroutine exits.


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// A Task object represents a coroutine.  Tasks can be started, paused, and stopped.
/// It is an error to attempt to start a task that has been stopped or which has
/// naturally terminated.
/// -----------------------------------------------------------------------------------
/// By lili :
/// TaskTaskManager.TaskStateTaskState[][][]
/// TaskManagerTaskManager
/// Task
public class Task
{
    #region Delegate
    /// Delegate for termination subscribers.  manual is true if and only if
    /// the coroutine was stopped with an explicit call to Stop().
    /// ----------------------------------------------------------------------
    /// By lili:
    /// Task
    public delegate void FinishedHandler(bool manual);

    /// Termination event.  Triggered when the coroutine completes execution.
    /// ----------------------------------------------------------------------
    /// By lili:
    /// Task
    public event FinishedHandler Finished;
    #endregion

    #region 
    /// <summary>
    /// TaskManager
    /// Taskþ
    /// </summary>
    private TaskManager.TaskState mTaskState;
    #endregion

    #region 
    /// Creates a new Task object for the given coroutine.
    ///
    /// If autoStart is true (default) the task is automatically started
    /// upon construction.
    /// -----------------------------------------------------------------
    /// by lili:
    /// IEnumeratorTaskManagerTaskState
    /// TaskStateTaskFinish
    public Task(IEnumerator c, bool autoStart = true)
    {
        mTaskState = TaskManager.CreateTask(c);
        if (mTaskState != null)
        {
            mTaskState.Finished += TaskFinished;
            if (autoStart)
                Start();
        }
    }

    #endregion

    #region 
    /// Begins execution of the coroutine
    /// ------------------------------------
    /// By lili:
    /// 
    public void Start()
    {
        if (mTaskState != null)
        {
            mTaskState.Start();
        }
    }

    /// -------------------------------------
    /// By lili:
    /// 
    public void Pause()
    {
        if (mTaskState != null)
        {
            mTaskState.Pause();
        }
    }

    /// --------------------------------------
    /// By lili:
    /// 
    public void Unpause()
    {
        if (mTaskState != null)
        {
            mTaskState.Unpause();
        }
    }

    /// Discontinues execution of the coroutine at its next yield.
    /// ----------------------------------------------------------
    /// By lili:
    /// 
    public void Stop()
    {
        if (mTaskState != null)
        {
            mTaskState.Stop();
        }
    }
    #endregion

    #region get&set
    /// Returns true if and only if the coroutine is running.  Paused tasks
    /// are considered to be running.
    /// --------------------------------------------------------------------
    /// By lili:
    /// Running
    public bool Running
    {
        get
        {
            return mTaskState.Running;
        }
    }

    /// Returns true if and only if the coroutine is currently paused.
    /// --------------------------------------------------------------
    /// By lili
    /// 
    public bool Paused
    {
        get
        {
            return mTaskState.Paused;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Coroutine Routine
    {
        get
        {
            return mTaskState.Routine;
        }
    }
    #endregion

    #region Private
    /// -------------------------------------------------------
    /// By lili:
    /// TaskStateStop
    /// <param name="manual">/</param>
    void TaskFinished(bool manual)
    {
        FinishedHandler handler = Finished;
        if (handler != null)
        {
            handler(manual);
        }
    }
    #endregion

}