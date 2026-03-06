using UnityEngine;

namespace CheeseMods.CheesesDroneBase.Components;

public class SimpleMultirotorRotorAnimator : MonoBehaviour
{
    public SimpleMultirotorFlightModel fm;
    public Transform rotor;
    public Vector3 axis;
    public float maxRPM;

    public float pitchResponse;
    public float yawResponse;
    public float rollResponse;

    private void Update()
    {
        float finalRpm = fm.RPM
            + fm.PYR.x * pitchResponse
            + fm.PYR.y * yawResponse
            + fm.PYR.z * rollResponse;

        finalRpm = Mathf.Clamp01(finalRpm);
        finalRpm *= maxRPM * 6f;

        rotor.Rotate(axis * finalRpm * Time.deltaTime, Space.Self);
    }
}
