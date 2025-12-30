using ModLoader.Framework;
using ModLoader.Framework.Attributes;
using UnityEngine;

namespace CheeseMods.CheeseDroneBase;

[ItemId("cheese.dronebase")]
public class Main : VtolMod
{
    public string ModFolder;

    private void Awake()
    {
        Debug.Log("Cheese's Drone Base: Ready!");
    }

    public override void UnLoad()
    {
        Debug.Log("Cheese's Drone Base: Nothing to unload");
    }
}