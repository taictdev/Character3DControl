using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MoveImage : MonoBehaviour
{
    private Vector2 lastTouch1;
    private Vector2 lastTouch2;

    public float moveSpeed = 0.1f;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Check if there are at least two active touches
        if (Touchscreen.current != null && Touchscreen.current.touches.Count >= 2)
        {
            // Get the first two active touches
            TouchControl touch1 = Touchscreen.current.touches[0];
            TouchControl touch2 = Touchscreen.current.touches[1];

            // Check if the touches are in phase where movement can occur
            if ((touch1.phase.value == UnityEngine.InputSystem.TouchPhase.Moved || touch1.phase.value == UnityEngine.InputSystem.TouchPhase.Began) &&
                (touch2.phase.value == UnityEngine.InputSystem.TouchPhase.Moved || touch2.phase.value == UnityEngine.InputSystem.TouchPhase.Began))
            {
                // Calculate the midpoint between two touches
                Vector2 midpoint = (touch2.position.ReadValue() - touch1.position.ReadValue()) / 2f;

                // Get the movement vector from the last frame
                Vector2 moveVector = midpoint - (lastTouch1 + lastTouch2) / 2f;

                // Apply the movement to the object
                rectTransform.anchoredPosition += new Vector2(moveVector.x, moveVector.y) * moveSpeed * Time.deltaTime;

                // Update the last touch positions for the next frame
                lastTouch1 = touch1.position.ReadValue();
                lastTouch2 = touch2.position.ReadValue();

                // Exit the function to avoid processing the single touch case
                return;
            }
        }

        // Single touch case
        if (Touchscreen.current != null && Touchscreen.current.touches.Count == 1)
        {
            TouchControl touch = Touchscreen.current.touches[0];

            if (touch.phase.value == UnityEngine.InputSystem.TouchPhase.Moved || touch.phase.value == UnityEngine.InputSystem.TouchPhase.Began)
            {
                Vector2 currentTouchPosition = touch.position.ReadValue();
                
                // Calculate the movement vector
                Vector2 moveVector = currentTouchPosition - lastTouch1;

                // Apply the movement to the RectTransform's anchoredPosition
                rectTransform.anchoredPosition += new Vector2(moveVector.x, moveVector.y) * moveSpeed * Time.deltaTime;

                // Update the last touch position for the next frame
                lastTouch1 = currentTouchPosition;
            }
            else if (touch.phase.value == UnityEngine.InputSystem.TouchPhase.Began)
            {
                lastTouch1 = touch.position.ReadValue();
            }
        }
    }
}