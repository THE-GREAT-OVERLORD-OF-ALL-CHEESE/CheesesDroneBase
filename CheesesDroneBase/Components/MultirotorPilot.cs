using UnityEngine;

namespace CheeseMods.CheesesDroneBase.Components;

public class MultirotorPilot : MonoBehaviour
{
    public SimpleMultirotorFlightModel flightModel;

    private Vector3 lookDir;
    private Vector3 targetAcceleration;

    public float maxSpeed = 30f;
    public float maxAcceleration = 10f;
    public float speedFactor = 1f;
    public float velocityFactor = 1f;

    public float steerSpring = 1f;
    public float steerDamping = 1f;

    private bool flying;

    private void Start()
    {
        lookDir = flightModel.tf.forward;
    }

    private void FixedUpdate()
    {
        if (flying)
        {
            FlyAcceleration(targetAcceleration);
        }
    }

    public void LookDir(Vector3 lookDir)
    {
        flying = true;
        this.lookDir = lookDir;
    }

    public void FlyPos(Vector3 pos, Vector3 vel, float decelFactor)
    {
        FlyVel((pos - flightModel.tf.position) * speedFactor * decelFactor + vel);
    }

    public void FlyTowardsPos(Vector3 pos, Vector3 vel, float speed)
    {
        FlyVel(vel + (pos - flightModel.tf.position).normalized * speed);
    }

    public void FlyVel(Vector3 vel)
    {
        flying = true;
        targetAcceleration = (Vector3.ClampMagnitude(vel, maxSpeed) - flightModel.rb.velocity) * velocityFactor;
    }

    public void FlyAcceleration(Vector3 acceleration)
    {
        Vector3 targetThrust = Vector3.ClampMagnitude(acceleration, maxAcceleration) + Vector3.up * 9.81f;

        float throttle = targetThrust.magnitude / (flightModel.maxTwr * 9.81f);

        Vector3 localAngularVel = flightModel.tf.InverseTransformDirection(flightModel.rb.angularVelocity);
        Vector3 thrustOffset = flightModel.tf.InverseTransformDirection(targetThrust).normalized;
        Vector3 steerOffset = flightModel.tf.InverseTransformDirection(lookDir).normalized;

        flightModel.SetInputs(throttle,
            new Vector3(thrustOffset.z * steerSpring + -localAngularVel.x * steerDamping,
                steerOffset.x * steerSpring + -localAngularVel.y * steerDamping,
                -thrustOffset.x * steerSpring + -localAngularVel.z * steerDamping));
    }
}
