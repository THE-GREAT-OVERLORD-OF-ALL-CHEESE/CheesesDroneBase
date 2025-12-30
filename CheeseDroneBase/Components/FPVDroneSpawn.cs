using UnityEngine;

namespace CheeseMods.CheeseDroneBase.Components
{
    public class FPVDroneSpawn : AIUnitSpawn
    {
        public FPVDroneAI fpvAi;

        /*
        public override void OnSpawnUnit()
        {
            Debug.Log("FPV drone spawning");
            gameObject.SetActive(true);
            base.OnSpawnUnit();
        }
        */

        [VTEvent("Launch", "Take off and destroy the first thing we see")]
        public void Launch()
        {
            fpvAi.activated = true;
        }
    }
}
