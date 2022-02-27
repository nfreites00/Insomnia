using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    const string LEFT = "left";
    const string RIGHT = "right";
    string facingDirection;

    // Object references for Zombie.
    Rigidbody2D rb;
    [SerializeField] Transform castPos;
    [SerializeField] Transform groundDetector;
    [SerializeField] Transform attackPoint;
    [SerializeField] float baseCastDist;
    [SerializeField] float groundCastDist;

    // Vectors
    Vector3 baseScale;
    Vector3 castPosition;
    Vector3 castScale;

    public Animator animator;
    public LayerMask playerLayer;

    // Base stats for Zombie
    // TODO: Have these stats refer to the Enemy Manager
    public float moveSpeed = 2.5f;
    private float baseMoveSpeed;
    public float aggroDistance;
    public float speedUpDistance;
    public float chaseSpeed;
    public float attackRange = 0.5f;


    // State Booleans
    bool isAggro;
    bool isPatrol;
    bool isIdle;
    bool isLatching;
    bool isSearching;

    // MinValue one is used to signify that no patrol or idle times have been assigned.
    private float idleTime = (float)System.Int32.MinValue;
    private float patrolTime = (float)System.Int32.MinValue;

    private float searchTime = (float)System.Int32.MinValue;
    private EnemyMode enemyMode = EnemyMode.patrol;

    // For Slopes
    private CapsuleCollider2D cc;
    private Vector2 newVelocity;
    private Vector2 newForce;
    private Vector2 colliderSize;

    private float slopeDownAngle;
    private float slopeDownAngleOld;
    private float slopeSideAngle;
    private bool isOnSlope;

    private Vector2 slopeNormalPerpendicular;

    [SerializeField] private float slopeCheckDistance;

    private Rigidbody2D playerRB;

    float nextSoundTime;

    // Start is called before the first frame update
    void Start()
    {
        baseScale = transform.localScale;
        castPosition = castPos.position;
        castScale = castPos.localScale;
        facingDirection = LEFT;
        baseMoveSpeed = moveSpeed;
        isPatrol = true;
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        colliderSize = cc.size;
        // playerRB = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isPatrol", isPatrol);
        animator.SetBool("isAggro", isAggro);

        Debug.Log("Zombie State = " + enemyMode);
        // Debug.Log("isLatching = " + isLatching);

        // if (CanLatch())
        // {
        //     isLatching = true;
        //     // isAggro = true;
        //     enemyMode = EnemyMode.latch;
        // }

        // Update sound timer
        if (nextSoundTime > 0f)
        {
            nextSoundTime = nextSoundTime - Time.deltaTime;
        }

        // Determine if we need to speed up to chase the player.
        if (CanSeePlayer(speedUpDistance))
        {
            moveSpeed = chaseSpeed;
            // enemyMode = EnemyMode.patrol;
        }

        // Determine if we can attack the player.
        if (!isLatching && CanSeePlayer(aggroDistance))
        {
            isIdle = false;
            isPatrol = false;
            enemyMode = EnemyMode.attack;
        }

        // TODO: See if the player is hiding nearby so we can search.
        if (!isLatching && !CanSeePlayer(speedUpDistance) && CanDoSearch())
        {
            enemyMode = EnemyMode.search;
        }

        switch (enemyMode)
        {
            case EnemyMode.idle:
                isIdle = true;
                isPatrol = false;
                isAggro = false;
                // isLatching = false;
                // isSearching = false;
                CalcIdleTime();
                Idle();
                break;
            case EnemyMode.patrol:
                isPatrol = true;
                isIdle = false;
                // isAggro = false;
                // isLatching = false;
                // isSearching = false;
                CalcPatrolTime();
                Patrol();
                break;
            case EnemyMode.attack:
                isAggro = true;
                isIdle = false;
                isPatrol = false;
                // isLatching = false;
                // isSearching = false;
                Attack();
                break;
            // case EnemyMode.latch:
            //     isAggro = true;
            //     isIdle = false;
            //     isPatrol = false;
            //     isLatching = true;
            //     isSearching = false;
            //     Latch();
            //     break;
            // case EnemyMode.search:
            //     isAggro = false;
            //     isIdle = false;
            //     isPatrol = false;
            //     isLatching = false;
            //     isSearching = true;
            //     CalcSearchTime();
            //     Search();
            //     break;
            default:
                break;
        }
    }

    void FixedUpdate()
    {
        // if (isLatching || isAggro)
        // {
        //     this.rb.velocity.Set(0f, 0f);
        //     return;
        // }

        if (CanSeePlayer(speedUpDistance))
        {
            moveSpeed = chaseSpeed;
            enemyMode = EnemyMode.patrol;
        }

        SlopeCheck();
        ApplyMovement();
    }

    // Logic for Zombie Idle State
    private void Idle()
    {
        isPatrol = false;

        if (idleTime > 0f)
        {
            idleTime = idleTime - Time.deltaTime;
            moveSpeed = 0;
        }
        else
        {
            enemyMode = EnemyMode.patrol;
            idleTime = (float)System.Int32.MinValue;

            return;
        }
    }

    // Logic for Zombie Patrol state
    private void Patrol()
    {
        moveSpeed = baseMoveSpeed;
        float xVel = moveSpeed;

        if (facingDirection == LEFT)
        {
            xVel = -moveSpeed;
        }

        if (patrolTime > 0f)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            patrolTime = patrolTime - Time.deltaTime;
        }
        else
        {
            // return to Idle State
            isPatrol = false;
            enemyMode = EnemyMode.idle;
            patrolTime = (float)System.Int32.MinValue;
            return;
        }

    }

    // Logic for Zombie Attack state
    private void Attack()
    {
        isAggro = true;
        patrolTime = (float)System.Int32.MinValue;
        idleTime = (float)System.Int32.MinValue;

        HitPlayer();

        // if (CanLatch())
        // {
            // enemyMode = EnemyMode.latch;
            // Latch();
        // }

        if (!CanSeePlayer(aggroDistance))
        {
            // Zombie has about a 50% chance of going into idle or patrol
            // after attacking the player.
            float decider = UnityEngine.Random.Range(0f, 1f);
            isAggro = false;

            if (decider >= .5f)
                enemyMode = EnemyMode.patrol;
            else
                enemyMode = EnemyMode.idle;
        }
    }

    void PlayZombieSound()
    {
        //float playChance = Mathf.Ceil(UnityEngine.Random.Range(1f, 100f));
        //Debug.Log("playchance = " + playChance);

        //if (playChance <= 50f)
        //{
        //    if (nextSoundTime >= 0f)
        //    {
        //        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("sfx");
        //        GetComponent<AudioSource>().Play();
        //        nextSoundTime = Mathf.Ceil(UnityEngine.Random.Range(3f, 10f));
        //    }
        //}
    }
    
    void PlayAttackSound()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("sfx");
        GetComponent<AudioSource>().Play();
    }

    private bool CanLatch()
    {
        var canLatch = false;
        isAggro = true;
         // Store all objects that get hit and loop through them.
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D obj in hitObjects)
        {
            // If we hit the Player, then do damage to the Player.
            if (obj.CompareTag("Player"))
            {
                Debug.Log("Zombie can latch onto the Player.");

                canLatch = true;
            }
        }

        return canLatch;
    }

    // Logic for Zombie Latch state
    private void Latch()
    {
        // Prevent the player from moving until the player dashes.
        // Should not be cheesable. Zombie should waste the players time
        // Goal is to run down the candle timer and to deal damage.

        // Debug.Log("Zombie is latching!");
        // Debug.Log("Did Player dash? --> " + player.GetComponent<PlayerController2D>().DidPlayerDash());

        // Deal damage when the player inviciblity runs out.
        gamestate.Instance.inflictDamage(20, true);

        // Latch on and deal damage until the player dashes 
        // When the player dashes, Frankie will idle for a few seconds.
        // if (player.GetComponent<PlayerController2D>().DidPlayerDash())
        // {
        //     Debug.Log("PLAYER DASHED!!!!!");
        //     isLatching = false;
        //     enemyMode = EnemyMode.idle;
        // } 
        // else
        // {
        //     // playerRB.velocity.Set(0f, 0f);
        //     // playerRB.angularVelocity = 0f;
        // }
    }

    private bool CanDoSearch()
    {
        var canSearch = false;

        // TODO: If the player is hidden within a certain radius, look back and forth.
        
        return canSearch;
    }

    private float CalcSearchTime()
    {
        if (idleTime == (float)System.Int32.MinValue)
            searchTime = Mathf.Ceil(UnityEngine.Random.Range(1f, 5f));

        return searchTime;
    }

    // Logic for Zombie Search State
    private void Search()
    {
        // Need to find a way to detect Player location
        // If a player hides within a certain radius the Zombie will look 
        // back and forth for the player for a few seconds.
        // (Flips on y-axis a few times, maybe walks around...)

        // TODO: Need to implement a timer to the Zombie will flip.

        // Creates a collider in order to check if the zombie collider is hitting the player's.
        Vector2 center = new Vector2(this.transform.position.x, this.transform.position.y);
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(center, 100);
        int i = 0;
        float flipTimer = 7f;

        // The loop below isn't exactly working. It only flips once (was following video for some code below).
        while (i < hitCollider.Length)
        {
            newVelocity.Set(0.0f, 0.0f); // stops zombie movement
            rb.velocity = newVelocity;
            if ((hitCollider[i].transform.tag == "Player") && (gamestate.Instance.getIsPlayerHidden()))
            {
                if (flipTimer >= 0f)
                {
                    if (facingDirection == LEFT)
                    {
                        ChangeFacingDirection(RIGHT);
                    }
                    else if (facingDirection == RIGHT)
                    {
                        ChangeFacingDirection(LEFT);
                    }

                }
            }
            flipTimer = flipTimer - 1f;
        }

        // Zombie should start walking again.
        newVelocity.Set(moveSpeed, -0.5f);
        rb.velocity = newVelocity;


        if (searchTime > 0f)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            searchTime = searchTime - Time.deltaTime;
        }
        else
        {
            // return to Idle State
            enemyMode = EnemyMode.idle;
            searchTime = (float)System.Int32.MinValue;
            return;
        }
    }

    private void ApplyMovement()
    {
        if (isIdle || isAggro)
        {
            newVelocity.Set(0.0f, 0.0f);
            rb.velocity = newVelocity;
            return;
        }

        float xVel = moveSpeed;

        if (IsHittingWall())
        {
            if (facingDirection == LEFT)
            {
                ChangeFacingDirection(RIGHT);
            }
            else if (facingDirection == RIGHT)
            {
                ChangeFacingDirection(LEFT);
            }
        }

        if (facingDirection == LEFT)
            xVel = -moveSpeed;


        if (IsOnGround() && !isOnSlope)
        {
            // If on flat ground move along the x axis.
            newVelocity.Set(xVel, -0.5f);
            rb.velocity = newVelocity;

        }
        else if (IsOnGround() && isOnSlope)
        {
            // If on a slope, apply velocity in the direction of the slope.
            newVelocity.Set(xVel * -slopeNormalPerpendicular.x, moveSpeed * slopeNormalPerpendicular.y);
            rb.velocity = newVelocity;

        }
        else if (!IsOnGround())
        {
            // If in the air do the following.
            newVelocity.Set(xVel, rb.velocity.y);
            rb.velocity = newVelocity;
        }
    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y / 2);
        VerticalSlopeCheck(checkPos);
        // HorizontalSlopeCheck(checkPos);
    }

    private void HorizontalSlopeCheck(Vector2 checkPos)
    {
        RaycastHit2D front = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, 1 << LayerMask.NameToLayer("Ground"));
        RaycastHit2D back = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, 1 << LayerMask.NameToLayer("Ground"));

        Debug.DrawRay(front.point, -transform.right, Color.green);
        Debug.DrawRay(back.point, transform.right, Color.red);

        // If our rays detect something then we have a slope!
        if (front)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(front.normal, Vector2.up);
        }
        else if (back)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(back.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
    }

    private void VerticalSlopeCheck(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, 1 << LayerMask.NameToLayer("Ground"));

        if (hit)
        {
            slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized;
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld)
            {
                isOnSlope = true;
            }

            slopeDownAngleOld = slopeDownAngle;

            // Debug.DrawRay(hit.point, hit.normal, Color.green);
            // Debug.DrawRay(hit.point, slopeNormalPerpendicular, Color.red);
        }
    }
    private float CalcIdleTime()
    {
        if (idleTime == (float)System.Int32.MinValue)
            idleTime = Mathf.Ceil(UnityEngine.Random.Range(1f, 3f));

        return idleTime;
    }

    private float CalcPatrolTime()
    {
        if (patrolTime == (float)System.Int32.MinValue)
            patrolTime = Mathf.Ceil(UnityEngine.Random.Range(1f, 8f));

        return patrolTime;
    }

    // When attacking, if we hit the player, tell the GSM to inflict damage onto the player. 
    void HitPlayer()
    {
        // Store all objects that get hit and loop through them.
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D obj in hitObjects)
        {
            // If we hit the Player, then do damage to the Player.
            if (obj.CompareTag("Player"))
            {
                // Debug.Log("Zombie damaged the Player.");

                // Deal damage to player.
                gamestate.Instance.inflictDamage(gamestate.Instance.getZombieDamage(), true);
            }

            Debug.Log("Zombie hit " + obj.name);
        }
    }

    bool CanSeePlayer(float distance)
    {
        bool val = false;
        float castDist = distance;

        if (facingDirection == LEFT)
        {
            castDist = -distance;
        }

        Vector2 endPos = castPos.position + (Vector3.right * castDist);
        RaycastHit2D hit = Physics2D.Linecast(castPos.position, endPos, (1 << LayerMask.NameToLayer("Playspace")));

        Debug.DrawLine(castPos.position, endPos, Color.green);

        if (hit.collider != null)
        {
            if ((hit.collider.gameObject.CompareTag("Player")) && !(gamestate.Instance.getIsPlayerHidden()))
            {
                val = true;
                Debug.DrawLine(castPos.position, endPos, Color.red);
            }
            else
            {
                val = false;
            }
        }
        else
        {
            Debug.DrawLine(castPos.position, endPos, Color.white);
        }

        return val;
    }

    bool IsOnGround()
    {
        bool val = false;

        // Determine target destination based on cast distance
        Vector3 targetPos = groundDetector.position - (Vector3.down * -groundCastDist);

        Debug.DrawLine(groundDetector.position, targetPos, Color.white);

        if (Physics2D.Linecast(groundDetector.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            Debug.DrawLine(groundDetector.position, targetPos, Color.blue);
            val = true;
        }

        return val;
    }
    bool IsHittingWall()
    {
        bool val = false;
        float castDist = baseCastDist;

        // Define cast distance for left and right
        if (facingDirection == LEFT)
        {
            castDist = -baseCastDist;
        }

        // Determine target destination based on cast distance
        Vector3 targetPos = castPos.position + (Vector3.right * castDist);

        Debug.DrawLine(castPos.position, targetPos, Color.white);

        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            Debug.DrawLine(castPos.position, targetPos, Color.blue);
            val = true;
        }

        return val;
    }

    void ChangeFacingDirection(string newDirection)
    {
        Vector3 newScale = baseScale;

        if (newDirection == RIGHT)
        {
            newScale.x = -baseScale.x;
        }
        else if (newDirection == LEFT)
        {
            newScale.x = baseScale.x;
        }

        transform.localScale = newScale;
        facingDirection = newDirection;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public enum EnemyMode
    {
        idle,
        patrol,
        attack,
        latch,
        search,
    }
}
