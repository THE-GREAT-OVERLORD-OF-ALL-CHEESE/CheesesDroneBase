using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CheeseMods.CheeseDroneBase.AIStates;

public class State_Sequence : AITryState
{
    private List<AITryState> states;
    private float coolDown;

    private bool started;
    private AITryState currentState;

    public override string Name { get; }
    public override float WarmUp { get; }
    public override float CoolDown { get; }

    public bool Idle => currentState == null;

    public State_Sequence(List<AITryState> states, string name, float warmUp, float coolDown)
    {
        this.states = states;
        Name = name;
        WarmUp = warmUp;
        CoolDown = coolDown;
    }

    public override bool CanStart()
    {
        return states.Any(s => s.CanStart());
    }

    public override void StartState()
    {

    }

    public override void UpdateState()
    {
        coolDown -= Time.deltaTime;

        if (coolDown > 0)
            return;

        if (currentState != null)
        {
            if (!started)
            {
                started = true;
                currentState.StartState();
            }
            if (currentState.IsOver())
            {
                currentState.EndState();
                coolDown = currentState.CoolDown;
                currentState = null;
                return;
            }
            else
            {
                currentState.UpdateState();
                return;
            }
        }

        foreach (AITryState state in states)
        {
            if (state.CanStart())
            {
                //Debug.Log($"{Main.aiGunnerName}: Starting {state.Name}");
                if (state.WarmUp <= 0)
                {
                    state.StartState();
                    started = true;
                }
                else
                {
                    started = false;
                    coolDown = state.WarmUp;
                }
                currentState = state;
                break;
            }
        }
    }

    public override bool IsOver()
    {
        return currentState == null
            && coolDown < 0
            && !states.Any(s => s.CanStart());
    }

    public override void EndState()
    {

    }
}
