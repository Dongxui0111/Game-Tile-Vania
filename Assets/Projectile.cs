using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 25f;
    float projectileDirection = 1f;

    Rigidbody2D myRigidBody;
    BoxCollider2D myBoxCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeLife();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidBody.velocity = new Vector2(projectileSpeed * projectileDirection, 0f);

        if (myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            Destroy(gameObject);
        }
    }

    public void SetProjectileDirection(float directionToSet)
    {
        projectileDirection = directionToSet;
    }
}
