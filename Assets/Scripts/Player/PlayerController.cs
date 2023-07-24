using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private CharacterController _characterController;

    public InputAction moveAction;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move(moveAction.ReadValue<Vector3>());
    }

    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    private void Move(Vector3 direction)
    {
        var scaledSpeed = _speed * Time.deltaTime;
        var newDirection = new Vector3(direction.x * scaledSpeed, direction.y * scaledSpeed, direction.z * scaledSpeed);

        Debug.Log($"newDir: {newDirection}");
        _characterController.Move(newDirection);
    }
}
