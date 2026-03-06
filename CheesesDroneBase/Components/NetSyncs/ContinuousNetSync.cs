using UnityEngine;
using VTNetworking;

namespace CheesesDroneBase.Components.NetSyncs;

public abstract class ContinuousNetSync : VTNetSyncRPCOnly
{
    [Range(1f, 30f)]
    public float syncRate = 30f;
    public float InverseSyncRate => 1f / syncRate;
    private float timer;

    private void Start()
    {
        timer = Random.Range(0, InverseSyncRate);
    }

    private void FixedUpdate()
    {
        if (isMine)
        {
            timer += Time.fixedDeltaTime;
            if (timer > InverseSyncRate)
            {
                timer -= InverseSyncRate;
                SendMessage();
            }
        }
        else
        {
            Interpolate();
        }
    }

    public abstract void SendMessage();
    public abstract void Interpolate();
}