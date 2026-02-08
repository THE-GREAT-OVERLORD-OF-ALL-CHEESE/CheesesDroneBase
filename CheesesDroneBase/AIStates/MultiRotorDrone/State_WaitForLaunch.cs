using CheeseMods.CheesesDroneBase.Components;

namespace CheeseMods.CheesesDroneBase.AIStates.MultiRotorDrone;

public class State_WaitForLaunch : AITryState
{
    public override string Name => "Waiting for launch";

    public override float WarmUp => 0.25f;

    public override float CoolDown => 0.25f;

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
        droneAI.droneBlackboard.takeOff = true;
    }

    public override bool IsOver()
    {
        return droneAI.droneBlackboard.takeOff
            || (droneAI.droneBlackboard.autolaunch && droneAI.droneTargetBlackboard.canSeeTarget);
    }
}
