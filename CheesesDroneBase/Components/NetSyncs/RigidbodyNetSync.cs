using CheesesDroneBase.Components.NetSyncs;
using UnityEngine;

namespace CheeseMods.CheesesDroneBase.Components.NetSyncs;

public class RigidbodyNetSync : ContinuousNetSync
{
    public Rigidbody rb;

    private bool havePos;
    private float lastTime;
    private Vector3D lastPos;
    private Vector3 lastVel;
    private Quaternion lastRotation;

    private float lerpSpeed = 3f;
    private float teleportThreshold = 50f;

    public override void SendMessage()
    {
        SendRPC("RPC_OnSyncRigidBody", VTMapManager.WorldToGlobalPoint(rb.position), rb.rotation, rb.velocity, rb.angularVelocity);
    }

    [VTRPC]
    private void RPC_OnSyncRigidBody(Vector3D position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
    {
        if (!isMine)
        {
            havePos = true;
            rb.isKinematic = true;
            lastTime = Time.time;
            lastPos = position;
            lastVel = velocity;
            lastRotation = rotation;

            if ((VTMapManager.GlobalToWorldPoint(position) - rb.position).magnitude > teleportThreshold)
            {
                rb.position = VTMapManager.GlobalToWorldPoint(position);
                rb.rotation = rotation;
            }
        }
    }

    public override void Interpolate()
    {
        Vector3 startingPos = rb.position;
        startingPos += lastVel * Time.fixedDeltaTime;

        Vector3 targetPos = VTMapManager.GlobalToWorldPoint(lastPos) + lastVel * (Time.time - lastTime);
        Vector3 finalPos = Vector3.Lerp(startingPos, targetPos, Time.fixedDeltaTime * lerpSpeed);

        rb.MovePosition(finalPos);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, lastRotation, Time.fixedDeltaTime));
    }
}
