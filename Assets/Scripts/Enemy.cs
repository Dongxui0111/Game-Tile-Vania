using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int life = 2;
    [SerializeField] bool toKickPlayer = false;
    [SerializeField] float playerKickVelocityY = 25f;

    [SerializeField] AudioClip deathSFX;

    bool canBeHurt = true;

    CapsuleCollider2D myBodyCollider;

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Player"))) { return; }

        // this enemy will die
        if (otherCollider.transform.position.y > transform.position.y)
        {
            Hurt();

            if (toKickPlayer)
            { 
                Rigidbody2D otherRigidBody = otherCollider.GetComponent<Rigidbody2D>();
                //otherRigidBody.velocity = new Vector2(0f, playerKickVelocityY);
                //otherCollider.GetComponent<Player>().SetInvincible(true);   //TODO
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        canBeHurt = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        myBodyCollider = GetComponent<CapsuleCollider2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hurt()
    {
        if (!canBeHurt) { return; }

        TakeLife();

        GetComponent<SpriteRenderer>().color = Color.grey;
        canBeHurt = false;
    }

    public void TakeLife()
    {
        if (life - 1 > 0)
        {
            life--;
        }
        else
        {
            AudioSource.PlayClipAtPoint(deathSFX, FindObjectOfType<Player>().transform.position);
            Destroy(gameObject);
        }
    }
}
