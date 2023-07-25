using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _cameraSensetivitySpeed;

    private CharacterController _characterController;
    private Vector2 _cameraRotation;

    

    public InputAction moveAction;
    public InputAction lookAction;

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
        Look(lookAction.ReadValue<Vector2>());
        Move(moveAction.ReadValue<Vector3>());
    }

    private void OnEnable()
    {
        lookAction.Enable();
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
    }

    private void Move(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;
        var scaledMoveSpeed = _speed * Time.deltaTime;
        // For simplicity's sake, we just keep movement in a single plane here. Rotate
        // direction according to world Y rotation of player.
        var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.z);

        if (!_characterController.isGrounded)
            move += Physics.gravity;

        move = Vector3.ClampMagnitude(move, _speed) * scaledMoveSpeed;



        _characterController.Move(move);
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
}
