using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform groundPosition;
    public Vector2 groundSize;
    public float speed;
    public float jumpSpeed;

    public int ballon = 3;

    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sr;
    BoxCollider2D col;

    bool CheckGround
    {
        get
        {
            var ground = Physics2D.BoxCast(groundPosition.position, groundSize, 0, Vector2.zero);

            if (ground.collider == null)
                return false;
            
            return true;
        }
    } 

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();

        anim.SetFloat("Ballons", ballon);
        rb.freezeRotation = true;
    }

    float horizontal = 0;
    bool jumpRequest;
    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        var g = CheckGround;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequest = true;
        }

        if (horizontal == 0)
        {
            if(g)
                anim.speed = 0;
        }
        else
        {
            sr.flipX = horizontal > 0;
            anim.speed = 1;
        }

        anim.SetBool("Ground", g);
    }
    private void FixedUpdate()
    {
        if (jumpRequest)
        {
            jumpRequest = false;
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }

        rb.velocity = new Vector3(horizontal * speed, rb.velocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(groundPosition.position, groundSize);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            Die();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (collision.collider.transform.position.y > transform.position.y)
            {
                ballon--;
                anim.SetFloat("Ballons", ballon);

                if (ballon <= 0)
                {
                    Die();
                }
            }
        }
    }

    void Die()
    {
        col.enabled = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1;
        Global.singleton.GameOver();
        this.enabled = false;
    }
}
