using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityMVC.Protocol
{
    public enum UserType:byte
    {
        Normal = 0,
        GameCenter = 1,
        Facebook = 2,
        QQ = 3,
        SinaWeibo = 4,
        RenRen = 5,
        Guest = 6,
        NinetyOne=7
    }
	
	public enum RoomType
	{
		Common=1,
		Egypt=2,
		Hawaii=4,
		Japan=8,
		West=16,
		China=32
	}

    public enum ChipType
    {
        Chip1=1,
        Chip2,
        Chip3,
        Chip4,
        Chip5
    }

    public enum UserStatus:byte
    {
        Offline = 1,
        Playing = 2,
        Idle = 3,
        Suspend=4
    }
	
	public enum PetType:byte
	{
		FatCat,
		SharPei,
		Lizard,
		Parrot
	}

    public enum ItemType : byte
    {
        Room=1,
        Chip,
        Avator,
        Jade,
        Lineage
    }

    public enum VIPItem
    {
        Days30 = 1,
        Days90,
        Days180,
        Days365,
        Forever
    }

    public enum MessageType:byte
    {
        /// <summary>
        /// 
        /// </summary>
        RequestFriend = 1,
        /// <summary>
        /// 
        /// </summary>
        SendChip,
        /// <summary>
        /// 
        /// </summary>
        BuyChips = 4,
        /// <summary>
        /// 
        /// </summary>
        AppScore=5
    }

    public enum ExpType { 
        Fold=1,
        Win=4,
        Normal=2
    }
	
	public enum GiftType
	{
        None=0,
		Gift1,
		Gift2,
		Gift3,
		Gift4,
		Gift5
	}

    public enum DeviceType { 
        UNKNOW=0,
        IOS,
        ANDROID
    }

    public enum NotificationType { 
        System,
        ActionNeeded,
        AddFriend,
        SendChips,
        InviteFriendGame,
        InviteFriendRoom
    }

    public enum BankActionType
    {
        RobotTaken = 1,
        Register,
        BuyChips,
        LevelUp,
        Achievement,
        Award,
        GuestUpgrade,

        BuyGift=101,
        CreatGame,
        GameTax,
        Broadcast,
        CreatGameSystem,
        GameTaxSystem

    }

    public enum GameType { 
        User,
        System,
        Training,
        Career
    }


    public enum CacheKeys {
        SystemNotice=0,
        BroadcastMessage,
        RegisterAwards,
        UpgradeAwards,
        CreateGame,
        GameTax,
        GameWaitingTime,
        SendChipsFee,
        RegisterAwardsGuest,
        ClientVersion,
        UpgradeUrl
    }

    // Global States of the Game
    public enum TypeState
    {
        Init,
        WaitForPlayers,
        WaitForBlinds,
        Playing,
        Showdown,
        DecideWinners,
        DistributeMoney,
        End
    }

    public enum PayWay
    { 
        UnKown = 0,
        Alipay,
        IAP,
        NineOnePay,
        Yeepay
    }

    public enum AwardType:byte
    {
        Guest=1,
        Normal,
        Pay,
        GuestNone = 10,
        NormalNone=20,
        PayNone=30
    }

    public enum PlayerLeaveType : byte { 
        Leave,
        Stand,
        Kick,
        KickByPlayer,
        KickFoldTwice,
    }
    public enum UnionJobType
    {
        Member = 0,
        Master = 1,
        Elder = 2,
    }
}
