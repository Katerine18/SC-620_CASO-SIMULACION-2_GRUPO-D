using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementTopDown : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Animator playerAnimator;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        playerAnimator.SetFloat("Horizontal", moveX);
        playerAnimator.SetFloat("Vertical", moveY);
        playerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
}
