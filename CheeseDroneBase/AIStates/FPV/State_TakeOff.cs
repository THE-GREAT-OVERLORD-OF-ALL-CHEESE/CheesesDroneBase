using CheeseMods.CheeseDroneBase.Components;
using UnityEngine;

namespace CheeseMods.CheeseDroneBase.AIStates.FPV;

public class State_TakeOff : AITryState
{
    public override string Name => "Take Off";

    public override float WarmUp => randomStartUpTime;

    public override float CoolDown => 0.5f;

    public FPVDroneAI droneAI;
    private float randomStartUpTime;

    private float launchTimer;

    public State_TakeOff(FPVDroneAI droneAI)
    {
        this.droneAI = droneAI;
    }

    public override bool CanStart()
    {
        randomStartUpTime = Random.value * 5f;
        return droneAI.landed;
    }

    public override void StartState()
    {
        droneAI.landed = false;
        launchTimer = 2f;
        Debug.Log("waited randomly, time to launch");
    }

    public override void UpdateState()
    {
        droneAI.pilot.FlyVel(Vector3.up * 10f);
        launchTimer -= Time.fixedDeltaTime;
    }

    public override void EndState()
    {
        Debug.Log("Done launching lets goo");
    }

    public override bool IsOver()
    {
        return launchTimer < 0;
    }
}
