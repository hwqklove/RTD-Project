using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WayPointsFamily
{
    /// <summary>
    /// Setup the Waypoint routes
    /// It should connect start & and points
    /// </summary>
    public class WayPointsGroup : MonoBehaviour
    {
        public PositionConstraint XYZConstraint = PositionConstraint.XYZ;

        [HideInInspector]
        public List<WayPoint> waypoints;

        private void Awake()
        {
            if (waypoints != null)
            {
                foreach (WayPoint wp in waypoints)
                {
                    wp.SetWayPointGroup(this);
                }
            }
        }

        public void Start ()
        {

        }

        public void Update ()
        {

        }

        public List<WayPoint> GetWayPointChildren (bool reparent = true)
        {
            if (waypoints == null)
            {
                waypoints = new List<WayPoint>();
            }

            if (reparent == true)
            {
                foreach (WayPoint wp in waypoints)
                {
                    wp.SetWayPointGroup(this);
                }
            }

            return waypoints;
        }

        public void AddWayPoint(WayPoint wp, int ndx = -1)
        {
            if (waypoints == null)
            {
                waypoints = new List<WayPoint>();
            }
            if (ndx == -1)
            {
                waypoints.Add(wp);
            }
            else
            {
                waypoints.Insert(ndx, wp);
            }

            wp.SetWayPointGroup(this);
        }
    }
}
