using CheeseMods.CheeseDroneBase;
using CheeseMods.CheesesDroneBase.AIStates;
using CheeseMods.CheesesDroneBase.AIStates.MultiRotorDrone;
using System.Collections.Generic;
using UnityEngine;

namespace CheeseMods.CheesesDroneBase.Components;

public class MultiRotorDroneAI : MonoBehaviour
{
    public VisualTargetFinder targetFinder;
    public MultirotorPilot pilot;
    public FPVDroneFuse fuse;

    private AITryState states;
    public MultiRotorDroneBlackboard droneBlackboard = new MultiRotorDroneBlackboard();

    private void Start()
    {
        droneBlackboard.basePosition = VTMapManager.WorldToGlobalPoint(pilot.flightModel.tf.position);
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
        states = GenerateStates();
    }

    protected virtual AITryState GenerateStates()
    {
        return new State_Sequence(
            new List<AITryState>
            {
                new State_WaitForLaunch(this),
                new State_TakeOff(this),
                new State_HoverAtBase(this)
                //new State_FindTarget(this),
                //new State_FlyToTarget(this, 0.0f, 0.1f, 200f, 2500f),
                //new State_TerminalFlight(this, 0.1f, 0.1f, 10f, 250f),
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