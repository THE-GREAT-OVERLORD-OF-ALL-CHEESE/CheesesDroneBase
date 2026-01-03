using CheeseMods.CheesesDroneBase.Components;
using UnityEngine;

namespace CheeseMods.CheesesDroneBase.AIStates.MultiRotorDrone;

public class State_HoverAtBase : AITryState
{
    public override string Name => "Find Target";

    public override float WarmUp => 0.5f;

    public override float CoolDown => 0.5f;

    public MultiRotorDroneAI droneAI;

    private float timer;

    public State_HoverAtBase(MultiRotorDroneAI droneAI)
    {
        this.droneAI = droneAI;
    }

    public override bool CanStart()
    {
        return true;
    }

    public override void StartState()
    {
        Debug.Log("Hover at base");
        timer = 0f;
    }

    public override void UpdateState()
    {
        droneAI.pilot.FlyPos(VTMapManager.GlobalToWorldPoint(droneAI.droneBlackboard.basePosition) + Vector3.up * 50f, Vector3.zero, 0.25f);
        timer += Time.fixedDeltaTime;
    }

    public override void EndState()
    {

    }

    public override bool IsOver()
    {
        return timer > 5f;
    }
}
