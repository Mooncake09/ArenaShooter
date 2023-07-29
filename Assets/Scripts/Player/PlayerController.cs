using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private float _cameraSensetive = 3.0f;

    private CharacterController _characterController;
    private Rigidbody _rb;

    private Vector3 _directionMovement;
    private Vector3 _rotation;

    public InputAction _lookAction;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Look();
        ApplyMove();
    }

    private void OnEnable()
    {
        _lookAction.Enable();
    }

    private void OnDisable()
    {
        _lookAction.Disable();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"Jump! {context.phase}");
            _rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }

    private void ApplyMove() => _characterController.Move(_directionMovement * _speed * Time.deltaTime);

    public void Move(InputAction.CallbackContext context)
    {
        var currentPosition = context.ReadValue<Vector2>();
        _directionMovement = new Vector3(currentPosition.x, 0, currentPosition.y);
    }

    public void Look()
    {
        var cameraPostition = _lookAction.ReadValue<Vector2>();
        if (cameraPostition.sqrMagnitude < 0.01)
            return;
        var scaledRotateSpeed = _cameraSensetive * Time.deltaTime;

        _rotation.y += cameraPostition.x * scaledRotateSpeed;
        _rotation.x = Mathf.Clamp(_rotation.x - cameraPostition.y * scaledRotateSpeed, -90, 90);

        transform.rotation = Quaternion.Euler(_rotation);
    }
}
