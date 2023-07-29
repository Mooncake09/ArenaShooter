using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;
    private Rigidbody _rb;
    private PlayerControllerActions _playerControllerActions;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _rb = GetComponent<Rigidbody>();

        _playerControllerActions = new PlayerControllerActions();
        //playerControllerActions.Enable(); //Enable every map (like Player at this example)
        _playerControllerActions.Player.Enable(); //Enable only Player map
        _playerControllerActions.Player.Jump.performed += Jump;
        _playerControllerActions.Player.Movement.performed += Movement_performed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        _playerControllerActions.Player.Movement.ReadValue<Vector2>();
    }

    private void Movement_performed(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        var inputVector = context.ReadValue<Vector2>();
        var speed = 5f;

        _rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed * Time.deltaTime, ForceMode.Force);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"Jump! {context.phase}");
            _rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }
}
