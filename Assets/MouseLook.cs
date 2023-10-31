using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices.WindowsRuntime;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 30;
    public Transform playerBody;

    float xRotation;
    float yRotation;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = 0;
        float mouseY = 0;


        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            Debug.Log($"x= {Mouse.current.leftButton.ReadValue()}");
            Debug.Log($"y= {Mouse.current.leftButton.ReadValue()}");
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log($"delta x= {Mouse.current.delta.ReadValue().x}");
                Debug.Log($"delta y= {Mouse.current.delta.ReadValue().y}");
                return;
            }

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                mouseX = Mouse.current.delta.ReadValue().x;
                mouseY = Mouse.current.delta.ReadValue().y;
            }
        }

        //if (Gamepad.current != null)
        //{
        //    mouseX = Gamepad.current.rightStick.ReadValue().x;
        //    mouseY = Gamepad.current.rightStick.ReadValue().y;
        //}

        //float mouseX = Input.GetAxis("Mouse X");
        //float mouseY = Input.GetAxis("Mouse Y");

        //if (Touchscreen.current == null || Touchscreen.current.touches.Count == 0)
        //    return;

        //if (EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[0].touchId.ReadValue()))
        //{
        //    if (Touchscreen.current.touches.Count > 1 && Touchscreen.current.touches[1].isInProgress)
        //    {
        //        if (EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[1].touchId.ReadValue()))
        //            return;

        //        Vector2 touchDeltaPosition = Touchscreen.current.touches[1].delta.ReadValue();
        //        mouseX = touchDeltaPosition.x;
        //        mouseY = touchDeltaPosition.y;
        //    }
        //}
        //else
        //{
        //    if (Touchscreen.current.touches.Count > 0 && Touchscreen.current.touches[0].isInProgress)
        //    {
        //        if (EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[0].touchId.ReadValue()))
        //            return;

        //        Vector2 touchDeltaPosition = Touchscreen.current.touches[0].delta.ReadValue();
        //        mouseX = touchDeltaPosition.x;
        //        mouseY = touchDeltaPosition.y;
        //    }

        //}

        mouseX *= mouseSensitivity;
        mouseY *= mouseSensitivity;

        xRotation += mouseX * Time.deltaTime;
        yRotation += mouseY * Time.deltaTime;
        //xRotation = Mathf.Clamp(xRotation, -80, 80);

        transform.localRotation = Quaternion.Euler(yRotation, xRotation, 0);

        // playerBody.Rotate(Vector3.up * mouseX * Time.deltaTime);
    }
}
