using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
#if UNITY_ANDROID || UNITY_IOS
public class MoveRectImage : MonoBehaviour
{
    private Vector2 lastTouchPosition;
    private RectTransform rectTransform;

    public float moveSpeed = 0.1f;
    private float timeCount = 0;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Check if there is at least one active touch
        if (Touchscreen.current == null || Touchscreen.current.touches.Count == 0)
        {
            timeCount = 0;
            return;
        }

        timeCount += Time.deltaTime;

        if (timeCount < 0.25f)
        {
            return;
        }

        // Get the first active touch
        TouchControl touch = Touchscreen.current.touches[0];

        // Check if the touch is in phase where movement can occur
        if (touch.phase.value == UnityEngine.InputSystem.TouchPhase.Moved)
        {
            Vector2 currentTouchPosition = touch.position.ReadValue();

            // Calculate the movement vector
            Vector2 moveVector = currentTouchPosition - lastTouchPosition;

            // Apply the movement to the RectTransform's anchoredPosition
            // rectTransform.anchoredPosition += moveVector * moveSpeed * Time.deltaTime;

            // Apply the movement to the RectTransform's anchoredPosition
            Vector2 newPosition = rectTransform.anchoredPosition + moveVector * moveSpeed * Time.deltaTime;

            // Limit the position to be within the screen boundaries
            newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
            newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

            rectTransform.anchoredPosition = newPosition;

            // Update the last touch position for the next frame
            lastTouchPosition = currentTouchPosition;
        }
        else if (touch.phase.value == UnityEngine.InputSystem.TouchPhase.Began)
        {
            // Set the initial touch position
            lastTouchPosition = touch.position.ReadValue();
        }
    }
}
#elif UNITY_WEBGL || UNITY_EDITOR
public class MoveRectImage : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    private Vector2 lastMousePosition;
    private RectTransform rectTransform;

    private float timeCount = 0;
    private const float TimeReTouch = 0.25f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        timeCount += Time.deltaTime;

        if (timeCount < TimeReTouch)
        {
            return;
        }

        // Check if the left mouse button is held down
        if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePosition = Input.mousePosition;

            // Calculate the movement vector
            Vector2 moveVector = currentMousePosition - lastMousePosition;

            // Apply the movement to the RectTransform's anchoredPosition
            Vector2 newPosition = rectTransform.anchoredPosition + moveVector * moveSpeed * Time.deltaTime;

            // Limit the position to be within the screen boundaries
            newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
            newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

            rectTransform.anchoredPosition = newPosition;

            // Update the last mouse position for the next frame
            lastMousePosition = currentMousePosition;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            // Set the initial mouse position
            lastMousePosition = Input.mousePosition;
        }
    }
}
#endif