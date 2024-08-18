using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    Transform player;

    [SerializeField]
    float speed;

    [SerializeField]
    float agroRange;

    Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        //print("distToPlayer: " + distToPlayer);

        if(distToPlayer < agroRange)
        {
            AgroPlayer();
        }
        else
        {
            StopAgroPlayer();
        }
    }

    private void AgroPlayer()
    {
        if(transform.position.x < player.position.x)//si esta a la izquierda del jugador
        {
            _rigidbody.velocity = new Vector2(speed, 0);
            transform.localScale = new Vector2(1, 1); //avatar ve la izquierda

        }
        else if(transform.position.x > player.position.x)  // si esta a la derecha del jugador
        {
            _rigidbody.velocity = new Vector2(-speed, 0);
            transform.localScale = new Vector2(-1, 1); 
        }
    }

    private void StopAgroPlayer()
    {
        _rigidbody.velocity = Vector2.zero;//para todo moviemiento.

    }
}

