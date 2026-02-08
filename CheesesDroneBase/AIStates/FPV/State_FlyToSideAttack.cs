using CheeseMods.CheesesDroneBase.Components;
using System.Security.Policy;
using UnityEngine;

namespace CheeseMods.CheesesDroneBase.AIStates.FPV;

public class State_FlyToSideAttack : AITryState
{
    public override string Name => "Fly to Side Attack";

    public override float WarmUp => 0.25f;

    public override float CoolDown => 0.25f;

    public FPVDroneAI droneAI;

    public float maxRange;
    public float finalDistance;
    public float finalHeight;
    public float finalPositionError;
    public float finalVelError;

    private Vector3 targetPos;

    public State_FlyToSideAttack(FPVDroneAI droneAI,
        float maxRange,
        float finalDistance,
        float finalHeight,
        float finalPositionError,
        float finalVelError)
    {
        this.droneAI = droneAI;
        this.maxRange = maxRange;
        this.finalDistance = finalDistance;
        this.finalHeight = finalHeight;
        this.finalPositionError = finalPositionError;
        this.finalVelError = finalVelError;
    }

    public override bool CanStart()
    {
        if (droneAI.droneTargetBlackboard.target == null || !droneAI.droneTargetBlackboard.canSeeTarget)
            return false;

        UpdateTargetPos();

        Vector3 offset = targetPos - droneAI.pilot.flightModel.tf.position;
        return offset.magnitude < maxRange && offset.magnitude > finalPositionError || droneAI.pilot.flightModel.rb.velocity.magnitude > finalVelError;
    }

    public override void StartState()
    {
        UpdateTargetPos();
    }

    public override void UpdateState()
    {
        if (droneAI.droneTargetBlackboard.target == null)
            return;

        UpdateTargetPos();

        droneAI.pilot.FlyPos(targetPos, droneAI.droneTargetBlackboard.target.velocity, 0.25f);
    }

    public override void EndState()
    {

    }

    public override bool IsOver()
    {
        if (droneAI.droneTargetBlackboard.target == null || !droneAI.droneTargetBlackboard.canSeeTarget)
            return true;

        Vector3 offset = targetPos - droneAI.pilot.flightModel.tf.position;
        return offset.magnitude < finalPositionError && droneAI.pilot.flightModel.rb.velocity.magnitude < finalVelError;
    }

    private void UpdateTargetPos()
    {
        if (droneAI.droneTargetBlackboard.target == null || !droneAI.droneTargetBlackboard.canSeeTarget)
            return;

        Vector3 offset = droneAI.droneTargetBlackboard.target.position - droneAI.pilot.flightModel.tf.position;
        Vector3 side = Mathf.Sign(Vector3.Dot(droneAI.droneTargetBlackboard.target.transform.right, offset)) * -droneAI.droneTargetBlackboard.target.transform.right;
        targetPos = side * 20f + droneAI.droneTargetBlackboard.target.position + Vector3.up * 10f;
    }
}
