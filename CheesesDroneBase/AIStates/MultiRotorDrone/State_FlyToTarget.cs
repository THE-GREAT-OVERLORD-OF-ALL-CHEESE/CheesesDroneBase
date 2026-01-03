using CheeseMods.CheesesDroneBase.AIStates;
using CheeseMods.CheesesDroneBase.Components;
using UnityEngine;

namespace CheesesDroneBase.AIStates.MultiRotorDrone;

public class State_FlyToTarget : AITryState
{
    public override string Name => "FlyToTarget";

    public override float WarmUp => 0.5f;

    public override float CoolDown => 0.5f;

    public MultiRotorDroneAI droneAI;

    public float hBias;
    public float vBias;
    public float minRange;
    public float maxRange;

    private float hBias2;
    private float vBias2;

    public State_FlyToTarget(MultiRotorDroneAI droneAI, float hBias, float vBias, float minRange, float maxRange)
    {
        this.droneAI = droneAI;
        this.hBias = hBias;
        this.vBias = vBias;
        this.minRange = minRange;
        this.maxRange = maxRange;
    }

    public override bool CanStart()
    {
        if (droneAI.droneTargetBlackboard.target == null || !droneAI.droneTargetBlackboard.canSeeTarget)
            return false;

        Vector3 offset = droneAI.droneTargetBlackboard.target.position - droneAI.pilot.flightModel.tf.position;
        return offset.magnitude > minRange && offset.magnitude < maxRange;
    }

    public override void StartState()
    {
        Debug.Log("Flying to target les goo");
        hBias2 = Random.Range(-0.1f, 0.1f);
        vBias2 = Random.Range(-0.1f, 0.1f);
    }

    public override void UpdateState()
    {
        if (droneAI.droneTargetBlackboard.target == null)
            return;

        Vector3 offset = droneAI.droneTargetBlackboard.target.position - droneAI.pilot.flightModel.tf.position;
        droneAI.pilot.FlyTowardsPos(droneAI.droneTargetBlackboard.target.position
            + Vector3.up * offset.magnitude * (vBias + vBias2)
            + Vector3.Cross(Vector3.up, offset).normalized * offset.magnitude * (hBias + hBias2),
            droneAI.droneTargetBlackboard.target.velocity,
            50f);
        droneAI.pilot.LookDir(offset);
    }

    public override void EndState()
    {
        if (droneAI.droneTargetBlackboard.target == null || !droneAI.droneTargetBlackboard.canSeeTarget)
        {
            Debug.Log("Lost target, wuh oh");
        }
        else
        {
            Debug.Log("Damn we look kinda close now");
        }
    }

    public override bool IsOver()
    {
        if (droneAI.droneTargetBlackboard.target == null || !droneAI.droneTargetBlackboard.canSeeTarget)
            return true;

        Vector3 offset = droneAI.droneTargetBlackboard.target.position - droneAI.pilot.flightModel.tf.position;
        return offset.magnitude < minRange || offset.magnitude > maxRange;
    }
}
