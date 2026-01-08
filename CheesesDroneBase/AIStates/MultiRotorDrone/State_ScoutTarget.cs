using CheeseMods.CheesesDroneBase.Components;
using UnityEngine;

namespace CheeseMods.CheesesDroneBase.AIStates.FPV;

public class State_ScoutTarget : AITryState
{
    public override string Name => "ScoutLKP";

    public override float WarmUp => 0.5f;

    public override float CoolDown => 0.5f;

    public MultiRotorDroneAI droneAI;

    public float scoutDistance = 1000f;
    public float scoutAltitude = 500f;

    private float scoutDistOffset;
    private float scoutAltOffset;

    public State_ScoutTarget(MultiRotorDroneAI droneAI, float scoutDistance, float scoutAltitude)
    {
        this.droneAI = droneAI;
        this.scoutDistance = scoutDistance;
        this.scoutAltitude = scoutAltitude;
    }

    public override bool CanStart()
    {
        return droneAI.droneTargetBlackboard.target != null && droneAI.droneTargetBlackboard.canSeeTarget;
    }

    public override void StartState()
    {
        scoutDistOffset = scoutDistance * Random.Range(-0.1f, 0.1f);
        scoutAltOffset = scoutAltitude * Random.Range(-0.1f, 0.1f);
    }

    public override void UpdateState()
    {
        if (droneAI.droneTargetBlackboard.target == null)
            return;

        Vector3 pos = droneAI.droneTargetBlackboard.target.position;
        Vector3 offset = pos - droneAI.pilot.flightModel.tf.position;
        offset.y = 0;
        Vector3 targetPos = offset.normalized * -scoutDistance + pos + Vector3.up * scoutAltitude;

        droneAI.pilot.FlyPos(targetPos, droneAI.droneTargetBlackboard.target.velocity, 0.25f);
        droneAI.pilot.LookDir(offset);
    }

    public override void EndState()
    {

    }

    public override bool IsOver()
    {
        return droneAI.droneTargetBlackboard.target == null || !droneAI.droneTargetBlackboard.canSeeTarget;
    }
}
