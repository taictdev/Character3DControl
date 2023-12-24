using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 10;
    public Transform playerBody;

    private float xRotation;
    private float yRotation;
    private bool lockY = true;

    private void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#else
        HandleMouseInput();
#endif
        mouseX *= mouseSensitivity;
        mouseY *= mouseSensitivity;

        xRotation += mouseX * Time.deltaTime;
        if (lockY == false)
        {
            yRotation += mouseY * Time.deltaTime;
        }

        transform.localRotation = Quaternion.Euler(-yRotation, -xRotation, 0);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "Lock Y"))
        {
            lockY = !lockY;
        }
    }

    private float mouseX;
    private float mouseY;

    private void HandleMouseInput()
    {
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log($"delta x= {Mouse.current.delta.ReadValue().x}");
                Debug.Log($"delta y= {Mouse.current.delta.ReadValue().y}");
                return;
            }

            mouseX = Mouse.current.delta.ReadValue().x;
            mouseY = Mouse.current.delta.ReadValue().y;
        }
    }

    private void HandleTouchInput()
    {
        if (Touchscreen.current == null || Touchscreen.current.touches.Count == 0)
            return;

        int touchIndex = 0;

        if (Touchscreen.current.touches.Count > 1 && Touchscreen.current.touches[1].isInProgress)
        {
            touchIndex = 1;
        }

        if (EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[touchIndex].touchId.ReadValue()))
            return;

        Vector2 touchDeltaPosition = Touchscreen.current.touches[touchIndex].delta.ReadValue();
        mouseX = touchDeltaPosition.x;
        mouseY = touchDeltaPosition.y;
    }
}