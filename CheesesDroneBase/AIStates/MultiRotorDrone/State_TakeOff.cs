using CheeseMods.CheesesDroneBase.Components;
using UnityEngine;

namespace CheeseMods.CheesesDroneBase.AIStates.MultiRotorDrone;

public class State_TakeOff : AITryState
{
    public override string Name => "Take Off";

    public override float WarmUp => randomStartUpTime;

    public override float CoolDown => 0.25f;

    public MultiRotorDroneAI droneAI;
    private float randomStartUpTime;

    private float launchTimer;

    public State_TakeOff(MultiRotorDroneAI droneAI)
    {
        this.droneAI = droneAI;
    }

    public override bool CanStart()
    {
        randomStartUpTime = Random.value * 5f;
        return droneAI.droneBlackboard.landed;
    }

    public override void StartState()
    {
        droneAI.droneBlackboard.landed = false;
        launchTimer = 2f;
    }

    public override void UpdateState()
    {
        droneAI.pilot.FlyVel(Vector3.up * 10f);
        launchTimer -= Time.fixedDeltaTime;
    }

    public override void EndState()
    {

    }

    public override bool IsOver()
    {
        return launchTimer < 0;
    }
}
