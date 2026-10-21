using System;
using System.Collections.Generic;

[Serializable]
public class TagStatesAndVolumes : EntityComponentDefinition
{
    public Dictionary<States, float> StatesAndVolumes = new Dictionary<States, float>
    {
        { States.Idle, 0 },
        { States.Run, 5 },
        { States.Attack, 7 },
        { States.Jump, 5 },
        { States.Fall, 5 }
    };
}
