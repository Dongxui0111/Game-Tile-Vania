using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);

    [SerializeField] GameObject gunPoint;
    [SerializeField] Projectile projectilePrefab;

    [Header("Sound FX")]
    [SerializeField] AudioClip footstepSFX;
    [SerializeField] AudioClip jumpingSFX;
    [SerializeField] AudioClip shootingSFX;
    [SerializeField] AudioClip dyingSFX;
    [SerializeField] AudioClip climbingSFX;

    [Header("Visual FX")]
    [SerializeField] ParticleSystem dustParticles;
    [SerializeField] ParticleSystem bloodParticles;

    // state
    bool isAlive = true;
    bool isShooting = false;
    bool isInvincible = false;

    // cached component references
    Rigidbody2D myRigidBody;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    Animator myAnimator;
    float gravityScaleAtStart;

    float steps = 0f;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();

        gravityScaleAtStart = myRigidBody.gravityScale;
    }

    void Update()
    {
        if (!isAlive) { return; }

        if (!isShooting)
        {
            Run();
            Jump();
            Climb();
            FlipSprite();
        }

        Shoot();
        Die();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            foreach(ContactPoint2D contactPoint in collision.contacts)
            {
                if (contactPoint.normal.y >= 0.9f)
                {
                    Vector2 jumpVelocityToAdd = new Vector2(myRigidBody.velocity.x, jumpSpeed);
                    myRigidBody.velocity += jumpVelocityToAdd;
                    enemy.Hurt();
                    break;
                }
                else
                {
                    Die();
                }   
            }
        }
    }
    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);

        if (playerHasHorizontalSpeed && Mathf.Abs(controlThrow) > 0) { 
            steps += 1 * Time.deltaTime;
            if (steps > 0.25f)
            {
                PlaySFX(footstepSFX);
                steps = 0;
            }
        }
    }

    private void Jump()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing"))) { return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(myRigidBody.velocity.x, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;

            PlaySFX(jumpingSFX);
        }
    }

    private void Climb()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = gravityScaleAtStart;
            return; 
        }

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);

        steps += 1 * Time.deltaTime;
        if (steps > 0.25f)
        {
            PlaySFX(climbingSFX);
            steps = 0;
        }
    }

    private void Shoot()
    {
        if (isShooting) {
            SetShooting();
            return; 
        }

        if (!isShooting)
        {
            if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            {
                isShooting = true;
                myAnimator.SetBool("Shooting", true);

                PlaySFX(shootingSFX);
            }
        }
    }

    private void FireProjectile() 
    {
        Projectile projectile = Instantiate(projectilePrefab, gunPoint.transform.position, Quaternion.identity);
        projectile.SetProjectileDirection(GetPlayerDirection());
        SetShooting();
    }

    private void Die()
    {
        if (isInvincible) { return; }

        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidBody.velocity = deathKick;

            StartCoroutine(FindObjectOfType<GameSession>().ProcessPlayerDeath());

            PlaySFX(dyingSFX);
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    public float GetPlayerDirection()
    {
        return Mathf.Sign(transform.localScale.x);
    }

    public void SetInvincible(bool toBeInvincible)
    {
        isInvincible = toBeInvincible;
    }

    private void SetShooting()
    {
        isShooting = false;
        myAnimator.SetBool("Shooting", false);
    }

    private void PlaySFX(AudioClip sound)
    {
        AudioSource.PlayClipAtPoint(sound, transform.position);
    }
}
