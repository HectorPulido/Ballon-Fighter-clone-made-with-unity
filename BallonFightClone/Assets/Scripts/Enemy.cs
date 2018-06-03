using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float period;
    bool ballon = true;
    Transform player;

    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sr;
    BoxCollider2D col;
    IEnumerator Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        anim.SetBool("Ballon", ballon);

        while (true)
        {
            if (ballon)
            {
                if (Random.value > 0.5f)
                {
                    tentativeVelocity = Random.onUnitSphere;
                    tentativeVelocity.y = tentativeVelocity.y < 0 ? 0 : tentativeVelocity.y;
                }
                else
                {
                    tentativeVelocity = player.position - transform.position;
                }
                tentativeVelocity.Normalize();

                sr.flipX = tentativeVelocity.x > 0;
                rb.AddForce(tentativeVelocity * speed, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(period);
        }
    }
    Vector2 tentativeVelocity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collision.collider.transform.position.y > transform.position.y)
            {
                ballon = false;
                anim.SetBool("Ballon", ballon);
                rb.gravityScale = 1;
                return;
            }

            if (!ballon)
            {
                col.enabled = false;
                Global.singleton.AddScore();
                Destroy(gameObject, 3);
            }

        }
    }

}
