using CheeseMods.CheesesDroneBase.Components;
using UnityEngine;

namespace CheeseMods.CheesesDroneBase.AIStates.FPV;

public class State_GoAround : AITryState
{
    public override string Name => "Go Around";

    public override float WarmUp => 0.5f;

    public override float CoolDown => 0.5f;

    public FPVDroneAI droneAI;

    public float maxRange;

    public State_GoAround(FPVDroneAI droneAI, float maxRange)
    {
        this.droneAI = droneAI;
        this.maxRange = maxRange;
    }

    public override bool CanStart()
    {
        if (droneAI.droneTargetBlackboard.target == null || !droneAI.droneTargetBlackboard.canSeeTarget)
            return false;

        Vector3 offset = droneAI.droneTargetBlackboard.target.position - droneAI.pilot.flightModel.tf.position;
        return offset.magnitude < maxRange;
    }

    public override void StartState()
    {
        Debug.Log("Go around");
    }

    public override void UpdateState()
    {
        droneAI.pilot.FlyVel(Vector3.up * 20f);
    }

    public override void EndState()
    {
        Debug.Log("Thats far enough...");
    }

    public override bool IsOver()
    {
        if (droneAI.droneTargetBlackboard.target == null || !droneAI.droneTargetBlackboard.canSeeTarget)
            return true;

        Vector3 offset = droneAI.droneTargetBlackboard.target.position - droneAI.pilot.flightModel.tf.position;
        return offset.magnitude > maxRange;
    }
}
