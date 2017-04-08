//
//namespace XYClient.Protocol
//{
//	/// <summary>
//	/// 
//	/// </summary>
//	public enum WebMessageErrorCode
//	{
//		// generic
//		Ok = 0, //
//		MoneyShortage = 10,//
//		BattleUnlockError = 11,//
//		ReqMemberExisting = 12,//
//		ShopTimeout = 13,//
//		EnergyShortage = 14, // 
//	
//		InvalidOperation = 198,
//		InvalidParameter = 199,
//		CustomError = 200,
//		Sucess = 201,
//		SystemError = 500,
//		NoResult = 505,
//		DataError = 506,
//		// user
//		AuthenticationFail = 10101,
//		UserExist = 10102,
//		UserNotExist = 10103,
//		NoAwards = 10104,
//		MailExist = 10105,
//		NickNameExist = 10106,
//		MailIsEmpty = 10107,
//		PassWordIsEmpty = 10108,
//		The3rdUserNotRegistered = 10109,
//		NickNameIsEmpty = 10110,
//	
//		//friend
//		FriendExist = 20101,
//		FriendNotExist = 20102,
//		FriendOffline = 20103,
//		FriendInBlacklist = 20104,
//	
//		//Room
//		RoomFull = 30101,
//		TableFull = 30102,
//		TableNotExist = 30103,
//		OnlyFriendsCanJoin = 30104,
//		GameEnded = 30105,
//		ChipsNotEnough = 30106,
//	
//		//Game
//		KickErrorOwner = 30201,
//		KickErrorLevel = 30202,
//		KickErrorLimit = 30203,
//		KickErrorPlaying = 30204,
//	
//		//Item
//		ItemExist = 40101,
//	
//		//payment
//		VerifyFail = 50101,
//	
//		//arena
//		CDTimeError = 60101,//CD
//		DayCountError = 60102,//
//		//union
//		UnionFull = 60201,//
//		UnionReqFull = 60202,//
//		//BuyItem(chips)
//		BuyItemNotFound = 50201,
//		BuyItemChipNotEnough = 50202,
//		BuyItemUserNotExist = 50203,
//		OperationDenied = -3,
//		OperationInvalid = -2,
//		InternalServerError = -1,
//		InvalidAuthentication = 0x7FFF, // codes start at short.MaxValue 
//		GameIdAlreadyExists = 0x7FFF - 1,
//		GameFull = 0x7FFF - 2,
//		GameClosed = 0x7FFF - 3,
//		AlreadyMatched = 0x7FFF - 4,
//		ServerFull = 0x7FFF - 5,
//		UserBlocked = 0x7FFF - 6,
//		NoMatchFound = 0x7FFF - 7,
//		RedirectRepeat = 0x7FFF - 8,
//		GameIdNotExists = 0x7FFF - 9,
//
//	}
//}