using Data;
using DefaultNamespace;

public enum ECharacterForRaidState
{
    Normal,
    Dying,
    Dead
}

public class CharacterForRaid
{
    public RoleBaseData roleData;
    public PropData propData;
    public ECharacterForRaidState state;

    public CharacterForRaid(int tid)
    {
       roleData = RoleDataer.Inst.Get(tid);
       propData = new PropData();
       propData.MaxHP = roleData.hp;
       propData.hp = propData.MaxHP;
       state = ECharacterForRaidState.Normal;
    }
}