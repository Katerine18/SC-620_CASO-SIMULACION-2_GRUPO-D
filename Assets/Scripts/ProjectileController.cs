using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    float speed;

    private Rigidbody2D _rigidbody;

    private float _damage;
    private bool _isPercentage;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    //private void OnEnable()
    //{
        //_rigidbody.velocity = Vector2.right * speed * Time.deltaTime;
    //}
   
    public void Go(float damage, bool isPercentage, bool isFacingRight)
    {
        _damage = damage;
        _isPercentage = isPercentage;
        gameObject.SetActive(true);
        if (isFacingRight)
        {
            _rigidbody.velocity = Vector2.right * speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, -90);
        } else
        {
            _rigidbody.velocity = Vector2.right * -speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {        
                EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();

                if (enemy != null)
                {
                    enemy.Die();
                    Destroy(gameObject);
                }          
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
