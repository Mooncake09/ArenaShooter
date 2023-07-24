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
    [SerializeField]
    private float _gravity = -9.8f;

    private CharacterController _characterController;
    private Vector2 _cameraRotation;

    

    public InputAction moveAction;
    public InputAction lookAction;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        //_rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //if (_rb != null)
        //    _rb.freezeRotation = true;
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
        var scaledSpeed = _speed * Time.deltaTime;
        var newDirection = new Vector3(direction.x, 0, direction.z);
        newDirection = Vector3.ClampMagnitude(newDirection, scaledSpeed);
        //newDirection.y = _gravity * Time.deltaTime;
        _characterController.Move(_characterController.transform.TransformDirection(newDirection));
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
}
