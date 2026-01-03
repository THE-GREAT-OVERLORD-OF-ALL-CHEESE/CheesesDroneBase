using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace CheeseMods.CheeseDroneBase
{
    public class MultiRotorDroneTargetBlackboard
    {
        public bool engageEnemies;

        public VisualTargetFinder targetFinder;
        public bool canSeeTarget;
        public Actor target;
        public bool haveLastKnownPosition;
        public Vector3D lastKnownPos;

        public float forgetTime;

        public MultiRotorDroneTargetBlackboard(VisualTargetFinder targetFinder)
        {
            this.targetFinder = targetFinder;
        }

        public void Update(float deltaTime)
        {
            if (!engageEnemies)
            {
                canSeeTarget = false;
                target = null;
                return;
            }

            if (target == null || target.gameObject.activeInHierarchy == false || !target.alive)
            {
                // No target, keep looking
                canSeeTarget = false;
                target = null;
                if (targetFinder.targetsSeen.Count > 0)
                {
                    canSeeTarget = true;
                    target = targetFinder.targetsSeen[Random.Range(0, targetFinder.targetsSeen.Count)];
                }
            }
            else
            {
                // check if target can still be seen
                if (targetFinder.targetsSeen.Contains(target))
                {
                    canSeeTarget = true;
                    haveLastKnownPosition = true;
                    lastKnownPos = VTMapManager.WorldToGlobalPoint(target.position);

                    forgetTime -= deltaTime;
                    forgetTime = Mathf.Max(forgetTime, 0f);
                }
                else
                {
                    canSeeTarget = false;
                    forgetTime += deltaTime;

                    if (forgetTime > 3f)
                    {
                        target = null;
                        forgetTime = 0;
                    }
                }
            }
        }
    }
}
