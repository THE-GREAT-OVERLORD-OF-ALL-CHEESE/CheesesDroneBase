using CheeseMods.CheesesDroneBase.AIStates;
using CheeseMods.CheesesDroneBase.Components;

namespace CheesesDroneBase.AIStates.MultiRotorDrone;

public class State_WaitForLaunch : AITryState
{
    public override string Name => "Waiting for launch";

    public override float WarmUp => 0.5f;

    public override float CoolDown => 0.5f;

    public MultiRotorDroneAI droneAI;

    public State_WaitForLaunch(MultiRotorDroneAI droneAI)
    {
        this.droneAI = droneAI;
    }

    public override bool CanStart()
    {
        return !droneAI.droneBlackboard.takeOff;
    }

    public override void StartState()
    {

    }

    public override void UpdateState()
    {

    }

    public override void EndState()
    {

    }

    public override bool IsOver()
    {
        return droneAI.droneBlackboard.takeOff;
    }
}
