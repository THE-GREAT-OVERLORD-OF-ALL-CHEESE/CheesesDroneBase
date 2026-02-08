namespace CheeseMods.CheesesDroneBase.Components;

public abstract class MultiRotorDroneSpawn : AIUnitSpawn
{
    public MultiRotorDroneAI droneAi;

    [UnitSpawn("Auto Launch")]
    public bool autolaunch;

    [UnitSpawn("Default Waypoint")]
    public Waypoint defaultWaypoint;

    [UnitSpawn("Default Path")]
    public FollowPath defaultPath;

    public override void OnSpawnUnit()
    {
        base.OnSpawnUnit();

        droneAi.droneBlackboard.autolaunch = autolaunch;
        if (defaultWaypoint != null)
        {
            SetWaypoint(defaultWaypoint, 50f);
            return;
        }
        if (defaultWaypoint != null)
        {
            SetWaypoint(defaultWaypoint, 50f);
            return;
        }
        if (defaultPath != null)
        {
            SetPath(defaultPath);
            return;
        }
    }

    [VTEvent("Launch", "Take off and destroy the first thing we see")]
    public void Launch()
    {
        droneAi.droneBlackboard.takeOff = true;
    }

    [VTEvent("Set Auto Launch", "Set the drone to launch when it sees an enemy", new string[]
    {
        "Auto Launch"
    })]
    public void SetAutoLaunch(bool autolaunch)
    {
        droneAi.droneBlackboard.autolaunch = autolaunch;
    }

    /*
    [VTEvent("Set Follow Target", "Follow a unit until we see an enemy.", new string[]
    {
        "Target Unit"
    })]
    public void SetFollowTarget([VTTeamOptionParam(TeamOptions.BothTeams)][VTActionParam(typeof(AnyUnitFilter), null)] UnitReference target)
    {
        if (unitSpawner.isUnspawnedOrDead)
        {
            return;
        }
        if (target.GetSpawner() == null)
        {
            return;
        }
        if (!target.GetSpawner().spawned || !target.GetActor() || !target.GetActor().alive)
        {
            return;
        }

        droneAi.droneBlackboard.ClearGoals();
        Launch();

        droneAi.droneBlackboard.followTarget = target.GetActor();
    }
    */

    [VTEvent("Set Waypoint", "Command the drone to orbit a waypoint.", new string[]
    {
        "Waypoint",
        "Altitude"
    })]
    public void SetWaypoint(Waypoint wpt, [VTRangeParam(10f, 500f)] float alt)
    {
        droneAi.droneBlackboard.ClearGoals();
        Launch();

        droneAi.droneBlackboard.waypoint = wpt;
        droneAi.droneBlackboard.waypointAlt = alt;
    }

    [VTEvent("Set Path", "Set the drone to fly along a path.", new string[]
    {
        "Path"
    })]
    public void SetPath(FollowPath path)
    {
        droneAi.droneBlackboard.ClearGoals();
        Launch();

        droneAi.droneBlackboard.path = path;
    }
}