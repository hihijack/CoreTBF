using System;
using System.Collections.Generic;

public class FightEventHPHeal : FightEventBase
{
    public Character target;
    public int hpOri;
    public int hpCur;
    public HealResult healResult;

    public FightEventHPHeal(Character target, int hpOri, int hpCur, HealResult healResult)
    {
        this.target = target;
        this.hpOri = hpOri;
        this.hpCur = hpCur;
        this.healResult = healResult;
    }

    internal override FightViewCmdBase ParseToViewCmd()
    {
        return new FightViewCmdHealed(target, hpOri, hpCur, healResult);
    }
}
