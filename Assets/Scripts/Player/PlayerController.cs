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
    private float _gravityMultiplier;

    private CharacterController _characterController;
    private Rigidbody _rigidbody;
    private Vector2 _cameraRotation;

    private const float GRAVITY = -9.81f;
    [SerializeField]
    private float _fallVelocity = 3.0f;
    private Vector3 _direction;


    public InputAction moveAction;
    public InputAction lookAction;
    public InputAction jumpAction;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();

        _direction = transform.position;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Update orientation first, then move. Otherwise move orientation will lag
        // behind by one frame.
        ApplyGravity();
        Look(lookAction.ReadValue<Vector2>());
        Move(moveAction.ReadValue<Vector3>());
    }

    private void OnEnable()
    {
        lookAction.Enable();
        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
    }

    private void Move(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;
        var scaledMoveSpeed = _speed * Time.deltaTime;
        // For simplicity's sake, we just keep movement in a single plane here. Rotate
        // direction according to world Y rotation of player.
        _direction = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.z);

        //if (!_characterController.isGrounded)
        //    direction += Physics.gravity;

        _direction = Vector3.ClampMagnitude(_direction, _speed) * scaledMoveSpeed;

        _characterController.Move(_direction);
    }

    private void Look(Vector2 rotation)
    {
        if (rotation.sqrMagnitude < 0.01)
            return;
        var scaledRotateSpeed = _cameraSensetivitySpeed * Time.deltaTime;
        _cameraRotation.y += rotation.x * scaledRotateSpeed;
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x - rotation.y * scaledRotateSpeed, -70, 70);

        transform.localEulerAngles = _cameraRotation;
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _fallVelocity < 0.0f)
            _fallVelocity = -1.0f;
        else
            _fallVelocity += GRAVITY * _gravityMultiplier * Time.deltaTime;

        _characterController.Move(new Vector3(0, _fallVelocity, 0));
    }
}
