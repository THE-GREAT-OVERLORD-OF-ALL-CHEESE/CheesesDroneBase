using CheeseMods.CheesesDroneBase.Components;
using UnityEngine;

namespace CheeseMods.CheesesDroneBase.AIStates.FPV;

public class State_TerminalFlight : State_TargetAttackBase
{
    public override string Name => "Terminal Flight";

    public override float WarmUp => 0.5f;

    public override float CoolDown => 0.5f;

    public float hBias;
    public float vBias;
    public float maxRange;
    public float minRange;

    public State_TerminalFlight(FPVDroneAI droneAI, float hBias, float vBias, float minRange, float maxRange) : base(droneAI)
    {
        this.hBias = hBias;
        this.vBias = vBias;
        this.minRange = minRange;
        this.maxRange = maxRange;
    }

    public override bool CanStart()
    {
        if (!base.CanStart())
            return false;

        Vector3 offset = droneAI.target.position - droneAI.pilot.flightModel.tf.position;
        return offset.magnitude < maxRange && offset.magnitude > minRange;
    }

    public override void StartState()
    {
        Debug.Log("Explody crashy time");
        droneAI.fuse.Arm();
    }

    public override void UpdateState()
    {
        if (droneAI.target == null)
            return;

        droneAI.pilot.FlyTowardsPos(droneAI.target.position, Mathf.Max(droneAI.pilot.flightModel.rb.velocity.magnitude + 5f, 10f));
        Vector3 offset = droneAI.target.position - droneAI.pilot.flightModel.tf.position;
        if (offset.magnitude < minRange && Vector3.Dot(droneAI.pilot.flightModel.rb.velocity, offset) < 0)
        {
            droneAI.fuse.Explode();
        }
    }

    public override void EndState()
    {
        Debug.Log("i've changed my mind...");
        droneAI.fuse.Disarm();
    }

    public override bool IsOver()
    {
        if (base.IsOver())
            return true;

        Vector3 offset = droneAI.target.position - droneAI.pilot.flightModel.tf.position;
        return offset.magnitude > maxRange;
    }
}
