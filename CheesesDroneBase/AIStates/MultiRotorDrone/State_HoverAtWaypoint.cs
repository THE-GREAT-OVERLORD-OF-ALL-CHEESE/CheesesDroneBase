using CheeseMods.CheesesDroneBase.Components;
using UnityEngine;

namespace CheeseMods.CheesesDroneBase.AIStates.MultiRotorDrone;

public class State_HoverAtWaypoint : AITryState
{
    public override string Name => "Follow Path";

    public override float WarmUp => 0.5f;

    public override float CoolDown => 0.5f;

    public MultiRotorDroneAI droneAI;

    public State_HoverAtWaypoint(MultiRotorDroneAI droneAI)
    {
        this.droneAI = droneAI;
    }

    public override bool CanStart()
    {
        return droneAI.droneTargetBlackboard.target == null && droneAI.droneBlackboard.waypoint != null;
    }

    public override void StartState()
    {
        Debug.Log("Hover at waypoint");
    }

    public override void UpdateState()
    {
        if (droneAI.droneBlackboard.waypoint == null)
            return;

        Vector3 targetPos = droneAI.droneBlackboard.waypoint.worldPosition + Vector3.up * droneAI.droneBlackboard.waypointAlt;
        Vector3 offset = targetPos - droneAI.pilot.flightModel.tf.position;
        droneAI.pilot.FlyPos(targetPos, Vector3.zero, 0.25f);
        droneAI.pilot.LookDir(offset);
    }

    public override void EndState()
    {

    }

    public override bool IsOver()
    {
        return droneAI.droneTargetBlackboard.target != null || droneAI.droneBlackboard.waypoint == null;
    }
}
