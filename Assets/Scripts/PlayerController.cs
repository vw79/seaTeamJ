using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool canMove = true;
    [SerializeField] private float playerSpeed = 2.0f;

    private InputAction moveAction;
    private Vector2 movementInput;

    private Vector3 originalScale;
    private Rigidbody2D rb;

    private void Awake()
    {
        moveAction = new InputAction(type: InputActionType.Value, binding: "<Gamepad>/leftStick");
        moveAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        moveAction.Enable();

        originalScale = transform.localScale;

        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        movementInput = moveAction.ReadValue<Vector2>();

        if (canMove && rb != null)
        {
            if (movementInput != Vector2.zero)
            {
                Vector2 newPosition = rb.position + movementInput * playerSpeed * Time.fixedDeltaTime;
                rb.MovePosition(newPosition);

                if (movementInput.x < 0 && transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
                }
                else if (movementInput.x > 0 && transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
                }
            }
        }
    }

    private void OnDestroy()
    {
        moveAction.Disable();
        moveAction.Dispose();
    }
}