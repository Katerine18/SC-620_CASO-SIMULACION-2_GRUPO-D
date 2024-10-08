using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTopDownController : MonoBehaviour
{
    private Animator anim;
    private Transform target;
    public Transform homePos;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxrange;
    [SerializeField]
    private float mixrange;
    void Start()
    {
        anim = GetComponent<Animator>();
        target = FindObjectOfType<PlayerMovementTopDown>().transform;
    }


    void Update()
    {
        if(Vector3.Distance(target.position, transform.position) <= maxrange && Vector3.Distance(target.position, transform.position) >= mixrange)
        {
            FollowPlayer();
        }
        else if(Vector3.Distance(target.position, transform.position) >=maxrange)
        {
            GoHome();
        }
        
    }

    public void FollowPlayer() {

        anim.SetBool("isMoving", true);
        anim.SetFloat("MoveX", (target.position.x - transform.position.x));
        anim.SetFloat("MoveY", (target.position.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }  
    

    public void GoHome()
    {
        anim.SetFloat("MoveX", (homePos.position.x - transform.position.x));
        anim.SetFloat("MoveY", (homePos.position.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, homePos.position, speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, homePos.position) == 0)
        {
            anim.SetBool("isMoving", false);
        }
    }
}
