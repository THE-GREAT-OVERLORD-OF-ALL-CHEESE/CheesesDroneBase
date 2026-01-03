using System.Linq;
using UnityEngine;

namespace CheeseMods.CheesesDroneBase.Components;

public class FPVDroneFuse : MonoBehaviour
{
    public Actor actor;
    public SimpleMultirotorFlightModel flightModel;

    public float contactFuseChance;
    public float maxDelay = 0.5f;

    private bool armed;
    private bool fuseActive;
    private float fuseTimer;

    public ExplosionManager.ExplosionTypes explosionType;
    public float radius;
    public float damage;

    public void Arm()
    {
        armed = true;
    }

    public void Disarm()
    {
        armed = false;
    }

    private void FixedUpdate()
    {
        if (!fuseActive)
            return;

        fuseTimer -= Time.fixedDeltaTime;
        if (fuseTimer < 0)
        {
            Explode();
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (Vector3.Dot(col.contacts.First().normal, flightModel.tf.up) < 0)
        {
            ActivateFuse();
            return;
        }

        if (fuseActive || !armed)
        {
            return;
        }

        if (Random.value < contactFuseChance)
        {
            Explode();
            fuseActive = true;
        }
        else
        {
            flightModel.BreakQuad();
            ActivateFuse();
        }
    }

    public void ActivateFuse()
    {
        fuseTimer = Random.Range(0, maxDelay);
        fuseActive = true;
    }

    public void Explode()
    {
        gameObject.SetActive(false);
        ExplosionManager.instance.CreateExplosionEffect(explosionType, flightModel.tf.position, flightModel.rb.velocity.normalized);
        ExplosionManager.instance.CreateDamageExplosion(flightModel.tf.position, radius, damage, actor, flightModel.rb.velocity);
    }

    public void SelfDestruct()
    {
        gameObject.SetActive(false);
        ExplosionManager.instance.CreateExplosionEffect(ExplosionManager.ExplosionTypes.DebrisPoof, flightModel.tf.position, flightModel.rb.velocity.normalized);
    }
}
