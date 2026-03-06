using CheeseMods.CheeseDroneBase;
using CheeseMods.CheesesDroneBase.AIStates;
using CheeseMods.CheesesDroneBase.AIStates.FPV;
using CheeseMods.CheesesDroneBase.AIStates.MultiRotorDrone;
using CheesesDroneBase.AIStates.MultiRotorDrone;
using System.Collections.Generic;
using UnityEngine;

namespace CheeseMods.CheesesDroneBase.Components;

public class MultiRotorDroneAI : MonoBehaviour, IEngageEnemies
{
    public VisualTargetFinder targetFinder;
    public MultirotorPilot pilot;
    public FPVDroneFuse fuse;

    private AITryState states;
    public MultiRotorDroneBlackboard droneBlackboard = new MultiRotorDroneBlackboard();
    public MultiRotorDroneTargetBlackboard droneTargetBlackboard;

    private void Awake()
    {
        droneTargetBlackboard = new MultiRotorDroneTargetBlackboard(targetFinder);
    }

    private void Start()
    {
        droneBlackboard.basePosition = VTMapManager.WorldToGlobalPoint(pilot.flightModel.tf.position);
        states = GenerateStates();
    }

    protected virtual AITryState GenerateStates()
    {
        return new State_Sequence(
            new List<AITryState> {
                new State_WaitForLaunch(this),
                new State_TakeOff(this),
                new State_FlyToTarget(this, 0.0f, 0.1f, 1500f, 2500f),
                new State_ScoutTarget(this, 1000f, 500f),
                new State_ScoutLKP(this, 1000f, 500f, 0f, 30f),
                new State_ScoutLKP(this, 500f, 500f, 30f, 60f),
                new State_ScoutLKP(this, 250f, 500f, 60f, 85f),
                new State_ForgetLKP(this),
                new State_FollowPath(this),
                new State_HoverAtWaypoint(this),
                new State_HoverAtBase(this),
            },
            "States",
            0f,
            0f
        );
    }

    private void FixedUpdate()
    {
        if (!VTScenario.isScenarioHost)
        {
            return;
        }

        droneTargetBlackboard.Update(Time.fixedDeltaTime);
        states.UpdateState();
    }

    public void SetEngageEnemies(bool engage)
    {
        if (droneTargetBlackboard == null)
        {
            droneTargetBlackboard = new MultiRotorDroneTargetBlackboard(targetFinder);
        }
        droneTargetBlackboard.engageEnemies = engage;
    }
}