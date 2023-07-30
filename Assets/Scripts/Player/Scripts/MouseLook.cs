using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    private Transform playerBody;

    private PlayerControllerActions actions;

    [SerializeField]
    private float mouseSensetivity = 100f;
    private float xRotation = 0f;

    private Vector2 mouseLook;

    private void Awake()
    {
        playerBody = transform.parent;
        actions = new PlayerControllerActions();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Look();
    }

    private void OnEnable()
    {
        actions.Player.Enable();
    }

    private void OnDisable()
    {
        actions.Player.Disable();
    }

    private void Look()
    {
        mouseLook = actions.Player.Look.ReadValue<Vector2>();

        float mouseX = mouseLook.x * mouseSensetivity * Time.deltaTime;
        float mouseY = mouseLook.y * mouseSensetivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
