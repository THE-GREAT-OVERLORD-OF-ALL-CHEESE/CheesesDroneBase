using CheeseMods.CheesesDroneBase.Components;
using UnityEngine;

namespace CheesesDroneBase.Components.NetSyncs;

public class SimpleMultirotorFMSync : ContinuousNetSync
{
    public SimpleMultirotorFlightModel fm;

    public override void SendMessage()
    {
        SendRPC("RPC_OnSyncSimpleMultitororFM", fm.Throttle, fm.PYR);
    }

    [VTRPC]
    private void RPC_OnSyncSimpleMultitororFM(float throttle, Vector3 pyr)
    {
        if (!isMine)
        {
            fm.SetInputs(throttle, pyr);
        }
    }

    public override void Interpolate()
    {

    }
}