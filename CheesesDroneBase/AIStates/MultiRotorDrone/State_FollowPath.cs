using CheeseMods.CheesesDroneBase.Components;
using UnityEngine;

namespace CheeseMods.CheesesDroneBase.AIStates.MultiRotorDrone;

public class State_FollowPath : AITryState
{
    public override string Name => "Hover at Waypoint";

    public override float WarmUp => 0.25f;

    public override float CoolDown => 0.25f;

    public MultiRotorDroneAI droneAI;

    public State_FollowPath(MultiRotorDroneAI droneAI)
    {
        this.droneAI = droneAI;
    }

    public override bool CanStart()
    {
        return droneAI.droneTargetBlackboard.target == null && droneAI.droneBlackboard.path != null;
    }

    public override void StartState()
    {

    }

    public override void UpdateState()
    {
        if (droneAI.droneBlackboard.path == null)
            return;

        Vector3 target = droneAI.droneBlackboard.path.GetFollowPoint(droneAI.pilot.flightModel.tf.position, 50f, out float currentT);
        droneAI.pilot.FlyTowardsPos(target, Vector3.zero, 50f);
        droneAI.pilot.LookDir(target - droneAI.pilot.flightModel.tf.position);

        if (!droneAI.droneBlackboard.path.finalLooped && currentT >= 0.999f)
        {
            droneAI.droneBlackboard.path = null;
        }
    }

    public override void EndState()
    {

    }

    public override bool IsOver()
    {
        return droneAI.droneTargetBlackboard.target != null || droneAI.droneBlackboard.path == null;
    }
}
