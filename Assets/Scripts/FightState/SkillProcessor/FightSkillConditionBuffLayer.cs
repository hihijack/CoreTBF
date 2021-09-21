using SimpleJSON;
using System;

public class FightSkillConditionBuffLayer : FightSkillConditionBase
{
    int buffID;
    int layer;
    public FightSkillConditionBuffLayer(JSONNode jsonData) : base(jsonData)
    {
    }

    public override bool IsTrue()
    {
        var owner = GetOwner();
        var buff = owner.GetBuff(buffID);
        return buff != null && buff.GetLayer() >= layer;
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        buffID = jsonData["buff_id"].AsInt;
        layer = jsonData["layer"].AsInt;
    }
}
