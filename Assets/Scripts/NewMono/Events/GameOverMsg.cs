using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



public class GameOverMsg : GameMessage
{
    public GameOverMsg()
    {
        MessageManager.dispatch(this);
    }
}
public class BattleSettlementMsg : GameMessage
{
    object settlementState = null;
    public BattleSettlementMsg(object _settlementState)
    {
        settlementState = _settlementState;
        MessageManager.dispatch(this);
    }
    public object GetSettlementState()
    {
        return settlementState;
    }

}

