////////////////////////////////////////////////////
//// File Name :        CallBack.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :    	2015.8.24
//// Content :           
////////////////////////////////////////////////////
namespace UnityMVC.Message
{
    using UnityEngine;

    // TODO : 
    //
    public delegate void Callback();
    public delegate void Callback<T>(T arg1);
    public delegate void Callback<T, U>(T arg1, U arg2);
    public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);
    //
    public delegate bool TriggerCondition(); //Trigger
    public delegate bool TriggerCondition<T>(T arg1);
    public delegate void TriggerAction(); //Trigger
    //
    public delegate void CallbackDelegateV3(Vector3 v3);
    public delegate void CallbackDelegateNull();
    public delegate void CallbackDelegateInt(int nIndex);
    public delegate void CallbackDelegateBool(bool bIsTrigger);
    //UI
    public delegate void OpenViewGuideDelegate();  //no params 
}