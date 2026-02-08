using CheeseMods.CheesesDroneBase.AIStates;
using CheeseMods.CheesesDroneBase.Components;

namespace CheesesDroneBase.AIStates.MultiRotorDrone;

public class State_ForgetLKP : AITryState
{
    public override string Name => "Forget LKP";

    public override float WarmUp => 0.25f;

    public override float CoolDown => 0.25f;

    public MultiRotorDroneAI droneAI;

    public State_ForgetLKP(MultiRotorDroneAI droneAI)
    {
        this.droneAI = droneAI;
    }

    public override bool CanStart()
    {
        return !droneAI.droneTargetBlackboard.canSeeTarget && droneAI.droneTargetBlackboard.haveLastKnownPosition;
    }

    public override void StartState()
    {
        droneAI.droneTargetBlackboard.ForgetLKP();
    }

    public override void UpdateState()
    {

    }

    public override void EndState()
    {

    }

    public override bool IsOver()
    {
        return true;
    }
}
