using System;
using System.Collections.Generic;

public class FightViewCmdBase
{
    private Action<FightViewCmdBase> onEnd;

    public List<FightViewCmdBase> lstChildrenCmd;

    protected float durTime;

    public virtual void Play() { durTime = 0; }
    public virtual void Update(float dt) { durTime += dt; }

    public void SetEndCB(Action<FightViewCmdBase> onEnd)
    {
        this.onEnd = onEnd;
    }

    protected void End()
    {
        onEnd?.Invoke(this);
    }

    public void AddChildCmd(FightViewCmdBase cmd)
    {
        if (lstChildrenCmd == null)
        {
            lstChildrenCmd = new List<FightViewCmdBase>();
        }
        lstChildrenCmd.Add(cmd);
    }

    public virtual void OnPlayableEventLogic(EEventLogic enentType) { }
    public virtual void OnPlayableSetSprite(PBParamSetCharacterSprite param) { }
    public virtual void OnPlayableCreateEff(PBParamCreateEff param) { }
    public virtual void OnPlayableMoveCharacter(PBParamMoveCharacter param) { }
}
