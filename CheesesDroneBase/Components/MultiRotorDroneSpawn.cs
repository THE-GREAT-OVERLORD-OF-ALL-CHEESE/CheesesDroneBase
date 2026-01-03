using System.IO;

namespace CheeseMods.CheesesDroneBase.Components;

public abstract class MultiRotorDroneSpawn : AIUnitSpawn
{
    public MultiRotorDroneAI droneAi;

    [UnitSpawn("Default Waypoint")]
    public Waypoint defaultWaypoint;

    [UnitSpawn("Default Path")]
    public FollowPath defaultPath;

    public override void OnSpawnUnit()
    {
        base.OnSpawnUnit();

        if (defaultWaypoint != null)
        {
            SetWaypoint(defaultWaypoint, 50f);
        }
        if (defaultPath != null)
        {
            SetPath(defaultPath);
        }
    }

    [VTEvent("Launch", "Take off and destroy the first thing we see")]
    public void Launch()
    {
        droneAi.droneBlackboard.takeOff = true;
    }

    [VTEvent("Set Waypoint", "Command the aircraft to orbit a waypoint.", new string[]
    {
        "Waypoint",
        "Altitude"
    })]
    public void SetWaypoint(Waypoint wpt, [VTRangeParam(10f, 500f)] float alt)
    {
        Launch();

        droneAi.droneBlackboard.path = null;

        droneAi.droneBlackboard.waypoint = wpt;
        droneAi.droneBlackboard.waypointAlt = alt;
    }

    [VTEvent("Set Path", "Set the aircraft to fly along a path.", new string[]
    {
        "Path"
    })]
    public void SetPath(FollowPath path)
    {
        Launch();

        droneAi.droneBlackboard.waypoint = null;

        droneAi.droneBlackboard.path = path;
    }
}