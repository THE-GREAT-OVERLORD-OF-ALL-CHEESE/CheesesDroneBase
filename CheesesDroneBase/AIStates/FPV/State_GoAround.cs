using CheeseMods.CheesesDroneBase.Components;
using UnityEngine;

namespace CheeseMods.CheesesDroneBase.AIStates.FPV;

public class State_GoAround : AITryState
{
    public override string Name => "Go Around";

    public override float WarmUp => 0.25f;

    public override float CoolDown => 0.25f;

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

    }

    public override void UpdateState()
    {
        Vector3 dir = droneAI.pilot.flightModel.rb.velocity.normalized;
        dir.y = 0f;

        Vector3 right = Vector3.Cross(Vector3.up, dir);
        dir = Quaternion.AngleAxis(30f, right) * dir;

        droneAI.pilot.FlyVel(dir.normalized * 50f);
    }

    public override void EndState()
    {

    }

    public override bool IsOver()
    {
        if (droneAI.droneTargetBlackboard.target == null || !droneAI.droneTargetBlackboard.canSeeTarget)
            return true;

        Vector3 offset = droneAI.droneTargetBlackboard.target.position - droneAI.pilot.flightModel.tf.position;
        return offset.magnitude > maxRange;
    }
}
