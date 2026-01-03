namespace CheeseMods.CheesesDroneBase.Components;

public abstract class MultiRotorDroneSpawn : AIUnitSpawn
{
    public MultiRotorDroneAI droneAi;

    [VTEvent("Launch", "Take off and destroy the first thing we see")]
    public void Launch()
    {
        droneAi.droneBlackboard.takeOff = true;
    }
}