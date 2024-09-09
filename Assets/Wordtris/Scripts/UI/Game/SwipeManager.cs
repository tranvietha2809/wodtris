using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Wordtris.Scripts.GamePlay
{
    public class SwipeManager : MonoBehaviour
    {
        #region Inspector Variables

        [Tooltip("Min swipe distance (inches) to register as swipe")]
        [SerializeField]
        float minSwipeLength = 0.5f;  // Minimum distance in inches a swipe must cover to be recognized.

        [Tooltip("If true, a swipe is counted when the min swipe length is reached. If false, a swipe is counted when the touch/click ends.")]
        [SerializeField]
        bool triggerSwipeAtMinLength = false;  // Determines if a swipe is detected immediately upon reaching the min length or at touch release.

        [Tooltip("Whether to detect eight or four cardinal directions")]
        [SerializeField]
        bool useEightDirections = false;  // Flag to switch between detecting 4 or 8 swipe directions.

        #endregion

        const float EightDirAngle = 0.906f;  // Dot product threshold for detecting 8 directions.
        const float FourDirAngle = 0.5f;  // Dot product threshold for detecting 4 directions.
        const float DefaultDpi = 72f;  // Default DPI value if the device doesn't provide one.
        const float DpcmFactor = 2.54f;  // Factor to convert DPI to DPCM (dots per centimeter).

        // Dictionary mapping swipe directions to corresponding vector directions.
        static readonly Dictionary<Swipe, Vector2> CardinalDirections =
            new Dictionary<Swipe, Vector2>()
            {
                {Swipe.Up, CardinalDirection.Up},
                {Swipe.Down, CardinalDirection.Down},
                {Swipe.Right, CardinalDirection.Right},
                {Swipe.Left, CardinalDirection.Left},
                {Swipe.UpRight, CardinalDirection.UpRight},
                {Swipe.UpLeft, CardinalDirection.UpLeft},
                {Swipe.DownRight, CardinalDirection.DownRight},
                {Swipe.DownLeft, CardinalDirection.DownLeft}
            };

        // Delegate and event to notify when a swipe is detected.
        public delegate void OnSwipeDetectedHandler(Swipe swipeDirection, Vector2 swipeVelocity);
        private static OnSwipeDetectedHandler _onSwipeDetected;

        public static event OnSwipeDetectedHandler OnSwipeDetected
        {
            add
            {
                _onSwipeDetected += value;
                autoDetectSwipes = true;  // Enables automatic swipe detection when listeners are added.
            }
            remove { _onSwipeDetected -= value; }
        }

        public static Vector2 swipeVelocity;  // Velocity of the detected swipe.

        private static float dpcm;  // Dots per centimeter, calculated from screen DPI.
        private static float swipeStartTime;  // Timestamp when the swipe started.
        private static float swipeEndTime;  // Timestamp when the swipe ended.
        private static bool autoDetectSwipes;  // Flag to control automatic swipe detection.
        private static bool swipeEnded;  // Flag indicating if the current swipe has ended.
        private static Swipe swipeDirection;  // Direction of the detected swipe.
        private static Vector2 firstPressPos;  // Position where the swipe started.
        private static Vector2 secondPressPos;  // Position where the swipe ended.
        private static SwipeManager instance;  // Singleton instance of the SwipeManager.

        void Awake()
        {
            instance = this;
            float dpi = (Screen.dpi == 0) ? DefaultDpi : Screen.dpi;  // Use default DPI if Screen.dpi is not available.
            dpcm = dpi / DpcmFactor;  // Convert DPI to DPCM.
        }

        void Update()
        {
            if (autoDetectSwipes)
            {
                DetectSwipe();  // Continuously check for swipes if auto-detection is enabled.
            }
        }

        /// <summary>
        /// Attempts to detect the current swipe direction.
        /// Should be called over multiple frames in an Update-like loop.
        /// </summary>
        static void DetectSwipe()
        {
            if (GetTouchInput() || GetMouseInput())
            {
                // If the swipe has already ended, wait for a new swipe.
                if (swipeEnded)
                {
                    return;
                }

                Vector2 currentSwipe = secondPressPos - firstPressPos;  // Calculate the swipe vector.
                float swipeCm = currentSwipe.magnitude / dpcm;  // Convert the swipe length to centimeters.

                // If the swipe is shorter than the minimum length, it doesn't count as a swipe.
                if (swipeCm < instance.minSwipeLength)
                {
                    if (!instance.triggerSwipeAtMinLength)
                    {
                        if (Application.isEditor)
                        {
                            // Debug.Log("[SwipeManager] Swipe was not long enough.");
                        }

                        swipeDirection = Swipe.None;  // Reset swipe direction if not long enough.
                    }

                    return;
                }

                swipeEndTime = Time.time;  // Record the time when the swipe ended.
                swipeVelocity = currentSwipe * (swipeEndTime - swipeStartTime);  // Calculate the swipe velocity.
                swipeDirection = GetSwipeDirByTouch(currentSwipe);  // Determine the swipe direction.
                swipeEnded = true;  // Mark the swipe as ended.

                _onSwipeDetected?.Invoke(swipeDirection, swipeVelocity);  // Trigger the swipe detected event.
            }
            else
            {
                swipeDirection = Swipe.None;  // Reset the swipe direction if no swipe detected.
            }
        }

        public static bool IsSwiping()
        {
            return swipeDirection != Swipe.None;  // Check if a swipe is currently being detected.
        }

        // Methods to check if the current swipe is in a specific direction.
        public static bool IsSwipingRight() => IsSwipingDirection(Swipe.Right);
        public static bool IsSwipingLeft() => IsSwipingDirection(Swipe.Left);
        public static bool IsSwipingUp() => IsSwipingDirection(Swipe.Up);
        public static bool IsSwipingDown() => IsSwipingDirection(Swipe.Down);
        public static bool IsSwipingDownLeft() => IsSwipingDirection(Swipe.DownLeft);
        public static bool IsSwipingDownRight() => IsSwipingDirection(Swipe.DownRight);
        public static bool IsSwipingUpLeft() => IsSwipingDirection(Swipe.UpLeft);
        public static bool IsSwipingUpRight() => IsSwipingDirection(Swipe.UpRight);

        #region Helper Functions

        // Gets touch input and determines the swipe state.
        static bool GetTouchInput()
        {
            if (Input.touches.Length > 0)
            {
                Touch t = Input.GetTouch(0);

                // Swipe/Touch started
                if (t.phase == TouchPhase.Began)
                {
                    firstPressPos = t.position;  // Record the position where the swipe started.
                    swipeStartTime = Time.time;  // Record the start time of the swipe.
                    swipeEnded = false;  // Reset swipe ended flag.
                }
                // Swipe/Touch ended
                else if (t.phase == TouchPhase.Ended)
                {
                    secondPressPos = t.position;  // Record the position where the swipe ended.
                    return true;
                }
                // Still swiping/touching
                else
                {
                    if (instance.triggerSwipeAtMinLength)  // Check if we should detect swipe mid-touch.
                    {
                        return true;
                    }
                }
            }

            return false;  // Return false if no swipe was detected.
        }

        // Gets mouse input and determines the swipe state.
        static bool GetMouseInput()
        {
            // Swipe/Click started
            if (Input.GetMouseButtonDown(0))
            {
                firstPressPos = (Vector2)Input.mousePosition;  // Record the position where the swipe started.
                swipeStartTime = Time.time;  // Record the start time of the swipe.
                swipeEnded = false;  // Reset swipe ended flag.
            }
            // Swipe/Click ended
            else if (Input.GetMouseButtonUp(0))
            {
                secondPressPos = (Vector2)Input.mousePosition;  // Record the position where the swipe ended.
                return true;
            }
            // Still swiping/clicking
            else
            {
                if (instance.triggerSwipeAtMinLength)  // Check if we should detect swipe mid-click.
                {
                    return true;
                }
            }

            return false;  // Return false if no swipe was detected.
        }

        // Checks if the swipe direction is close enough to a cardinal direction.
        static bool IsDirection(Vector2 direction, Vector2 cardinalDirection)
        {
            var angle = instance.useEightDirections ? EightDirAngle : FourDirAngle;  // Choose the angle based on the number of directions.
            return Vector2.Dot(direction, cardinalDirection) > angle;  // Compare the swipe direction with the cardinal direction.
        }

        // Determines the direction of the swipe based on the swipe vector.
        static Swipe GetSwipeDirByTouch(Vector2 currentSwipe)
        {
            currentSwipe.Normalize();  // Normalize the swipe vector.
            var swipeDir = CardinalDirections.FirstOrDefault(dir => IsDirection(currentSwipe, dir.Value));  // Find the closest matching swipe direction.
            return swipeDir.Key;  // Return the detected swipe direction.
        }

        // Checks if the current swipe matches a specific direction.
        static bool IsSwipingDirection(Swipe swipeDir)
        {
            DetectSwipe();  // Ensure a swipe is detected before checking.
            return swipeDirection == swipeDir;  // Return true if the swipe direction matches the given direction.
        }

        #endregion

        // Class to define the cardinal directions as static vectors.
        public class CardinalDirection
        {
            public static readonly Vector2 Up = new Vector2(0, 1);
            public static readonly Vector2 Down = new Vector2(0, -1);
            public static readonly Vector2 Right = new Vector2(1, 0);
            public static readonly Vector2 Left = new Vector2(-1, 0);
            public static readonly Vector2 UpRight = new Vector2(1, 1);
            public static readonly Vector2 UpLeft = new Vector2(-1, 1);
            public static readonly Vector2 DownRight = new Vector2(1, -1);
            public static readonly Vector2 DownLeft = new Vector2(-1, -1);
        }

        // Enum to represent possible swipe directions.
        public enum Swipe
        {
            None,
            Up,
            Down,
            Left,
            Right,
            UpLeft,
            UpRight,
            DownLeft,
            DownRight
        };
    }
}
