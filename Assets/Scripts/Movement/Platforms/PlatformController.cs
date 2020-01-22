using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityEngine
{
    //Credit to Sebastian Lague for their Unity 2D Platformer series: https://github.com/SebLague/2DPlatformer-Tutorial
    [RequireComponent(typeof(AudioSource))]
    public class PlatformController : RaycastController
    {
        public LayerMask passengerMask;

        List<PassengerMovement> passengerMovement;
        List<PassengerMovement> pastPassengerMovement;
        Dictionary<Transform, KinematicBody> passengerDictionary = new Dictionary<Transform, KinematicBody>();

        public Vector3 Movement;
        private Vector3 pastMovement;

        public Vector3[] localWaypoints = new Vector3[2];
        [HideInInspector]
        public Vector3[] globalWaypoints;
        [HideInInspector]
        public LineRenderer WaypointLine;
        private Vector3 startPos;

        private ContactFilter2D passengerFilter;

        public AudioClipGroup AudioMove;
        public AudioClipGroup AudioStop;
        protected AudioSource audioSource;
        public float Volume = 1;
        public float StopVolume = 1;

        protected virtual void Awake()
        {
            LevelEvents.OnPlayerRespawn += OnPlayerRespawn;
            if (transform.parent && transform.parent.name == "Content")
            {
                transform.parent.parent.GetComponent<RoomManager>().OnRoomStart += OnPlayerRespawn;
            }
            startPos = transform.position;
        }

        protected virtual void OnDestroy()
        {
            LevelEvents.OnPlayerRespawn -= OnPlayerRespawn;
            if (transform.parent && transform.parent.name == "Content")
            {
                transform.parent.parent.GetComponent<RoomManager>().OnRoomStart -= OnPlayerRespawn;
            }
        }

        public virtual void OnPlayerRespawn()
        {
            transform.position = startPos;
            audioSource.Stop();
        }

        public override void Start()
        {
            base.Start();
            audioSource = GetComponent<AudioSource>();
            passengerFilter.layerMask = passengerMask;
            passengerFilter.useLayerMask = true;
            WaypointLine = GetComponent<LineRenderer>();

            globalWaypoints = new Vector3[localWaypoints.Length];
            Vector3[] linePositions = new Vector3[localWaypoints.Length];
            for (int i = 0; i < globalWaypoints.Length; i++)
            {
                globalWaypoints[i] = localWaypoints[i] + transform.position;
                linePositions[i] = globalWaypoints[i] + new Vector3(0, 0, 1);
            }
            pastPassengerMovement = new List<PassengerMovement>();
            WaypointLine.useWorldSpace = true;
            WaypointLine.positionCount = 2;
            WaypointLine.SetPositions(linePositions);
            /*audioSource.clip = AudioMove.AudioClips[Random.Range(0, AudioMove.AudioClips.Count)];
            audioSource.volume = AudioMove.VolumeMin * Movement.magnitude;
            audioSource.pitch = AudioMove.PitchMin * Movement.magnitude;*/
        }

        protected virtual void FixedUpdate()
        {
            UpdateRaycastOrigins();
            ResetMovedByPlatform();
            pastMovement = Movement;
        }

        protected virtual void Update()
        {
            if (Movement.magnitude != 0 && !audioSource.isPlaying)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.loop = true;
                    audioSource.volume = 0.5f * Volume;
                    audioSource.clip = AudioMove.AudioClips[Random.Range(0, AudioMove.AudioClips.Count)];
                    audioSource.Play();
                }
            }
            if (Movement.magnitude == 0 && pastMovement.magnitude != 0)
            {
                audioSource.loop = false;
                audioSource.volume = 0.5f * pastMovement.magnitude * 5 * Volume * StopVolume;
                audioSource.clip = AudioStop.AudioClips[Random.Range(0, AudioStop.AudioClips.Count)];
                audioSource.Play();
            }
            Vector2 inscreen = Camera.main.WorldToViewportPoint(transform.position);
            audioSource.mute = !(inscreen.x >= 0 && inscreen.x <= 1 && inscreen.y >= 0 && inscreen.y <= 1);
        }
        public void Move(Vector3 velocity)
        {
            CalculatePassengerMovement(velocity);
            MovePassengers(true);
            transform.Translate(velocity);
            Physics2D.SyncTransforms();
            MovePassengers(false);
        }

        public virtual Vector3 GetStoredMovement()
        {
            return Movement / Time.deltaTime;
        }

        void MovePassengers(bool beforeMovePlatform)
        {
            foreach (PassengerMovement passenger in passengerMovement)
            {
                if (!passengerDictionary.ContainsKey(passenger.transform))
                {
                    passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<KinematicBody>());
                }
                if (passenger.moveBeforePlatform == beforeMovePlatform)
                {
                    passengerDictionary[passenger.transform].SetStoredMovementRestTime();
                    passengerDictionary[passenger.transform].detection.Move(passenger.velocity, passenger.standingOnPlatform, passenger.leftCollision, passenger.rightCollision, passenger.belowCollision);
                    passengerDictionary[passenger.transform].TargetStoredMovement = GetStoredMovement();
                    passengerDictionary[passenger.transform].detection.MovedByPlatform = true;
                    pastPassengerMovement.Add(passenger);
                }
            }
        }

        void ResetMovedByPlatform()
        {
            if (pastPassengerMovement.Count > 0)
            {
                foreach (PassengerMovement passenger in pastPassengerMovement)
                {
                    passengerDictionary[passenger.transform].detection.MovedByPlatform = false;
                }
                pastPassengerMovement = new List<PassengerMovement>();
            }
        }

        public Action<Transform> OnMovingTransform;

        void CalculatePassengerMovement(Vector3 velocity)
        {
            HashSet<Transform> movedPassengers = new HashSet<Transform>();
            passengerMovement = new List<PassengerMovement>();
            RaycastHit2D[] results = new RaycastHit2D[2];

            float directionX = Mathf.Sign(velocity.x);
            float directionY = Mathf.Sign(velocity.y);

            // Vertically moving platform
            if (velocity.y != 0)
            {
                float rayLength = Mathf.Abs(velocity.y) + skinWidth;
                for (int i = 0; i < verticalRayCount; i++)
                {
                    Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                    rayOrigin += Vector2.right * (verticalRaySpacing * i);
                    int n = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, passengerFilter, results, rayLength);

                    Debug.DrawRay(rayOrigin, Vector2.up * rayLength * 3, Color.blue);

                    if (n > 0)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            RaycastHit2D hit = results[j];
                            if (!movedPassengers.Contains(hit.transform))
                            {
                                movedPassengers.Add(hit.transform);
                                OnMovingTransform?.Invoke(hit.transform);
                                float pushX = (directionY == 1) ? velocity.x : 0;
                                float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

                                passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true, false, false, directionX == -1));
                            }
                        }
                    }
                }
            }

            // Horizontally moving platform
            if (velocity.x != 0)
            {
                float rayLength = Mathf.Abs(velocity.x) + skinWidth;

                for (int i = 0; i < horizontalRayCount; i++)
                {
                    Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                    rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                    if (hit)
                    {
                        if (!movedPassengers.Contains(hit.transform))
                        {
                            movedPassengers.Add(hit.transform);
                            float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                            float pushY = -skinWidth;

                            passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true, directionX == -1, directionX == 1, false));
                        }
                    }
                }
            }

            // Passenger on top of a horizontally or downward moving platform
            if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
            {
                float rayLength = 2 * skinWidth;
                for (int i = 0; i < verticalRayCount; i++)
                {
                    Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
                    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                    if (hit)
                    {
                        if (!movedPassengers.Contains(hit.transform) && hit.transform.GetComponent<KinematicBody>().CanBeGrounded())
                        {
                            movedPassengers.Add(hit.transform);
                            float pushX = velocity.x;
                            float pushY = velocity.y;

                            //hit.transform.Translate(new Vector3(pushX, pushY));
                            passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
                        }
                    }
                }
            }

            // Passenger to the side of a horizontally or downward moving platform
            if (velocity.x != 0)
            {
                float rayLength = 2 * skinWidth;
                for (int i = 0; i < verticalRayCount; i++)
                {
                    Vector2 rayOrigin = (directionX == 1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                    rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * -directionX, rayLength, passengerMask);

                    if (hit)
                    {
                        if (!movedPassengers.Contains(hit.transform) && hit.transform.GetComponent<KinematicBody>().CanHugWalls())
                        {
                            movedPassengers.Add(hit.transform);
                            float pushX = velocity.x;
                            float pushY = velocity.y;

                            //hit.transform.Translate(new Vector3(pushX, pushY));
                            passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, false));
                        }
                    }
                }
            }
        }

        struct PassengerMovement
        {
            public Transform transform;
            public Vector3 velocity;
            public bool standingOnPlatform;
            public bool moveBeforePlatform;
            public bool leftCollision;
            public bool rightCollision;
            public bool belowCollision;

            public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
            {
                transform = _transform;
                velocity = _velocity;
                standingOnPlatform = _standingOnPlatform;
                moveBeforePlatform = _moveBeforePlatform;
                leftCollision = false;
                rightCollision = false;
                belowCollision = false;
            }

            public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform, bool _leftCollision, bool _rightCollision, bool _belowCollision)
            {
                transform = _transform;
                velocity = _velocity;
                standingOnPlatform = _standingOnPlatform;
                moveBeforePlatform = _moveBeforePlatform;
                leftCollision = _leftCollision;
                rightCollision = _rightCollision;
                belowCollision = _belowCollision;
            }
        }

        private void OnDrawGizmos()
        {
            /*if (localWaypoints != null)
            {
                Gizmos.color = Color.red;
                float size = .3f;

                for(int i = 0; i < localWaypoints.Length; i++)
                {
                    Vector3 globalWaypointPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                    Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                    Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
                }
            }*/
        }
    }
}
