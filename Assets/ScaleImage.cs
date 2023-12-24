using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ScaleImage : MonoBehaviour
{
    private Vector2 initialTouch1;
    private Vector2 initialTouch2;
    private float initialDistance;

    public float scaleSpeed = 0.2f;

    void Update()
    {
        // Check if there are at least two active touches
        if (Touchscreen.current == null || Touchscreen.current.touches.Count < 2)
            return;

        // Get the first two active touches
        TouchControl touch1 = Touchscreen.current.touches[0];
        TouchControl touch2 = Touchscreen.current.touches[1];

        // Check if the touches are in phase where scaling can occur
        if (touch1.phase.value == UnityEngine.InputSystem.TouchPhase.Began || touch2.phase.value == UnityEngine.InputSystem.TouchPhase.Began)
        {
            initialTouch1 = touch1.position.ReadValue();
            initialTouch2 = touch2.position.ReadValue();
            initialDistance = Vector2.Distance(initialTouch1, initialTouch2);
        }
        else if (touch1.phase.value == UnityEngine.InputSystem.TouchPhase.Moved || touch2.phase.value == UnityEngine.InputSystem.TouchPhase.Moved)
        {
            Vector2 currentTouch1 = touch1.position.ReadValue();
            Vector2 currentTouch2 = touch2.position.ReadValue();
            float currentDistance = Vector2.Distance(currentTouch1, currentTouch2);

            // Calculate the scale factor
            float scaleFactor = currentDistance / initialDistance;

            // Apply the scale to the object
            transform.localScale *= scaleFactor * (1 + scaleSpeed * Time.deltaTime);

            // Update initial values for the next frame
            initialTouch1 = currentTouch1;
            initialTouch2 = currentTouch2;
            initialDistance = currentDistance;
        }
    }
}
