using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    private CharacterController _characterController;
    private Rigidbody _rb;

    private Vector3 _directionMovement;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        ApplyMove();
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
}
