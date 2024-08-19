using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController2D : MonoBehaviour
{
    private int ANIMATION_SPEED;
    private int ANIMATION_FORCE;
    private int ANIMATION_FALL;
    private int ANIMATION_WALK;
    private int ANIMATION_RUN;
    private int ANIMATION_MELEE;
    private int ANIMATION_RELEASE;
    private int ANIMATION_DIE;
    private int ANIMATION_FIRE;

    [Header("Movement")]
    [SerializeField]
    float walkSpeed;

    [SerializeField]
    float jumpForce;

    [SerializeField]
    float gravityMultiplier;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    Vector2 groundCheckSize;

    [SerializeField]
    LayerMask groundMask;

    [SerializeField]
    bool isFacingRight;

    [Header("Attack")]
    [SerializeField]
    Transform meleePoint;

    [SerializeField]
    float meleeRadius;

    [SerializeField]
    LayerMask attackMask;

    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    Transform projectilePoint;

    [SerializeField]
    float projectileLifeTime;

    [SerializeField]
    string fireSoundSFX;

    [SerializeField]
    string meleeSoundSFX;

    [Header("Die")]
    [SerializeField]
    float dieTime;

    Rigidbody2D _rigidbody;
    Animator _animator;

    float _inputX;
    float _gravityY;
    float _velocityY;

    bool _isGrounded;
    bool _isJumpPressed;
    bool _isJumping;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

        _gravityY = Physics2D.gravity.y;

        ANIMATION_SPEED = Animator.StringToHash("speed");
        ANIMATION_FORCE = Animator.StringToHash("force");
        ANIMATION_FALL = Animator.StringToHash("fall");
        ANIMATION_WALK = Animator.StringToHash("walk");
        ANIMATION_RUN = Animator.StringToHash("run");
        ANIMATION_MELEE = Animator.StringToHash("melee");
        ANIMATION_RELEASE = Animator.StringToHash("release");
        ANIMATION_DIE = Animator.StringToHash("die");
        ANIMATION_FIRE = Animator.StringToHash("fire");
    }

    private void Start()
    {
        HandleGrounded();
    }

    private void Update()
    {
        HandleGravity();
        HandleInputMove();
    }

    private void FixedUpdate()
    {
        HandleJump();
        HandleRotate();
        HandleMove();
    }

    private void HandleGrounded()
    {
        _isGrounded = IsGrounded();
        if (!_isGrounded)
        {
            StartCoroutine(WaitForGroundedCoroutine());
        }
    }

    private void HandleGravity()
    {
        if (_isGrounded)
        {
            if (_velocityY < -1.0F)
            {
                _velocityY = -1.0F;
            }

            HandleInputJump();
        }
    }

    private void HandleInputJump()
    {
        _isJumpPressed = Input.GetButton("Jump");
    }

    private void HandleInputMove()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
    }

    private void HandleJump()
    {
        if (_isJumpPressed)
        {
            _isJumpPressed = false;
            _isGrounded = false;
            _isJumping = true;

            _velocityY = jumpForce;

            _animator.SetTrigger(ANIMATION_FORCE);

            StartCoroutine(WaitForGroundedCoroutine());
        }
        else if (!_isGrounded)
        {
            _velocityY += _gravityY * gravityMultiplier * Time.fixedDeltaTime;
            if (!_isJumping)
            {
                _animator.SetTrigger(ANIMATION_FALL);
            }
        }
        else if (_isGrounded)
        {
            if (_velocityY >= 0.0F)
            {
                _velocityY = -1.0F;
            }
            else
            {
                HandleGrounded();
            }

            _isJumping = false;
        }
    }

    private void HandleMove()
    {
        float speed = _inputX != 0.0F ? 1.0F : 0.0F;
        float animatorSpeed = _animator.GetFloat(ANIMATION_SPEED);

        if (speed != animatorSpeed)
        {
            _animator.SetFloat(ANIMATION_SPEED, speed);
        }

        Vector2 velocity = new Vector2(_inputX, 0.0F) * walkSpeed * Time.fixedDeltaTime;
        velocity.y = _velocityY;

        _rigidbody.velocity = velocity;
    }

    private void HandleRotate()
{
    if (_inputX == 0.0F)
    {
        return;
    }

    bool facingRight = _inputX > 0.0F;
    if (isFacingRight != facingRight)
    {
        // Guarda la posición actual(igual no sirvio :(
        Vector3 originalPosition = transform.position;
        
        // Realiza la rotación
        isFacingRight = facingRight;
        transform.Rotate(0.0F, 180.0F, 0.0F);

        // Restablece la posición para evitar el efecto raro este
        transform.position = originalPosition;
    }
}


    private bool IsGrounded()
    {
        Collider2D collider2D =
            Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0.0F, groundMask);
        return collider2D != null;
    }

    private IEnumerator WaitForGroundedCoroutine()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(() => IsGrounded());
        _isGrounded = true;
    }

    public void Melee()
    {
        _animator.SetTrigger(ANIMATION_MELEE);
        SoundManager.Instance.PlaySFX(meleeSoundSFX);
    }

    public void Melee(float damage, bool isPercentage)
    {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(meleePoint.position, meleeRadius, attackMask);
        

        foreach (Collider2D collider in colliders)
        {
            DamageableController controller = collider.GetComponent<DamageableController>();
            if (controller == null)
            {
                continue;
            }

            controller.TakeDamage(damage, isPercentage);
        }
    }

    public void Fire()
    {
        SoundManager.Instance.PlaySFX(fireSoundSFX);
        _animator.SetTrigger(ANIMATION_FIRE);
    }

    public void Fire(float damage, bool isPercentage)
    {
        GameObject projectile =
            Instantiate(projectilePrefab, projectilePoint.position, transform.rotation);
        ProjectileController controller = projectile.GetComponent<ProjectileController>();
            controller.Go(damage, isPercentage, isFacingRight);
        

        Destroy(projectile, projectileLifeTime);
    }

    private DestroyController _killEnemy;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(_rigidbody.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();

                if (enemy != null)
                {
                    enemy.Die();
                }
                //Destroy(collision.gameObject);
                //_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce * 0.5f);
            } else {
                Die();
            }
            
        }
    }

    public void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        _animator.SetTrigger(ANIMATION_DIE);
        yield return new WaitForSeconds(dieTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDrawGizmos() //Dibuja un cuadrado rojo donde esta el Ground Check
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}
