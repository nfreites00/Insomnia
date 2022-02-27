using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidBody;
    private SpriteRenderer sprite;
    private ParticleSystem dashParticles;

    bool isGrounded = false;
    bool didHitEnemy;
    [SerializeField] Transform groundCheck;

    public float walkSpeed = 4;
    public float jumpSpeed = 10;
    public float fallMultiplier = 2f;
    public float lowJumpMultiplier = 1f;

    // Dashing
    public float startDashTime;
    public float dashSpeed;
    public float dashPenalty;
    private bool isDashing = false;
    private float dashTime;
    private int direction;
    bool isLatched;
    [SerializeField] Transform latchDetector;
    public float latchDetectionRange;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        dashParticles = GetComponentInChildren<ParticleSystem>();

        dashTime = startDashTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Hides chracter when the gamestate is hidden
        sprite.enabled = !gamestate.Instance.getIsPlayerHidden();

        if (!gamestate.Instance.getIsPlayerHidden())
        {
            // Walk animation
            animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));     

            // Movement
            MovementUpdate();

            // Jumping
            JumpUpdate();

            // Dashing
            DashUpdate();    

        }
    }
    
    bool BeingGrabbed()
    {

        var beingGrabbed = false;
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(latchDetector.position, latchDetectionRange, (1 << LayerMask.NameToLayer("Enemies")));

        foreach (Collider2D obj in hitObjects)
        {
            // If a Zombie is grabbing us, set isLatched to true;
            if (obj.CompareTag("Zombie"))
            {
                // Debug.Log("Player grabbed by Zombie!");
                beingGrabbed = true;
            }
        }

        return beingGrabbed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D has been called --> " + collision.gameObject.name);

        if (!isDashing && collision.gameObject.tag == "Enemy" && !gamestate.Instance.getIsPlayerHidden())
        {
            // General enemy
            Debug.Log("We hit an enemy");
            animator.SetBool("DidHitEnemy", true);
            didHitEnemy = true;
        }
        else if (!isDashing && collision.gameObject.tag == "Zombie" && !gamestate.Instance.getIsPlayerHidden())
        {
            // Zombie Case 
            Debug.Log("We hit an Zombie");
            isLatched = true;
            didHitEnemy = true;
            animator.SetBool("DidHitEnemy", true);
        } 

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        animator.SetBool("DidHitEnemy", false);
        didHitEnemy = false;
    }

    // Player Movement
    private void MovementUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rigidBody.velocity = new Vector2(-walkSpeed, rigidBody.velocity.y);
            sprite.flipX = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigidBody.velocity = new Vector2(walkSpeed, rigidBody.velocity.y);
            sprite.flipX = true;
        }
        else
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }
    }

    // Improved Jump Mechanics with better fall
    private void JumpUpdate()
    {
        // To let us know when the player is jumping
        if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigidBody.velocity = Vector2.up * jumpSpeed;
        }

        if (rigidBody.velocity.y < 0)
        {
            rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigidBody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void DashUpdate()
    {

        if (direction == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
            {
                if (rigidBody.velocity.x < 0)
                {
                    dashParticles.Play();
                    GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("sfx");
                    GetComponent<AudioSource>().Play();
                    direction = 1;
                    gamestate.Instance.inflictDamage(0, dashTime, true);
                }
                else if (rigidBody.velocity.x > 0)
                {
                    dashParticles.Play();
                    GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("sfx");
                    GetComponent<AudioSource>().Play();
                    direction = 2;
                    gamestate.Instance.inflictDamage(0, dashTime, true);
                }
            }
        }
        else
        {
            if (dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rigidBody.velocity = Vector2.zero;

                // Animation (working)
                isDashing = false;
                //animator.SetBool("isDashing", isDashing);
            }
            else
            {
                // Animation (working)
                isDashing = true;
                //animator.SetBool("isDashing", isDashing);

                // Timer and movement
                dashTime -= Time.deltaTime;
                if (direction == 1)
                {
                    rigidBody.velocity += Vector2.left * dashSpeed;
                }
                else if (direction == 2)
                {
                    rigidBody.velocity += Vector2.right * dashSpeed;
                }
            }
        }
    }

    public bool DidPlayerDash()
    {
        return isDashing;
    }

    void OnDrawGizmosSelected()
    {
        if (latchDetector == null)
            return;

        Gizmos.DrawWireSphere(latchDetector.position, latchDetectionRange);
    }
}
