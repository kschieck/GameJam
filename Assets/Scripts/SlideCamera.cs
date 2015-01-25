using UnityEngine;
using System.Collections;

namespace LevelSelect
{
    public class SlideCamera : MonoBehaviour
    {
        //The Background image that defines the bounds
        public SpriteRenderer Background;

        //Bounds for camera zoom (MinZoom < MaxZoom)
        public float MinZoom;           // -1 will set MinZoom to max available
        public float MaxZoom = -1;      // -1 will set MaxZoom to max available

        // Control speed
        public float friction = 0.1f;   // Speed that velocity (linearly) declines
        public float ZoomSpeed = 0.05f; // Speed of camera zoom

        // Variables to enable/disable controls (.enabled enables/disables everything)
        public bool SlideEnabled = true;
        public bool MomentumSlideEnabled = true;
        public bool ZoomEnabled = true;

        private Vector3 velocity;       // Current slide velocity
        private Vector2 LastPos;        // Current finger position
        private int fingerId = -1;      // Current finger Id

        // Camera bounds at max and min zooms
        private Vector2 MinCameraBounds;
        private Vector2 MaxCameraBounds;

        /// <summary>
        /// Move camera to specified (x,y) position then clamp to within bounds
        /// </summary>
        /// <param name="Position"></param>
        public void RequestPosition(Vector2 Position)
        {
            if (MinCameraBounds == MaxCameraBounds && MinCameraBounds == new Vector2(0f, 0f))
                GetPositionBounds();

            transform.position = new Vector3(Position.x, Position.y, transform.position.z);
            ClampCamera();
        }

        private void Start()
        {
            camera.orthographic = true;
            GetPositionBounds();
        }

        private void Update()
        {
            // Don't move if disabled
            if (!enabled) return;

            int touchCount = Input.touchCount;

            // Check touch status and possibly reset velocity and fingerId
            if (touchCount > 1 ||
                (touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                velocity = Vector3.zero;
                fingerId = -1;
            }
            else if (touchCount == 0)
            {
                fingerId = -1;
            }

            // Continue sliding after finger is lifted
            if (touchCount == 0 && SlideEnabled && MomentumSlideEnabled)
                MomentumSlide();
            // Slide if 1 finger down
            else if (touchCount == 1 && SlideEnabled)
                Slide();
            // Zoom if 2 fingers down
            else if (touchCount == 2 && ZoomEnabled)
                Zoom();
            // Don't Clamp if no movement
            //else
            //return;

            // Clamp camera position
            ClampCamera();
        }

        /// <summary>
        /// Continue the slide from momentum
        /// </summary>
        private void MomentumSlide()
        {
            transform.position -= velocity;
            velocity *= (1f - friction);
        }

        /// <summary>
        /// Move object based on deltaTouch
        /// </summary>
        private void Slide()
        {
            // Get touch data
            Touch touch = Input.GetTouch(0);

            if (touch.fingerId != fingerId)
            {
                fingerId = touch.fingerId;
                LastPos = touch.position;
                return;
            }

            // Get difference in world points
            Vector3 LastWorldPoint = camera.ScreenToWorldPoint(LastPos);
            Vector3 WorldPoint = camera.ScreenToWorldPoint(touch.position);
            Vector3 positionChange = (WorldPoint - LastWorldPoint);

            // Set velocity
            if (positionChange.magnitude > 0f)
                velocity = positionChange;

            // Move camera to new position
            transform.position -= positionChange;

            LastPos = touch.position;
        }

        /// <summary>
        /// Alter camera's orthographic size, then clamp
        /// </summary>
        private void Zoom()
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Change Size
            camera.orthographicSize =
                Mathf.Clamp(camera.orthographicSize + deltaMagnitudeDiff * ZoomSpeed,
                    MinZoom, MaxZoom);
        }

        /// <summary>
        /// Set clamp bounds
        /// </summary>
        private void GetPositionBounds()
        {
            //Quit if there is no Background Sprite
            if (!Background)
            {
                Debug.LogError("Camera's Background Sprite is null", gameObject);
                return;
            }
            // Quit if MinZoom or MaxZoom invalid
            if (MaxZoom == 0 || MinZoom == 0)
            {
                Debug.LogError("Min and Max Zoom must be non-zero", gameObject);
                gameObject.SetActive(false);
                return;
            }
            // Quit if MaxZoom < MinZoom
            if (MaxZoom <= MinZoom && MaxZoom > 0 && MinZoom > 0)
            {
                Debug.LogError("Max Zoom must be larger than Min Zoom");
                gameObject.SetActive(false);
                return;
            }

            // Get background Sprite center
            Vector3 center = Background.bounds.center;

            // Set default Bounds (No moving from center)
            MinCameraBounds = new Vector3(center.x, center.y, transform.position.z);
            MaxCameraBounds = MinCameraBounds;

            // Get Resolution settings
            float aspectRatio = (float)Screen.width / (float)Screen.height;
            if (aspectRatio > 1f) aspectRatio = 1f / aspectRatio;
            float HalfWidth = Background.bounds.size.x / 2f;
            float HalfHeight = Background.bounds.size.y / 2f;
            float maxAllowedZoom = Mathf.Min(HalfHeight, HalfWidth * aspectRatio);

            // Get Minimum bounds
            if (MinZoom < 0 || MinZoom > maxAllowedZoom) MinZoom = maxAllowedZoom;
            MinCameraBounds = new Vector3(
                HalfWidth - (MinZoom / aspectRatio),
                HalfHeight - MinZoom);

            // Get Maximum bounds
            if (MaxZoom < 0 || MaxZoom > maxAllowedZoom) MaxZoom = maxAllowedZoom;
            MaxCameraBounds = new Vector3(
                HalfWidth - (MaxZoom / aspectRatio),
                HalfHeight - (MaxZoom));

            // Clamp Camera
            ClampCamera();
        }

        /// <summary>
        /// Clamp camera based on current Zoom and Background bounds
        /// </summary>
        private void ClampCamera()
        {
            // Get Current Position
            Vector3 camPos = transform.position;

            // Clamp camera's orthographic size
            camera.orthographicSize =
                Mathf.Clamp(camera.orthographicSize, MinZoom, MaxZoom);

            // Get zoom scale
            float scale = 1f;
            if (MaxZoom != MinZoom)
            {
                scale = (camera.orthographicSize - MinZoom) / (MaxZoom - MinZoom);
            }

            // Get Maximum allowed distances from centre
            Vector2 Max = new Vector3(
                MinCameraBounds.x + (MaxCameraBounds.x - MinCameraBounds.x) * scale,
                MinCameraBounds.y + (MaxCameraBounds.y - MinCameraBounds.y) * scale);

            // Get image center
            Vector3 center = Background.bounds.center;

            // Clamp to centre
            camPos.x = Mathf.Clamp(camPos.x, center.x - Max.x, center.x + Max.x);
            camPos.y = Mathf.Clamp(camPos.y, center.y - Max.y, center.y + Max.y);

            // Set clamped position
            transform.position = camPos;
        }

    }
}