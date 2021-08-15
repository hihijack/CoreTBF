using DefaultNamespace;
using System;
using System.Collections.Generic;

public interface ISkillProcOwner
{
    Character GetOwnerCharacter();
    Skill GetOwnerSkill();
}
