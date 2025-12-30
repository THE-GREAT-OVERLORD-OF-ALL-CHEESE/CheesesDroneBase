using CheeseMods.CheeseDroneBase.Components;

namespace CheeseMods.CheeseDroneBase.AIStates.FPV;

public class State_WaitForLaunch : AITryState
{
    public override string Name => "Waiting for launch";

    public override float WarmUp => 0.5f;

    public override float CoolDown => 0.5f;

    public FPVDroneAI droneAI;

    public State_WaitForLaunch(FPVDroneAI droneAI)
    {
        this.droneAI = droneAI;
    }

    public override bool CanStart()
    {
        return !droneAI.activated;
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
        return droneAI.activated;
    }
}
