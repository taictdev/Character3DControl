using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ZoomToMidpoint : MonoBehaviour
{
    private Vector2 initialTouch1;
    private Vector2 initialTouch2;
    private float initialDistance;

    public float zoomSpeed = 0.5f;

    void Update()
    {
        // Check if there are exactly two active touches
        if (Touchscreen.current == null || Touchscreen.current.touches.Count < 2)
            return;

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

            // Apply the zoom and translation to the object
            transform.localScale *= zoomFactor * (1 + zoomFactor * Time.deltaTime);
            transform.Translate(new Vector3(translation.x, translation.y, 0) * zoomSpeed * Time.deltaTime, Space.World);
        }
    }
}
