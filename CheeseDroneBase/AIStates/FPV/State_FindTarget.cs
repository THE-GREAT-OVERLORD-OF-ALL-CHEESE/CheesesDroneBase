using CheeseMods.CheeseDroneBase.Components;
using UnityEngine;

namespace CheeseMods.CheeseDroneBase.AIStates.FPV;

public class State_FindTarget : AITryState
{
    public override string Name => "Find Target";

    public override float WarmUp => 0.5f;

    public override float CoolDown => 0.5f;

    public FPVDroneAI droneAI;

    public State_FindTarget(FPVDroneAI droneAI)
    {
        this.droneAI = droneAI;
    }

    public override bool CanStart()
    {
        return droneAI.target == null || !droneAI.target.alive;
    }

    public override void StartState()
    {
        Debug.Log("No target, searching");
    }

    public override void UpdateState()
    {
        droneAI.pilot.FlyPos(VTMapManager.GlobalToWorldPoint(droneAI.basePosition) + Vector3.up * 50f, 0.25f);

        if (droneAI.targetFinder.targetsSeen.Count > 0)
        {
            droneAI.target = droneAI.targetFinder.targetsSeen[Random.Range(0, droneAI.targetFinder.targetsSeen.Count)];
        }
    }

    public override void EndState()
    {
        Debug.Log("Found one :)");
    }

    public override bool IsOver()
    {
        return droneAI.target != null && droneAI.target.alive;
    }
}
