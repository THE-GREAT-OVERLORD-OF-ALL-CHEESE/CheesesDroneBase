using CheeseMods.CheesesDroneBase.AIStates;
using CheeseMods.CheesesDroneBase.AIStates.FPV;
using CheeseMods.CheesesDroneBase.AIStates.MultiRotorDrone;
using CheesesDroneBase.AIStates.MultiRotorDrone;
using System.Collections.Generic;

namespace CheeseMods.CheesesDroneBase.Components;

public class FPVDroneAI : MultiRotorDroneAI
{
    private AITryState states;

    protected override AITryState GenerateStates()
    {
        return new State_Sequence(
            new List<AITryState>
            {
                new State_WaitForLaunch(this),
                new State_TakeOff(this),
                new State_PickRandom(
                    new List<AITryState>{
                        new State_Sequence(
                            new List<AITryState>
                            {
                                new State_FlyToTarget(this, 0.0f, 0.1f, 200f, 2500f),
                                new State_TerminalFlight(this, 30f, 250f, 10f),
                            },
                            "Basic Attack",
                            0f,
                            0f
                        ),
                        new State_Sequence(
                            new List<AITryState>
                            {
                                new State_FlyToTarget(this, -0.2f, 0.1f, 250f, 2500f),
                                new State_FlyToSideAttack(this, 300f, 15f, 10f, 5f, 3f),
                                new State_TerminalFlight(this, 10f, 100f, 5f),
                            },
                            "Side Attack Left",
                            0f,
                            0f
                        ),
                        new State_Sequence(
                            new List<AITryState>
                            {
                                new State_FlyToTarget(this, 0.2f, 0.1f, 250f, 2500f),
                                new State_FlyToSideAttack(this, 300f, 15f, 10f, 5f, 3f),
                                new State_TerminalFlight(this, 10f, 100f, 5f),
                            },
                            "Side Attack Right",
                            0f,
                            0f
                        ),
                        new State_Sequence(
                            new List<AITryState>
                            {
                                new State_FlyToTarget(this, 0.0f, 0.1f, 250f, 2500f),
                                new State_FlyToTopAttack(this, 300f, 10f, 20f, 5f, 3f),
                                new State_TerminalFlight(this, 10f, 100f, 10f),
                            },
                            "Top Attack",
                            0f,
                            0f
                        )
                    },
                    "Random Attack",
                    0f,
                    0f
                ),
                new State_GoAround(this, 250f),
                new State_ScoutLKP(this, 200f, 100f, 0f, 30f),
                new State_ScoutLKP(this, 100f, 100f, 30f, 45f),
                new State_ScoutLKP(this, 50f, 100f, 45f, 60),
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
}