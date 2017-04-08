
namespace UnityMVC.Protocol
{
    /// <summary>
    /// OnEvent
    /// </summary>
    public enum MobaEventCode
    {
        RequestFriend = 1,
        AddFriend = 2,
        DeleteFriend = 3,
        InviteFriend = 4,
        AcceptFriend = 5,
        BroadcastMessage = 6,
        BroadcastMessageInTable = 7,
        Sit = 8,
        PlayerTurnEnded = 9,
        BetTurnStarted = 10,
        BetTurnEnded = 11,
        GameStarted = 12,
        PlayerHoleCardsChanged = 13,
        GameEnded = 14,
        PlayerWonPot = 15,
        PlayerMoneyChanged = 16,
        TableClosed = 17,
        PlayerTurnBegan = 18,
        PlayerJoined = 19,
        PlayerLeaved = 20,
        SameAccountLogin = 21,
        SendChip = 22,
        ExperienceAdded = 23,
        SendGift = 24,
        Achievement = 25,
        RoomTypeChanged = 26,
        PlayerWonPotImprove = 27,
        PlayersShowCards = 28,
        BroadcastStatusTipsMsg = 29,
        TakenMoneyChanged = 30,
        PlayRankChanegd = 31,
        PropertiesChanged = 32,
        Leave = 33,
        Join = 34,
        LobbyBroadcast,
        RoomBroadcastActorAction,
        RoomBroadcastActorQuit,
        RoomBroadcastActorSpeak,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum MobaEventKey : byte
    {
        //MobaEventKey
        UserId = 1,
        UserData,
        Message,
        Sit,
        Bystander,
        PlayerInfo,
        Amounts,
        TypeRound,
        NoSeatDealer,
        NoSeatSmallBlind,
        NoSeatBigBlind,
        TotalPortAmnt,
        Action,
        AmountPlayed,
        NoSeat,
        MoneyPotId,
        AmountWon,
        MoneySafeAmnt,
        LastNoSeat,
        GameCardIds,
        IsPlaying,
        MoneyBetAmnt,
        TotalRounds,
        StepID,
        NickName,
        Chip,
        MessageContent,
        Level,
        LevelExp,
        SendNoSeat,
        ReceiverNoSeat,
        GiftId,
        PlayingNoSeats,
        AchievementNumber,
        RoomType,
        RoomID,
        GameServerAddress,
        TaxAmnt,

        WinnerSeats,
        AttachedPlayerSeats,
        IsKick,
        HigherBet,
        PlayerLeaveType,
        StatusTipsMsgType,
        StatusTipsMsgParams,
        BigBlind,
        PlayRankList,

        //LiteEventKey
        ActorNr = 254,
        TargetActorNr = 253,
        ActorList = 252,
        Properties = 251,
        ActorProperties = 249,
        GameProperties = 248,
        Data = 245,
        CustomContent = 245
    }

    /// <summary>
    /// 
    /// </summary>
    public enum MobaEventParameter : byte
    {
        UserDatas = 1,
        UserData
    }

  
}
