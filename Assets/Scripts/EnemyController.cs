using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int ANIMATION_SPEED;

    [SerializeField]
    Transform player;

    [SerializeField]
    float speed;

    [SerializeField]
    float agroRange;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    LayerMask groundLayer;

    Animator _animator;

    float groundCheckRadius;

    Rigidbody2D _rigidbody;

    private Vector2 _originalPosition;
    private bool isReturning = false;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();

        ANIMATION_SPEED = Animator.StringToHash("speed");
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _originalPosition = transform.position;
    }

    private void Update()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        //print("distToPlayer: " + distToPlayer); para revisar distancia

        if (distToPlayer < agroRange)
        {
            AgroPlayer();
        }
        else
        {
            StopAgroPlayer();
            //ReturnOriginalPosition();
        }

        _animator.SetFloat(ANIMATION_SPEED, Mathf.Abs(_rigidbody.velocity.x));
    }

    private void FixedUpdate()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.40F, groundLayer);

        if (!raycastHit)
        {
            _rigidbody.velocity = Vector2.zero;
        }
        else
        {
            if (ShouldAgroPlayer())
            {
                Vector2 direction = (player.position - transform.position).normalized;
                _rigidbody.velocity = new Vector2(direction.x * speed, _rigidbody.velocity.y);

                transform.localScale = new Vector2(Mathf.Sign(direction.x), 1);
            }
        }

    }

    private bool ShouldAgroPlayer()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        return distToPlayer <agroRange;
    }

    private void AgroPlayer()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        

        if (transform.position.x < player.position.x)//si esta a la izquierda del jugador
        {
            _rigidbody.velocity = new Vector2(speed, 0);
            transform.localScale = new Vector2(1, 1); //avatar ve la izquierda

        }
        else if (transform.position.x > player.position.x)  // si esta a la derecha del jugador
        {
            _rigidbody.velocity = new Vector2(-speed, 0);
            transform.localScale = new Vector2(-1, 1);
        }
    }

    private void StopAgroPlayer()
    {
        _rigidbody.velocity = Vector2.zero;//para todo moviemiento.

    }

    private void ReturnOriginalPosition()
    {
        if (!isReturning)
        {
            isReturning = true;
        }
        
        float step = speed * Time.deltaTime;

        if (Mathf.Abs(transform.position.x - _originalPosition.x) > 0.01F)
        {
            if (transform.position.x < _originalPosition.x)
            {
                _rigidbody.velocity = new Vector2(step, _rigidbody.velocity.y);
                transform.localScale = new Vector2(1, 1);
            }
            else if (transform.position.x > _originalPosition.x)
            {
                _rigidbody.velocity = new Vector2(step, _rigidbody.velocity.y);
                transform.localScale = new Vector2(-1, 1);
            }
        } 
        else 
        {
            _rigidbody.velocity = Vector2.zero;
            transform.position = new Vector2(_originalPosition.x, transform.position.y);
            isReturning = false;
        }
    }
}