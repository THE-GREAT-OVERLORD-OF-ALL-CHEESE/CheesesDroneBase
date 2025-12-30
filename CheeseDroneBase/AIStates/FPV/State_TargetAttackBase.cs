using CheeseMods.CheeseDroneBase.Components;

namespace CheeseMods.CheeseDroneBase.AIStates.FPV;

public abstract class State_TargetAttackBase : AITryState
{
    public FPVDroneAI droneAI;

    public State_TargetAttackBase(FPVDroneAI droneAI)
    {
        this.droneAI = droneAI;
    }

    public override bool CanStart()
    {
        return droneAI.target != null && droneAI.target.alive;
    }

    public override bool IsOver()
    {
        return droneAI.target == null || !droneAI.target.alive;
    }
}
