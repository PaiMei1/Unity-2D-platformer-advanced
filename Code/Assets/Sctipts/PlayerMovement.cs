// ...existing code...
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpPower = 12f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D body;
    private Animator anim;
    private AudioSource audioSource;

    [SerializeField] private float coyoteTime = 0.15f;
    private float coyoteCounter;

    [SerializeField] private int extraJumps = 1;
    private int jumpCounter;

    [SerializeField] private float wallJumpX = 8f;
    [SerializeField] private float wallJumpY = 12f;

    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    [SerializeField] private float wallJumpCooldownTime = 0.2f;
    private float horizontalInput;

    [SerializeField] private AudioClip jumpSound;

    private Vector3 startScale;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider2D>();
        startScale = transform.localScale; // remember original scale so flipping doesn't change size
    }

    private void Update()
    {
        wallJumpCooldown -= Time.deltaTime;

        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Flip based on input only when not in wall-jump cooldown
        if (horizontalInput > 0.01f && wallJumpCooldown <= 0)
            transform.localScale = new Vector3(Mathf.Abs(startScale.x), startScale.y, startScale.z);
        else if (horizontalInput < -0.01f && wallJumpCooldown <= 0)
            transform.localScale = new Vector3(-Mathf.Abs(startScale.x), startScale.y, startScale.z);

        bool grounded = IsGrounded();
        bool touchingWall = OnWall();

        if (anim != null)
        {
            anim.SetBool("Run", Mathf.Abs(horizontalInput) > 0.01f);
            anim.SetBool("Grounded", grounded);
        }

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        // Short hop (cut jump when releasing)
        if (Input.GetKeyUp(KeyCode.Space) && body.linearVelocity.y > 0)
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y * 0.5f);

        // Wall slide / stick
        if (touchingWall && !grounded)
        {
            body.gravityScale = 0;
            body.linearVelocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7f;

            if (wallJumpCooldown <= 0)
                body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            if (grounded)
            {
                coyoteCounter = coyoteTime;
                jumpCounter = extraJumps;
            }
            else
            {
                coyoteCounter -= Time.deltaTime;
            }
        }
    }

    private void Jump()
    {
        bool grounded = IsGrounded();
        bool touchingWall = OnWall();

        // If not in coyote, not on wall and no extra jumps -> can't jump
        if (coyoteCounter <= 0f && !touchingWall && jumpCounter <= 0)
            return;

        // Wall jump has priority when touching wall (and not grounded)
        if (touchingWall && !grounded)
        {
            WallJump();
            return;
        }

        // Normal/coyote/air jump
        if (grounded || coyoteCounter > 0f)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            coyoteCounter = 0f;
        }
        else if (jumpCounter > 0)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            jumpCounter--;
        }

        if (audioSource != null && jumpSound != null)
            audioSource.PlayOneShot(jumpSound);
    }

    private void WallJump()
    {
        // Push away from the wall: if facing right (scale.x > 0) push left, and vice versa
        float pushDirection = -Mathf.Sign(transform.localScale.x);
        body.linearVelocity = new Vector2(pushDirection * wallJumpX, wallJumpY);
        wallJumpCooldown = wallJumpCooldownTime;
        coyoteCounter = 0f;
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool OnWall()
    {
        // Cast toward the facing direction
        float dir = Mathf.Sign(transform.localScale.x);
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, new Vector2(dir, 0f), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return Mathf.Abs(horizontalInput) < 0.01f && IsGrounded() && !OnWall();
    }
}
// ...existing code...