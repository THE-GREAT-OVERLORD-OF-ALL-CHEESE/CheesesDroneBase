using UnityEngine;

namespace CheeseMods.CheeseDroneBase.Components;

public class SimpleQuadAudio : MonoBehaviour
{
    public SimpleQuadFlightModel fm;
    public AudioSource source;

    public AnimationCurve pitchCurve;
    public AnimationCurve volumeCurve;

    private void Update()
    {
        float rpm = fm.RPM;

        if (source.isPlaying != (rpm >= 0))
        {
            if (rpm >= 0)
            {
                source.Play();
            }
            else
            {
                source.Stop();
            }
        }

        source.pitch = pitchCurve.Evaluate(rpm);
        source.volume = volumeCurve.Evaluate(rpm);
    }
}
