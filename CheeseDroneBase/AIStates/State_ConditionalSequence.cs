using System;
using System.Collections.Generic;

namespace CheeseMods.CheeseDroneBase.AIStates;

public class State_ConditionalSequence : State_Sequence
{
    private Func<bool> condition;

    public State_ConditionalSequence(Func<bool> condition, List<AITryState> states, string name, float warmUp, float coolDown)
        : base (states, name, warmUp, coolDown)
    {
        this.condition = condition;
    }

    public override bool CanStart()
    {
        return condition() && base.CanStart();
    }
}
