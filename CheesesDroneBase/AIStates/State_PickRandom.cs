using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CheeseMods.CheesesDroneBase.AIStates;

public class State_PickRandom : AITryState
{
    private List<AITryState> states;
    private float coolDown;

    private bool started;
    private AITryState currentState;

    public override string Name { get; }
    public override float WarmUp { get; }
    public override float CoolDown { get; }

    public bool Idle => currentState == null;

    public State_PickRandom(List<AITryState> states, string name, float warmUp, float coolDown)
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

        List<AITryState> availableStates = states.Where(s => s.CanStart()).ToList();
        if (availableStates.Count > 0)
        {
            AITryState state = availableStates[Random.Range(0, availableStates.Count)];
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
