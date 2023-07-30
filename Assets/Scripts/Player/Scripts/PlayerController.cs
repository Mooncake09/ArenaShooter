using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 6f;

    private Vector3 velocity;
    private float gravity = -9.81f;
    private Vector2 move;
    private float jumpHeight = 2.4f;

    public Transform ground;
    public float distanceToGround = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

    private PlayerControllerActions actions;
    private CharacterController characterController;

    private void Awake()
    {
        actions = new PlayerControllerActions();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Gravity();
        PlayerMovement();
        Jump();
    }

    private void OnEnable()
    {
        actions.Player.Movement.Enable();
        actions.Player.Jump.Enable();
    }

    private void OnDisable()
    {
        actions.Player.Movement.Disable();
        actions.Player.Jump.Disable();
    }

    private void PlayerMovement()
    {
        move = actions.Player.Movement.ReadValue<Vector2>();

        Vector3 movement = (move.y * transform.forward) + (move.x * transform.right);
        characterController.Move(movement * moveSpeed * Time.deltaTime);
    }

    private void Gravity()
    {
        isGrounded = Physics.CheckSphere(ground.position, distanceToGround, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (actions.Player.Jump.triggered)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
