using UnityEngine;

namespace CheeseMods.CheeseDroneBase.Components;

public class QuadPilot : MonoBehaviour
{
    public SimpleQuadFlightModel flightModel;

    public float maxSpeed = 30f;
    public float speedFactor = 1f;
    public float velocityFactor = 1f;

    public float steerSpring = 1f;
    public float steerDamping = 1f;

    public void FlyPos(Vector3 pos, float decelFactor)
    {
        FlyVel((pos - flightModel.tf.position) * speedFactor * decelFactor);
    }

    public void FlyTowardsPos(Vector3 pos, float speed)
    {
        FlyVel((pos - flightModel.tf.position).normalized * speed);
    }

    public void FlyVel(Vector3 vel)
    {
        FlyAcceleration((Vector3.ClampMagnitude(vel, maxSpeed) - flightModel.rb.velocity) * velocityFactor);
    }

    public void FlyAcceleration(Vector3 acceleration)
    {
        Vector3 targetThrust = acceleration + Vector3.up * 9.81f;

        float throttle = targetThrust.magnitude / (flightModel.maxTwr * 9.81f);

        Vector3 localAngularVel = flightModel.tf.InverseTransformDirection(flightModel.rb.angularVelocity);
        Vector3 thrustOffset = flightModel.tf.InverseTransformDirection(targetThrust).normalized;

        flightModel.SetInputs(throttle, new Vector3(thrustOffset.z * steerSpring + -localAngularVel.x * steerDamping, -localAngularVel.y * steerDamping, -thrustOffset.x * steerSpring + -localAngularVel.z * steerDamping));
    }
}
