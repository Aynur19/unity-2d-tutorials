using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float moveSpeed = 5;

    private float hMovement = 0;


    void Update()
    {
        rb.linearVelocity = new Vector2(hMovement * moveSpeed, rb.linearVelocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        hMovement = context.ReadValue<Vector2>().x;
    }
}
