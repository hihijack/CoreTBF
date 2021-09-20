using System;
using System.Collections.Generic;

public interface ITriggedable
{
     void OnDie(ActionContent sourceContent);
     void OnHitted(ActionContent sourceContent);
     void OnHurtd(ActionContent sourceContent);
     void OnStartAttack(ActionContent sourceContent);
     void OnStartAttacked(ActionContent sourceContent);
     void OnBeforeEndTurn(ActionContent sourceContent);
}
