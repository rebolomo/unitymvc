////////////////////////////////////////////////////
//// File Name :        BattleMessageType.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :    	2015.8.24
//// Content :           
////////////////////////////////////////////////////
using System;

namespace UnityMVC.Message
{
	public enum UIMessageType
	{
		UI_Null = ClientMsg.UI_Msg_Begin,

		//	TopView 
		UI_TopView_UpdateTitle, 					// update title in TopView
		UI_TopView_BackButton,					//click back button on TopView
		UI_TopView_Hide,					
		UI_TopView_Show,					

		//	Wait
		UI_WaitingView_WebRequest_Start,
		UI_WaitingView_WebRequest_End,
		UI_WaitingView_WebRequest_TimeOut,
		UI_WaitingView_WebRequest_Error,
		UI_WaitingView_WebRequest_Failed,

		//	Token
		UI_WaitingView_WebRequest_TokenError,

		Reset_Resolution,

		//View message
		UI_OpenView,
		UI_CloseView,
		UI_HideView,

		UI_Max = ClientMsg.UI_Msg_End
	}
}