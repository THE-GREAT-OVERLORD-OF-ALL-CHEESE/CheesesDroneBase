using UnityEngine;

namespace CheeseMods.CheesesDroneBase.Components;

public class SimpleMultirotorFlightModel : MonoBehaviour, IWindReceiver
{
    public Rigidbody rb;
    public Transform tf;

    private Vector3 windVel;

    public float maxTwr;
    public float spoolSpeed;
    public float drag;
    public float torque;
    public float torqueDrag;

    public float RPM { get; private set; }

    private float throttle;
    public Vector3 pyr { get; private set; }
    private bool broken;

    private void Start()
    {
        rb.maxAngularVelocity = Mathf.PI * 8;
    }

    private void FixedUpdate()
    {
        if ((RPM == 0 && throttle == 0) || broken)
        {
            return;
        }

        RPM += Mathf.Clamp(throttle - RPM, -spoolSpeed * Time.fixedDeltaTime, spoolSpeed * Time.fixedDeltaTime);
        rb.AddForce(tf.up * RPM * maxTwr * 9.81f + CurrentDrag(), ForceMode.Acceleration);

        Vector3 localAngularVel = tf.InverseTransformDirection(rb.angularVelocity);

        rb.AddRelativeTorque(-localAngularVel * torqueDrag + pyr * torque, ForceMode.Acceleration);
    }

    public void SetInputs(float throttle, Vector3 pyr)
    {
        this.throttle = Mathf.Clamp01(throttle);
        pyr.x = Mathf.Clamp(pyr.x, -1f, 1f);
        pyr.y = Mathf.Clamp(pyr.y, -1f, 1f);
        pyr.z = Mathf.Clamp(pyr.z, -1f, 1f);
        this.pyr = pyr;
    }

    public void BreakQuad()
    {
        broken = true;
    }

    public Vector3 CurrentDrag()
    {
        Vector3 airSpeed = rb.velocity - windVel;
        return airSpeed * airSpeed.magnitude * -drag;
    }

    public void SetWind(Vector3 w)
    {
        windVel = w;
    }

    public Vector3 GetPosition()
    {
        return tf.position;
    }
}
