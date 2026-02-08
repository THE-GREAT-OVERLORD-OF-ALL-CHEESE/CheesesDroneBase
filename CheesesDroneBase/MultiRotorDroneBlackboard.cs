namespace CheeseMods.CheeseDroneBase
{
    public class MultiRotorDroneBlackboard
    {
        // basic info
        public Vector3D basePosition;

        // current state
        public bool landed = true;

        // goal memory
        public bool takeOff;

        public bool autolaunch;

        public Actor followTarget;

        public Waypoint waypoint;
        public float waypointAlt = 50f;

        public FollowPath path;

        public void ClearGoals()
        {
            followTarget = null;
            waypoint = null;
            path = null;
        }
    }
}
