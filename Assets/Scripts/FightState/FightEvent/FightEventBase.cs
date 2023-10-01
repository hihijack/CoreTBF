using System;
using System.Collections.Generic;

public class FightEventBase
{
    internal virtual FightViewCmdBase ParseToViewCmd()
    {
        return null;
    }
}
