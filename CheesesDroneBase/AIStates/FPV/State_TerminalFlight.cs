using CheeseMods.CheesesDroneBase.Components;
using UnityEngine;

namespace CheeseMods.CheesesDroneBase.AIStates.FPV;

public class State_TerminalFlight : AITryState
{
    public override string Name => "Terminal Flight";

    public override float WarmUp => 0.25f;

    public override float CoolDown => 0.25f;

    public FPVDroneAI droneAI;

    public float maxRange;
    public float minRange;
    public float minAlt;

    public State_TerminalFlight(FPVDroneAI droneAI, float minRange, float maxRange, float minAlt)
    {
        this.droneAI = droneAI;
        this.minRange = minRange;
        this.maxRange = maxRange;
        this.minAlt = minAlt;
    }

    public override bool CanStart()
    {
        if (droneAI.droneTargetBlackboard.target == null || !droneAI.droneTargetBlackboard.canSeeTarget)
            return false;

        Vector3 offset = droneAI.droneTargetBlackboard.target.position - droneAI.pilot.flightModel.tf.position;
        return offset.magnitude < maxRange && offset.magnitude > minRange && (Vector3.Angle(Vector3.up, -offset) < 80f || -offset.y > minAlt);
    }

    public override void StartState()
    {

    }

    public override void UpdateState()
    {
        if (droneAI.droneTargetBlackboard.target == null)
            return;

        Vector3 offset = droneAI.droneTargetBlackboard.target.position - droneAI.pilot.flightModel.tf.position;

        droneAI.pilot.FlyTowardsPos(droneAI.droneTargetBlackboard.target.position,
            droneAI.droneTargetBlackboard.target.velocity,
            Mathf.Max(droneAI.pilot.flightModel.rb.velocity.magnitude + 5f, 10f));
        droneAI.pilot.LookDir(offset);

        if (offset.magnitude < minRange && Vector3.Dot(droneAI.pilot.flightModel.rb.velocity, offset) < 0)
        {
            droneAI.fuse.Explode();
        }
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
