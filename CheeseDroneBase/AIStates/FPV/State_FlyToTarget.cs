using CheeseMods.CheeseDroneBase.Components;
using UnityEngine;

namespace CheeseDroneBase.AIStates.FPV
{
    public class State_FlyToTarget : State_TargetAttackBase
    {
        public override string Name => "FlyToTarget";

        public override float WarmUp => 0.5f;

        public override float CoolDown => 0.5f;

        public float hBias;
        public float vBias;
        public float minRange;
        public float maxRange;

        private float hBias2;
        private float vBias2;

        public State_FlyToTarget(FPVDroneAI droneAI, float hBias, float vBias, float minRange, float maxRange): base(droneAI)
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
            if (droneAI.target == null)
                return;

            Vector3 offset = droneAI.target.position - droneAI.pilot.flightModel.tf.position;
            droneAI.pilot.FlyTowardsPos(droneAI.target.position
                + Vector3.up * offset.magnitude * (vBias + vBias2)
                + Vector3.Cross(Vector3.up, offset).normalized * offset.magnitude * (hBias + hBias2), 50f);
        }

        public override void EndState()
        {
            Debug.Log("Damn we look kinda close now");
        }

        public override bool IsOver()
        {
            if (base.IsOver())
                return true;

            Vector3 offset = droneAI.target.position - droneAI.pilot.flightModel.tf.position;
            return offset.magnitude < minRange || offset.magnitude > maxRange;
        }
    }
}
