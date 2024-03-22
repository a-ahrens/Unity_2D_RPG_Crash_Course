using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Dash Info")]
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashCooldown;
    private float _dashTime;
    private float _dashCooldownTimer;

    [Header("Attack Info")]
    [SerializeField] private float _comboTime = 0.3f;
    [SerializeField] private float _comboTimeWindow;
    private bool _isAttacking;
    private int _comboCounter;

    private float xInput;
    private int facingDirection = 1;
    private bool facingRight = true;

    [Header("Collision Info")]
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _whatIsGround;
    private bool _isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        CheckInput();
        Movement();

        _dashTime -= Time.deltaTime;
        _dashCooldownTimer -= Time.deltaTime;
        _comboTimeWindow -= Time.deltaTime;

        CollisionChecks();

        FlipController();
        AnimatorControllers();
    }

    public void AttackOver()
    {
        _isAttacking = false;
        _comboCounter++;
        
        if(_comboCounter > 2)
        {
            _comboCounter = 0;
        }

    }

    private void CollisionChecks()
    {
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, _whatIsGround);
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashAbility();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAttack();
        }

    }

    private void StartAttack()
    {
        if(!_isGrounded)
        {
            return;
        }

        if (_comboTimeWindow < 0)
        {
            _comboCounter = 0;
        }

        _isAttacking = true;
        _comboTimeWindow = _comboTime;
    }

    private void DashAbility()
    {
        if (_dashCooldownTimer <= 0 && !_isAttacking)
        {
            _dashCooldownTimer = _dashCooldown;
            _dashTime = _dashDuration;
        }
    }

    private void Movement()
    {
        if(_isAttacking)
        {
            rb.velocity = new Vector2(0, 0);
        }

        else if (_dashTime > 0)
        {
            /* Setting the y velocity to 0 creates a nice darting effect.
             * Player doesn't lose elevation if performed in the air.
             * If 0 is replaced with rb.velocity.y, then you achieve a parabolic curve type path.
             */
            rb.velocity = new Vector2(facingDirection * _dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }

    }

    private void Jump()
    {
        if (_isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void AnimatorControllers()
    {
        bool isMoving = rb.velocity.x != 0 ? true : false;

        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", _isGrounded);
        anim.SetBool("isDashing", _dashTime > 0);
        anim.SetBool("isAttacking", _isAttacking);
        anim.SetInteger("comboCounter", _comboCounter);
    }

    private void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController()
    {
        if ((rb.velocity.x > 0 && !facingRight) || (rb.velocity.x < 0 && facingRight))
        {
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - _groundCheckDistance));
    }
}
