using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float vertical;

    public float damage;
    public float resistence;

    public float speed;
    public float jumpingPower;
    public bool jump;
    private bool isGrounded;

    [SerializeField] private Rigidbody2D rb;

    //Buttons
    [SerializeField] private ButtonsController up;
    [SerializeField] private ButtonsController down;
    [SerializeField] private ButtonsController left;
    [SerializeField] private ButtonsController right;

    private void Update()
    {
        
        if (left.isPressed)
            horizontal = -1;
        else if (right.isPressed)
            horizontal = 1;
        else
            horizontal = Input.GetAxisRaw("Horizontal");
        if (up.isPressed)
            vertical = 1;
        else if (down.isPressed)
            vertical = -1;
        else
            vertical = Input.GetAxisRaw("Vertical");

    }
    private void FixedUpdate()
    {
        if (!jump)
            rb.velocity = new Vector2(horizontal * speed, vertical * speed);
        else
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
                rb.AddForce(new Vector2(rb.velocity.x, vertical * jumpingPower), ForceMode2D.Impulse);
            }
            else
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Projectile")
        {
            MiniGamesManager.instance.TakeDamage(true);
        }
        if (collision.collider.tag == "Down")
            isGrounded = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Down")
            isGrounded = false;
    }
}
