using UnityEngine;
using System.Collections;

public enum ClientMsg
{
	#region 

    PeerConnectTimeOut,

    MasterPeerStartConnect,
    MasterPeerEndConnect,

	#endregion

	#region Model/View

    NotifyModel_master_begin = 5001,
    NotifyModel_master_end = 5500,

    NotifyModel_game_begin = 6301,
    //NotifyModel_game_end = 6600,

    NotifyView_master_begin = 10000,
    //NotifyView_master_end = 10500,

    NotifyView_game_begin = 10501,
    //NotifyView_game_end = 10600,

	#endregion

	#region 
	
	ModelChanged_Begin = 1000,
	ModelChanged_End = 5000,
	
	Battle_Msg_Begin = 10601,
	Battle_Msg_End = 10700,
	
	Entity_Msg_Begin = 10701,
	Entity_Msg_End = 10900,

	UI_Msg_Begin = 11001,
	UI_Msg_End = 11200,

	Action_Msg_Begin = 11201,
	Action_Msg_End = 11500,

	#endregion
}