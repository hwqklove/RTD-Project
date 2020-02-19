using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WayPointsFamily
{

    /// <summary>
    /// WayPoint Traveler is that allow the traveler to follow the desinated route(WayPointGroup)
    /// Decide on the traveler's speed and what to do last(loof, lerp, and forward)
    /// </summary>
    public class WayPointTraveler : MonoBehaviour
    {
        GameObject tempobj;

        public WayPointsGroup Waypoints = null;
        public PositionConstraint XYZConstraint = PositionConstraint.XYZ;
        public bool AutoStart = false;
        public float MoveSpeed;
        public float LookAtSpeed;
        public int StartIndex = 0;
        public bool AutoPositionAtStart = true;
        public TravelDirection StartTravelDirection = TravelDirection.FORWARD;
        public EndPointBehavior EndReachedBehavior = EndPointBehavior.LOOP;
        public MoveType StartingMovementType = MoveType.LERP;

        delegate bool MovementFunction();
        MovementFunction moveFunc = null;

        int positionIndex = -1;
        List<WayPoint> waypointsList;

        Vector3 nextPosition;
        Vector3 startPosition;
        Vector3 destinationPosition;

        float distanceToNextWaypoint;
        float distanceTraveled = 0;
        float timeTraveled = 0;

        int travelIndexCounter = 1;

        bool isMoving = false;

        Vector3 positionOriginal;
        Quaternion rotationOriginal;
        float moveSpeedOriginal = 0;
        float lookAtSpeedOriginal = 0;

        public bool IsMoving
        {
            get { return isMoving; }
        }

        public void ResetTraveler()
        {
            transform.position = positionOriginal;
            transform.rotation = rotationOriginal;

            MoveSpeed = moveSpeedOriginal;
            LookAtSpeed = lookAtSpeedOriginal;

            StartAtIndex(StartIndex, AutoPositionAtStart);
            SetNextPosition();
            travelIndexCounter = StartTravelDirection == TravelDirection.REVERSE ? -1 : 1;

            if (StartingMovementType == MoveType.LERP)
            {
                moveFunc = MoveLerpSimple;
            }
            else if (StartingMovementType == MoveType.FORWARD_TRANSLATE)
            {
                moveFunc = MoveForwardToNext;
            }
        }

        void Start()
        {
            moveSpeedOriginal = MoveSpeed;
            lookAtSpeedOriginal = LookAtSpeed;

            positionOriginal = transform.position;
            rotationOriginal = transform.rotation;

            ResetTraveler();

            Move(AutoStart);
        }

        public void Move(bool tf)
        {
            isMoving = tf;
        }

        private void Awake()
        {
            tempobj = GameObject.Find("WayPoint");
            Waypoints = tempobj.GetComponent<WayPointsGroup>();

            if (Waypoints != null)
            {
                waypointsList = Waypoints.waypoints;
            }
        }

        void Update ()
        {
            if (isMoving == true && moveFunc != null)
            {
                bool arrivedAtDestination = false;

                arrivedAtDestination = moveFunc();

                if (arrivedAtDestination == true)
                {
                    SetNextPosition();
                }
            }
        }

        public void SetWaypointsGroup (WayPointsGroup newGroup)
        {
            Waypoints = newGroup;
            waypointsList = null;

            if (newGroup != null)
            {
                waypointsList = newGroup.waypoints;
            }
        }

        void StartAtIndex (int idx, bool autoUpdatePosition = true)
        {
            if (StartTravelDirection == TravelDirection.REVERSE)
            {
                idx = waypointsList.Count - idx - 1;
            }

            idx = Mathf.Clamp(idx, 0, waypointsList.Count - 1);
            positionIndex = idx - 1;

            if (autoUpdatePosition)
            {
                transform.position = waypointsList[idx].GetPosition();

                if (LookAtSpeed > 0)
                {
                    if (StartTravelDirection == TravelDirection.REVERSE)
                    {
                        idx -= 1;
                        if (idx < 0)
                        {
                            idx = waypointsList.Count - 1;
                        }
                    }
                    else
                    {
                        idx += 1;
                        if (idx >= waypointsList.Count)
                        {
                            idx = 0;
                        }
                    }
                }
            }
        }

        void SetNextPosition ()
        {
            int posCount = waypointsList.Count;

            if (posCount > 0)
            {
                if ((positionIndex == 0 && travelIndexCounter < 0) ||(positionIndex == posCount - 1 && travelIndexCounter > 0))
                {
                    if (EndReachedBehavior == EndPointBehavior.STOP)
                    {
                        isMoving = true;
                        Destroy(gameObject);
                        return;
                    }
                    else if (EndReachedBehavior == EndPointBehavior.PINGPONG)
                    {
                        travelIndexCounter = -travelIndexCounter;
                    }
                    else if (EndReachedBehavior == EndPointBehavior.LOOP)
                    {

                    }
                }

                positionIndex += travelIndexCounter;

                if (positionIndex >= posCount)
                {
                    positionIndex = 0;
                }
                else if (positionIndex < 0)
                {
                    positionIndex = posCount - 1;
                }

                nextPosition = waypointsList[positionIndex].GetPosition();

                if (XYZConstraint == PositionConstraint.XY)
                {
                    nextPosition.z = transform.position.z;
                }
                else if (XYZConstraint == PositionConstraint.XZ)
                {
                    nextPosition.y = transform.position.y;
                }

                ResetMovementValues();
            }
        }

        void ResetMovementValues ()
        {
            startPosition = transform.position;
            destinationPosition = nextPosition;
            distanceToNextWaypoint = Vector3.Distance(startPosition, destinationPosition);
            distanceTraveled = 0;
            timeTraveled = 0;
        }

        bool MoveLerpSimple ()
        {
            if (MoveSpeed < 0)
            {
                MoveSpeed = 0;
            }

            timeTraveled += Time.deltaTime;
            distanceTraveled += Time.deltaTime * MoveSpeed;
            float fracAmount = distanceTraveled / distanceToNextWaypoint;
            transform.position = Vector3.Lerp(startPosition, destinationPosition, fracAmount);
            UpdateLookAtRotation();
            return fracAmount >= 1;
        }

        bool MoveForwardToNext ()
        {
            if (MoveSpeed < 0)
            {
                MoveSpeed = 0;
            }

            float rate = Time.deltaTime * MoveSpeed;
            float distance = Vector3.Distance(transform.position, destinationPosition);

            if (distance < rate)
            {
                transform.position = destinationPosition;
                return true;
            }

            if (LookAtSpeed <= 0)
            {
                LookAtSpeed = float.MaxValue;
            }

            UpdateLookAtRotation();

            Vector3 moveDir = Vector3.forward;

            if (XYZConstraint == PositionConstraint.XY)
            {
                moveDir = Vector3.up;
            }

            transform.Translate(moveDir * rate);

            return false;
        }

        void UpdateLookAtRotation ()
        {
            if (LookAtSpeed <= 0)
            {
                return;
            }

            float step = LookAtSpeed * Time.deltaTime;
            Vector3 targetDir = nextPosition - transform.position;

            if (XYZConstraint == PositionConstraint.XY)
            {
                float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;
                Quaternion qt = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qt, step);
            }
            else if (XYZConstraint == PositionConstraint.XZ)
            {
                float angle = Mathf.Atan2(targetDir.x, targetDir.y) * Mathf.Rad2Deg;
                Quaternion qt = Quaternion.AngleAxis(angle, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qt, step);
            }
            else
            {
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
    }
}