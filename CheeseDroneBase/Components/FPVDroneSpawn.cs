using UnityEngine;

namespace CheeseMods.CheeseDroneBase.Components
{
    public class FPVDroneSpawn : AIUnitSpawn
    {
        public FPVDroneAI fpvAi;

        [VTEvent("Launch", "Take off and destroy the first thing we see")]
        public void Launch()
        {
            fpvAi.activated = true;
        }
    }
}
