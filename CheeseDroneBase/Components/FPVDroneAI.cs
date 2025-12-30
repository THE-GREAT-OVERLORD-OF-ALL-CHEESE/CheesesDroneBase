using CheeseDroneBase.AIStates.FPV;
using CheeseMods.AIHelicopterGunner.AIStates;
using System.Collections.Generic;
using UnityEngine;

namespace CheeseMods.CheeseDroneBase.Components;

public class FPVDroneAI : MonoBehaviour
{
    public abstract class FPVDroneState
    {
        public abstract void FixedUpate(FPVDroneAI drone);
        public abstract bool IsDone(FPVDroneAI drone);
    }

    public class WaitRandom : FPVDroneState
    {
        private float timer;

        public WaitRandom()
        {
            timer = Random.value * 5f;
        }

        public override void FixedUpate(FPVDroneAI drone)
        {
            timer -= Time.fixedDeltaTime;
        }

        public override bool IsDone(FPVDroneAI drone)
        {
            return timer < 0;
        }
    }

    public class Launch : FPVDroneState
    {
        private float timer = 2f;
        private float launchSpeed = 10f;

        public override void FixedUpate(FPVDroneAI drone)
        {
            timer -= Time.fixedDeltaTime;
            drone.pilot.FlyVel(Vector3.up * launchSpeed);
        }

        public override bool IsDone(FPVDroneAI drone)
        {
            return timer < 0;
        }
    }

    public class FlyTo : FPVDroneState
    {
        public float range;
        public float vBias;
        public float hBias;

        public FlyTo(float range, float veritcalBias, float horizontalBias)
        {
            this.range = range;
            this.vBias = veritcalBias;
            this.hBias = horizontalBias;
        }

        public override void FixedUpate(FPVDroneAI drone)
        {
            Vector3 offset = drone.target.position - drone.pilot.flightModel.tf.position;
            drone.pilot.FlyTowardsPos(drone.target.position
                + Vector3.up * offset.magnitude * vBias
                + Vector3.Cross(Vector3.up, offset).normalized * offset.magnitude * hBias, 50f);
        }

        public override bool IsDone(FPVDroneAI drone)
        {
            Vector3 offset = drone.target.position - drone.pilot.flightModel.tf.position;
            return offset.magnitude < range;
        }
    }

    public class FlyToTopAttack : FPVDroneState
    {
        private Vector3 targetPos;
        public override void FixedUpate(FPVDroneAI drone)
        {
            Vector3 offset = drone.target.position - drone.pilot.flightModel.tf.position;
            offset.y = 0;
            targetPos = offset.normalized * -20f + drone.target.position + Vector3.up * 20f;

            drone.pilot.FlyPos(targetPos, 0.25f);
        }

        public override bool IsDone(FPVDroneAI drone)
        {
            Vector3 offset = targetPos - drone.pilot.flightModel.tf.position;
            return offset.magnitude < 10f && drone.pilot.flightModel.rb.velocity.magnitude < 10f;
        }
    }

    /*
    public class FlyToSideAttack : FPVDroneState
    {
        private Vector3 targetPos;
        public override void FixedUpate(FPVDroneAI drone)
        {
            Vector3 offset = drone.target.position - drone.pilot.flightModel.tf.position;
            Vector3 side = Mathf.Sign(Vector3.Dot(drone.target.right, offset)) * -drone.target.transform.right;
            targetPos = side * 20f + drone.target.position + Vector3.up * 10f;

            drone.pilot.FlyPos(targetPos, 0.25f);
        }

        public override bool IsDone(FPVDroneAI drone)
        {
            Vector3 offset = targetPos - drone.pilot.flightModel.tf.position;
            return offset.magnitude < 10f && drone.pilot.flightModel.rb.velocity.magnitude < 10f;
        }
    }
    */

    public class ArmFuse : FPVDroneState
    {
        public override void FixedUpate(FPVDroneAI drone)
        {
            drone.fuse.Arm();
        }

        public override bool IsDone(FPVDroneAI drone)
        {
            return true;
        }
    }

    public class Terminal : FPVDroneState
    {
        public override void FixedUpate(FPVDroneAI drone)
        {
            drone.pilot.FlyTowardsPos(drone.target.position, Mathf.Max(drone.pilot.flightModel.rb.velocity.magnitude + 5f, 10f));
        }

        public override bool IsDone(FPVDroneAI drone)
        {
            return false;
        }
    }

    public VisualTargetFinder targetFinder;
    public QuadPilot pilot;
    public FPVDroneFuse fuse;
    public Actor target;

    public AITryState states;

    //private List<List<FPVDroneState>> strategies;
    //private List<FPVDroneState> states;
    private int stateId;
    public int overrideStrat = -1;

    public bool activated;
    public bool done;

    public Vector3D basePosition;
    public bool landed = true;

    private void Start()
    {
        landed = true;
        basePosition = VTMapManager.WorldToGlobalPoint(pilot.flightModel.tf.position);
        /*
        if (strategies == null)
        {
            strategies = new List<List<FPVDroneState>>{
                new List<FPVDroneState>
                {
                    new WaitRandom(),
                    new Launch(),
                    new FlyTo(100f, 0.1f, 0f),
                    new ArmFuse(),
                    new Terminal()
                },
                new List<FPVDroneState>
                {
                    new WaitRandom(),
                    new Launch(),
                    new FlyTo(200f, 0.2f, 0f),
                    new ArmFuse(),
                    new FlyToTopAttack(),
                    new Terminal()
                },
                new List<FPVDroneState>
                {
                    new WaitRandom(),
                    new Launch(),
                    new FlyTo(100f, 0.1f, 0.2f),
                    new ArmFuse(),
                    new FlyToSideAttack(),
                    new Terminal()
                },
                new List<FPVDroneState>
                {
                    new WaitRandom(),
                    new Launch(),
                    new FlyTo(100f, 0.1f, -0.2f),
                    new ArmFuse(),
                    new FlyToSideAttack(),
                    new Terminal()
                }
            };
        }
        states = strategies[Random.Range(0, strategies.Count)];
        if (overrideStrat >= 0)
            states = strategies[overrideStrat];
        */
        states = new State_Sequence(
            new List<AITryState> {
                new State_WaitForLaunch(this),
                new State_TakeOff(this),
                new State_FindTarget(this),
                new State_FlyToTarget(this, 0.0f, 0.1f, 200f, 2500f),
                new State_TerminalFlight(this, 0.1f, 0.1f, 250f),
            },
            "States",
            0f,
            0f
        );  
    }

    private void FixedUpdate()
    {
        states.UpdateState();

        /*
        if (!activated || done)
            return;

        states[stateId].FixedUpate(this);
        if (states[stateId].IsDone(this))
        {
            stateId++;
            if (stateId > states.Count)
            {
                done = true;
            }
        }
        */
    }
}