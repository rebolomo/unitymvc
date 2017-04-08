using UnityEngine;
using System.Collections;

public class ExitBattleMsg : GameMessage {
    public ExitBattleMsg()
    {
        MessageManager.dispatch(this);
    }
}
