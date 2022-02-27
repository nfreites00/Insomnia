using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowSkeleton : MonoBehaviour
{
    const string LEFT = "left";
    const string RIGHT = "right";
    Rigidbody2D rb;
    
    // For projectile
    public GameObject arrow;
    public Transform launchPoint;
    private float timeBtwShots;
    public float startTimeBtwShots;

    // Gizmos
    [SerializeField] Transform castPos;
    [SerializeField] Transform groundDetector;
    [SerializeField] float baseCastDist;
    [SerializeField] float groundCastDist;

    // Vectors
    Vector3 baseScale;
    Vector3 castPosition;
    Vector3 castScale;
    Vector3 launchPosition;

    // Public Game Components
    public Animator animator;
    public LayerMask playerLayer;

    // Entity Variables
    public float moveSpeed = 2.5f;
    private float baseMoveSpeed;
    public float aggroDistance;
    public float attackRange = 0.5f;
    string facingDirection;

    // State Booleans
    bool isAggro;
    bool isPatrol;
    bool isIdle;

    // MinValue is used to signify that no patrol or idle times have been assigned.
    private float idleTime = (float)System.Int32.MinValue;
    private float patrolTime = (float)System.Int32.MinValue;
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

        launchPosition = launchPoint.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isPatrol", isPatrol);
        animator.SetBool("isAggro", isAggro);

        // Debug.Log("Bow Skeleton State = " + enemyMode);
        // Debug.Log("IsPatrol = " + isPatrol);
        // Debug.Log("IsPatrol = " + facingDirection);

        if (CanSeePlayer(aggroDistance))
        {
            isIdle = false;
            isPatrol = false;
            enemyMode = EnemyMode.attack;
        }

        switch (enemyMode)
        {
            case EnemyMode.idle:
                isIdle = true;
                CalcIdleTime();
                Idle();
                break;
            case EnemyMode.patrol:
                isPatrol = true;
                CalcPatrolTime();
                Patrol();
                break;
            case EnemyMode.attack:
                isAggro = true;
                Attack();
                break;
            default:
                break;
        }
    }
    void FixedUpdate()
    {
        SlopeCheck();
        ApplyMovement();
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
            idleTime = Mathf.Ceil(UnityEngine.Random.Range(1f, 5f));

        return idleTime;
    }

    private float CalcPatrolTime()
    {
        if (patrolTime == (float)System.Int32.MinValue)
            patrolTime = Mathf.Ceil(UnityEngine.Random.Range(3f, 7f));

        return patrolTime;
    }

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
            isIdle = false;
            enemyMode = EnemyMode.patrol;
            idleTime = (float)System.Int32.MinValue;

            return;
        }
    }
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
    private void Attack()
    {
        isAggro = true;
        patrolTime = (float)System.Int32.MinValue;
        idleTime = (float)System.Int32.MinValue;

        // Shoot Player
        if (timeBtwShots <= 0) 
        {
            // GameObject arrowInstance = Instantiate(arrow, launchPoint.position, Quaternion.identity);
            // arrowInstance.GetComponent<ProjDataContainer>().SetValues(facingDirection.ToUpper());
            timeBtwShots = startTimeBtwShots;
        } 
        else 
        {
            timeBtwShots -= Time.deltaTime;
        }   

        if (!CanSeePlayer(aggroDistance))
        {
            // Skeleton has about a 50% chance of going into idle or patrol
            // after attacking the player.
            float decider = UnityEngine.Random.Range(0f, 1f);
            isAggro = false;

            if (decider >= .5f)
                enemyMode = EnemyMode.patrol;
            else
                enemyMode = EnemyMode.idle;
        }
    }

    void SpawnArrow()
    {
        GameObject arrowInstance = Instantiate(arrow, launchPoint.position, Quaternion.identity);
        arrowInstance.GetComponent<ProjDataContainer>().SetValues(facingDirection.ToUpper());

        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("sfx");
        GetComponent<AudioSource>().Play();
    }

    void Rattle()
    {
        int playChance = Random.Range(1, 100);

        if (playChance <= 5)
        {
            // TODO: Add rattle sound
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

    public enum EnemyMode
    {
        idle,
        patrol,
        attack
    }
}
