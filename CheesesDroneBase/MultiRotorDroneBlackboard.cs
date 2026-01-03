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

        public Waypoint waypoint;
        public float waypointAlt = 50f;

        public FollowPath path;
    }
}
