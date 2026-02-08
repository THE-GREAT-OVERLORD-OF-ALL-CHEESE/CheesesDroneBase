using CheeseMods.CheesesDroneBase.AIStates;
using CheeseMods.CheesesDroneBase.Components;
using UnityEngine;

namespace CheesesDroneBase.AIStates.MultiRotorDrone;

public class State_FollowUnit : AITryState
{
    public override string Name => "ScoutLKP";

    public override float WarmUp => 0.25f;

    public override float CoolDown => 0.25f;

    public MultiRotorDroneAI droneAI;

    public float scoutDistance = 100f;
    public float scoutAltitude = 50f;

    private float scoutDistOffset;
    private float scoutAltOffset;

    public State_FollowUnit(MultiRotorDroneAI droneAI, float scoutDistance, float scoutAltitude)
    {
        this.droneAI = droneAI;
        this.scoutDistance = scoutDistance;
        this.scoutAltitude = scoutAltitude;
    }

    public override bool CanStart()
    {
        return !droneAI.droneTargetBlackboard.canSeeTarget && droneAI.droneTargetBlackboard.haveLastKnownPosition;
    }

    public override void StartState()
    {
        scoutDistOffset = scoutDistance * Random.Range(-0.1f, 0.1f);
        scoutAltOffset = scoutAltitude * Random.Range(-0.1f, 0.1f);
    }

    public override void UpdateState()
    {
        Vector3 lastKnownPos = VTMapManager.GlobalToWorldPoint(droneAI.droneTargetBlackboard.lastKnownPos);
        Vector3 offset = lastKnownPos - droneAI.pilot.flightModel.tf.position;
        offset.y = 0;
        Vector3 targetPos = offset.normalized * -(scoutDistance + scoutDistOffset) + lastKnownPos + Vector3.up * (scoutAltitude + scoutAltOffset);

        droneAI.pilot.FlyPos(targetPos, Vector3.zero, 0.25f);
        droneAI.pilot.LookDir(offset);
    }

    public override void EndState()
    {

    }

    public override bool IsOver()
    {
        return droneAI.droneTargetBlackboard.canSeeTarget;
    }
}
