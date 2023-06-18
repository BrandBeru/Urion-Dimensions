using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Movement : CombatMode
{
    //move
    private float horizontal;
    [SerializeField] private float jumpingPower = 2f;
    private bool doubleJump;
    private bool isFacingRight = true;

    //Skills
    private float speed;

    //Dash
    private bool canDash = true;
    private bool isDashing = false;
    [SerializeField] private float dashingPower = 4f;
    [SerializeField] private float dashingTime = 0.2f;
    private float dashingCooldown = 0.5f;

    //Wall
    private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingCounter;
    [SerializeField] private float wallJumpingTime = 0.2f;
    [SerializeField] private float wallJumpingDuration = 0.4f;

    //Buttons
    [SerializeField] private ButtonsController up;
    [SerializeField] private ButtonsController down;
    [SerializeField] private ButtonsController left;
    [SerializeField] private ButtonsController right;
    [SerializeField] private ButtonsController attackBtn;
    [SerializeField] private ButtonsController jumpBtn;
    [SerializeField] private ButtonsController dashBtn;


    public bool canMove;

    //Ladder
    private bool isLadder;
    private bool isClimbing;
    private float vertical;

    public AudioClip[] effects;
    public SoundController soundController;

    private float defaultGravity;

    [SerializeField] private Vector2 wallJumpingPower = new Vector2(2f, 4f);

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCkeck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private TrailRenderer tr;

    public SpriteRenderer weapon;

    private Vector2 initialPosition;

    private void Awake()
    {
        canMove = true;
        initialPosition = transform.position;
        defaultGravity = rb.gravityScale;
        jumpLeft = 1;
    }
    public void UpdateSkills(float speed)
    {
        this.speed = speed;
    }
    // Update is called once per frame
    void Update()
    {
        if (isDashing)
            return;

        if (canMove)
        {
            if (left.isPressed)
                horizontal = -1;
            else if (right.isPressed)
                horizontal = 1;
            else if (up.isPressed)
                vertical = 1;
            else if (down.isPressed)
                vertical = -1;
            else
            {
                horizontal = Input.GetAxisRaw("Horizontal");
                vertical = Input.GetAxisRaw("Vertical");
            }

            if (isLadder && Mathf.Abs(vertical) > 0f)
                isClimbing = true;
            if (Input.GetButtonDown("Fire1") || attackBtn.isPressed)
                Attack();
            if ((Input.GetButtonDown("Dash") && canDash) || (dashBtn.isPressed && canDash))
                StartDash();
            WallSlide();
            WallJump();
            Jump();
            if (!isWallJumping)
                Flip();

            if (Input.GetButtonDown("Horizontal") && IsGrounded())
                soundController.Play(effects[0]);
            if (Input.GetButtonUp("Horizontal"))
                soundController.Stop();

            anim.SetInteger("Walk", (horizontal > 0 || horizontal < 0) ? 1 : 0);
            anim.SetBool("Jump", !IsGrounded());
            anim.SetBool("IsClimbing", isClimbing);
            anim.SetFloat("Vertical", vertical);
        }
        else
        {
            vertical = 0;
            horizontal = 0;
        }

    }
    private int jumpLeft;
    public int maxJumps = 3;
    private void Jump()
    {
        if((jumpLeft > 0 && Input.GetButtonDown("Jump")) || (jumpLeft > 0 && jumpBtn.isPressed))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jumpLeft--;
        }
        if (IsGrounded())
            jumpLeft = maxJumps;
        
    }
    public void UpdateWeapon(Sprite sprite)
    {
        weapon.sprite = sprite;
        powerHit = GameManager.instance.weaponSelected.powerAmount;
    }
    public void StartDash()
    {
        soundController.Play(effects[1]);
        StartCoroutine(Dash());
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.05f, groundLayer);
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCkeck.position, 0.05f, wallLayer);
    }
    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            anim.SetBool("IsWalling", isWallSliding);
            anim.SetBool("Jump", false);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
            anim.SetBool("IsWalling", isWallSliding);
        }
    }
    public void SetCanMove(bool canMoveSet)
    {
        canMove = canMoveSet;
    }
    private void FixedUpdate()
    {
        if (!isWallJumping && !isDashing)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        if (isDashing)
            return;

        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(horizontal * speed, vertical * speed);
            
        }
        else if (isDeath)
        {
            StartCoroutine(RestartGame());
        }
        else
            rb.gravityScale = defaultGravity;

    }

    public IEnumerator RestartGame()
    {
        anim.SetBool("IsDeath", isDeath);
        rb.velocity = new Vector2(0, vertical * speed);
        rb.gravityScale = 0f;
        StartDied();
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
            wallJumpingCounter -= Time.deltaTime;

        if((Input.GetButtonDown("Jump") || jumpBtn.isPressed) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if(transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
        else if (collision.CompareTag("Finish"))
            isDeath = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
    public float Speed()
    {
        return speed;
    }
    public float Horizontal()
    {
        return horizontal;
    }

    public void changeToMiniGame()
    {
        SceneManager.LoadScene(1);
    }
}
