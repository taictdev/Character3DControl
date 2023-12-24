using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
#if UNITY_ANDROID || UNITY_IOS
public class ZoomInOut : MonoBehaviour
{
    private Vector2 initialTouch1;
    private Vector2 initialTouch2;
    private float initialDistance;

    public float zoomSpeed = 0.2f;
    private float timeCount = 0;

    private void Update()
    {
        // Check if there are exactly two active touches
        if (Touchscreen.current == null || Touchscreen.current.touches.Count < 2)
        {
            timeCount = 0;
            return;
        }
        timeCount += Time.deltaTime;

        if (timeCount < 0.25f)
        {
            return;
        }
        // Get the first two active touches
        TouchControl touch1 = Touchscreen.current.touches[0];
        TouchControl touch2 = Touchscreen.current.touches[1];

        // Check if the touches are in phase where zooming can occur
        if ((touch1.phase.value == UnityEngine.InputSystem.TouchPhase.Began || touch1.phase.value == UnityEngine.InputSystem.TouchPhase.Moved) &&
            (touch2.phase.value == UnityEngine.InputSystem.TouchPhase.Began || touch2.phase.value == UnityEngine.InputSystem.TouchPhase.Moved))
        {
            Vector2 currentTouch1 = touch1.position.ReadValue();
            Vector2 currentTouch2 = touch2.position.ReadValue();
            float currentDistance = Vector2.Distance(currentTouch1, currentTouch2);

            // Check if this is the beginning of the zoom (store initial values)
            if (touch1.phase.value == UnityEngine.InputSystem.TouchPhase.Began || touch2.phase.value == UnityEngine.InputSystem.TouchPhase.Began)
            {
                initialTouch1 = currentTouch1;
                initialTouch2 = currentTouch2;
                initialDistance = currentDistance;
            }

            // Calculate the zoom factor
            float zoomFactor = currentDistance / initialDistance;

            // Calculate the midpoint between two touches
            Vector2 midpoint = (currentTouch1 + currentTouch2) / 2f;

            // Get the initial midpoint position
            Vector2 initialMidpoint = (initialTouch1 + initialTouch2) / 2f;

            // Calculate the translation vector
            Vector2 translation = initialMidpoint - midpoint;

            // Determine the direction of the pinch (expand or contract)
            float pinchDirection = Vector2.SignedAngle(initialTouch2 - initialTouch1, currentTouch2 - currentTouch1);

            // Apply the zoom and translation to the object based on pinch direction
            if (pinchDirection > 0)
            {
                // Zoom out
                transform.localScale *= zoomFactor * (1 - zoomSpeed * Time.deltaTime);
            }
            else
            {
                // Zoom in
                transform.localScale *= zoomFactor * (1 + zoomSpeed * Time.deltaTime);
            }

            // Update initial values for the next frame
            initialTouch1 = currentTouch1;
            initialTouch2 = currentTouch2;
            initialDistance = currentDistance;
        }
    }
}
#elif UNITY_WEBGL || UNITY_EDITOR

public class ZoomInOut : MonoBehaviour
{
    public float zoomSpeed = 0.5f; // Điều chỉnh tốc độ zoom

    void Update()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        if (scrollWheel != 0)
        {
            // Lấy vị trí chuột trên màn hình
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            // Thực hiện zoom
            transform.localScale += new Vector3(scrollWheel * zoomSpeed, scrollWheel * zoomSpeed, 0);

            // Giữ cho vị trí tâm không thay đổi
            Vector3 offset = transform.position - worldPos;
            transform.position += offset - (transform.position - worldPos);
        }
    }
}

#endif