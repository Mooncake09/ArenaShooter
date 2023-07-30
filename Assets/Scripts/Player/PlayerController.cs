using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _cameraSensetivitySpeed;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _gravityMultiplier = 1f;

    private CharacterController _characterController;
    private Rigidbody _rigidbody;
    private Vector2 _cameraRotation;

    private float _mouseXInput;
    private float _mouseYInput;

    private Vector3 _direction;

    //public InputAction lookAction;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Update orientation first, then move. Otherwise move orientation will lag
        // behind by one frame.
        //Look(lookAction.ReadValue<Vector2>());
        ApplyMove();
        Look(new Vector2(_mouseXInput, _mouseYInput));
        
    }

    private void Look(Vector2 rotation)
    {
        Debug.Log(rotation);
        if (rotation.sqrMagnitude < 0.01)
            return;

        var scaledRotateSpeed = _cameraSensetivitySpeed * Time.deltaTime;
        _cameraRotation.y += rotation.x * scaledRotateSpeed;
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x - rotation.y * scaledRotateSpeed, -70, 70);

        transform.localEulerAngles = _cameraRotation;
    }

    public void Move(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();

        var movement = new Vector3(input.x, 0, input.y) * _speed;
        movement = Vector3.ClampMagnitude(movement, _speed);
        movement += Physics.gravity * _gravityMultiplier;
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);

        _direction = movement;
    }

    public void MouseX(InputAction.CallbackContext context) => _mouseXInput = context.ReadValue<float>();
    public void MouseY(InputAction.CallbackContext context) => _mouseYInput = context.ReadValue<float>();


    private void ApplyMove() 
    {
        _characterController.Move(_direction);
    } 
}
