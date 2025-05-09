using NUnit.Framework.Interfaces;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;


    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 5;
    private float hMovement = 0;



    [Header("Jumping")]
    [SerializeField]
    private float jumpPower = 10f;

    [SerializeField]
    private int maxJumps = 2;
    private int jumpsRemaining = 0;


    [Header("Ground check")]
    [SerializeField]
    private Transform groundCheckPos;

    [SerializeField]
    private Vector2 groundCheckSize = new(0.5f, 0.5f);

    [SerializeField]
    private LayerMask groundLayer;


    [Header("Gravity")]
    [SerializeField] private float baseGravity = 2f;
    [SerializeField] private float maxFallSpeed = 18f;
    [SerializeField] private float fallSpeedMultiplier = 2f;



    //void Update()
    //{
    //    rb.linearVelocity = new Vector2(hMovement * moveSpeed, rb.linearVelocity.y);
    //}

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(hMovement * moveSpeed, rb.linearVelocityY);
        GroundCheck();
        Gravity();
    }

    public void Move(InputAction.CallbackContext context)
    {
        hMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining <= 0)
        {
            return;
        }

        if (context.performed)
        {
            // Hold down jump button = full height
            Jump(isLight: false);
        }
        else if (context.canceled)
        {
            // Light tap of jump button = half the height
            Jump(isLight: true);
        }
    }

    private void Jump(bool isLight)
    {
        Debug.Log(isLight ? "canceled" : "performed");
        var height = isLight ? rb.linearVelocity.y * 0.5f : jumpPower;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, height);

        jumpsRemaining--;
        Debug.Log($"jumpsRemaining: {jumpsRemaining}");
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumpsRemaining = maxJumps;
        }
    }


    private void Gravity()
    {
        if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Max(rb.linearVelocityY, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}
