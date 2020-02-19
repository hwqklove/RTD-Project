using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WayPointsFamily
{
    public enum PositionConstraint
    {
        XYZ,
        XY,
        XZ
    }

    public enum TravelDirection
    {
        FORWARD,
        REVERSE
    }

    public enum EndPointBehavior
    {
        STOP,
        LOOP,
        PINGPONG
    }

    public enum MoveType
    {
        LERP,
        FORWARD_TRANSLATE
    }

    [Serializable]
    public class WayPoint
    {
        public Vector3 position;

        [HideInInspector]
        public Quaternion rotation = Quaternion.identity;

        [HideInInspector]
        [SerializeField]
        Vector3 xyzPosition;

        [HideInInspector]
        [SerializeField]
        Vector3 xyPosition;

        [HideInInspector]
        [SerializeField]
        Vector3 xzPosition;

        WayPointsGroup wpGroup;

        public Vector3 XY
        {
            get { return xyPosition; }
        }
        public Vector3 XYZ
        {
            get { return xyzPosition; }
        }
        public Vector3 XZ
        {
            get { return xzPosition; }
        }

        public void SetWayPointGroup (WayPointsGroup wpsg)
        {
            wpGroup = wpsg;
        }

        public void CopyOther (WayPoint other)
        {
            if (other == null) return;

            xyPosition = other.XY;
            xzPosition = other.XZ;
            xyzPosition = other.XYZ;

            Debug.Log(other.XYZ);
            Debug.Log(xyzPosition);
        }

        public Vector3 GetPosition (PositionConstraint constraint = PositionConstraint.XYZ)
        {
            if (wpGroup != null)
            {
                constraint = wpGroup.XYZConstraint;
            }

            if (constraint == PositionConstraint.XY)
            {
                position = xyPosition;
            }
            else if (constraint == PositionConstraint.XZ)
            {
                position = xzPosition;
            }
            else
            {
                position = xyzPosition;
            }

            if (wpGroup != null)
            {
                return wpGroup.transform.position + position;
            }
            else
            {
                return position;
            }
        }

        public void UpdatePosition (Vector3 newPos, PositionConstraint constraint)
        {
            xyPosition.x += newPos.x;
            xzPosition.x += newPos.x;
            xyzPosition.x += newPos.x;

            if (constraint == PositionConstraint.XY)
            {
                xyzPosition.y += newPos.y;
                xyPosition.y += newPos.y;
            }
            else if (constraint == PositionConstraint.XZ)
            {
                xzPosition.z += newPos.z;
                xyzPosition.z += newPos.z;
            }
            else if (constraint == PositionConstraint.XYZ)
            {
                xyzPosition.y += newPos.y;
                xyzPosition.z += newPos.z;

                xyPosition.y += newPos.y;
                xzPosition.z += newPos.z;
            }
        }
    }
}
