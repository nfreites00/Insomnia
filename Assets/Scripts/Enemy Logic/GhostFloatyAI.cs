using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFloatyAI : MonoBehaviour
{
    // Delete this comment
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float frequency = 5;
    [SerializeField] float amplitude = 0.5f;
    [SerializeField] float strafeDistance = 2f;
    
    // Max distance for enemy aggro line of sight
    [SerializeField] float aggroDistance;
    [SerializeField] float aggroSpeedMult;
    [SerializeField] Transform castPoint;
    public Animator animator;
    Vector3 pos, localScale;
    bool isFacingRight;
    bool isAggro;
    bool isCharging;
    bool touchingPlayer;


    // These will allow the ghost to strafe from any position on the map.
    float maxStrafe = 1f;
    float minStrafe = 1f;

    float nextSoundTime;

    void Start()
    {
        pos = transform.position;
        localScale = transform.localScale;

        // Establish bounds for movement
        maxStrafe = Mathf.Abs((float)(pos.x + (strafeDistance)));
        minStrafe = Mathf.Abs((float)(pos.x + -strafeDistance));

        if (pos.x < 0)
        {
            minStrafe = -minStrafe;
            maxStrafe = -maxStrafe;
        }

        moveSpeed = Mathf.Abs(moveSpeed);
    }

    void Update()
    {
        // Debug.Log("MaxStrafe = " + maxStrafe + ", MinStrafe = " + minStrafe);

        // Grab vars for animation controller
        animator.SetFloat("MoveSpeed", Mathf.Abs(moveSpeed));
        animator.SetBool("isCharging", isCharging);
        animator.SetBool("isAggro", isAggro);

        // Update sound timer
        if (nextSoundTime > 0f)
        {
            nextSoundTime = nextSoundTime - Time.deltaTime;
        }

        CheckWhereToFace();

        // Aggro if we see the player
        if (CanSeePlayer(aggroDistance))
        {
            // TODO: Take 1 second to play charging animation.
            // Attack player in a straight line.
            isAggro = true;
            ChasePlayer();
        }
        else
        {
            if (isAggro)
            {
                StopChasingPlayer();
            }
        }

        // Move normally if player not in range
        if (!isAggro)
        {
            if (isFacingRight)
            {
                // move left
                pos -= transform.right * Time.deltaTime * moveSpeed;
                transform.position = pos + transform.up * Mathf.Sin(Time.time * frequency) * amplitude;
            }
            else
            {
                // move right
                pos += transform.right * Time.deltaTime * moveSpeed;
                transform.position = pos + transform.up * Mathf.Sin(Time.time * frequency) * amplitude;
            }
        }
    }

    void CheckWhereToFace()
    {
        if (pos.x < minStrafe)
            isFacingRight = false;
        else if (pos.x > maxStrafe)
            isFacingRight = true;

        // Debug.Log("IsFacingRight = " + isFacingRight);

        if (((!isFacingRight) && (localScale.x > 0)) || ((isFacingRight) && (localScale.x < 0)))
            localScale = new Vector2(-(Mathf.Sign(transform.localScale.x)) * Mathf.Abs(transform.localScale.x), transform.localScale.y);

        transform.localScale = localScale;
    }

    bool CanSeePlayer(float distance)
    {
        bool val = false;
        float castDist = distance;

        if (isFacingRight)
        {
            castDist = -distance;
        }
        Vector2 endPos = castPoint.position + (Vector3.right * castDist);
        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, (1 << LayerMask.NameToLayer("Playspace")));

        Debug.DrawLine(castPoint.position, endPos, Color.white);

        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.gameObject.name);
            if ((hit.collider.gameObject.CompareTag("Player")) 
                && !(gamestate.Instance.getIsPlayerHidden()))
            {
                val = true;

                // Debug.Log("Player sighted by ghost.");
                Debug.DrawLine(castPoint.position, endPos, Color.red);

                if (touchingPlayer)
                {
                    gamestate.Instance.inflictDamage(gamestate.Instance.getGhostDamage(), true);
                    print(gamestate.Instance.getCurrentHP());

                }
            }
            else
            {
                val = false;
            }

        }
        else
        {
            Debug.DrawLine(castPoint.position, endPos, Color.white);
        }

        return val;
    }

    void ChargeUp(bool charging)
    {
        isCharging = true;
        
        // TODO: Add charge-up animation
    }
    void ChasePlayer()
    {
        isCharging = false;

        // TODO: Add new movement for chasing the player.
        if (isFacingRight)
        {
            // move left
            pos -= transform.right * Time.deltaTime * (aggroSpeedMult * moveSpeed);
            transform.position = pos;
        }
        else
        {
            // move right
            pos += transform.right * Time.deltaTime * (aggroSpeedMult * moveSpeed);
            transform.position = pos;
        }
    }

    void StopChasingPlayer()
    {
        isAggro = false;
    }

    void PlayGhostlyWail()
    {
        float playChance = Mathf.Ceil(UnityEngine.Random.Range(1f, 100f));
        Debug.Log("playchance = " + playChance);

        if (playChance <= 50f)
        {
            if (nextSoundTime >= 0f)
            {
                GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("sfx");
                GetComponent<AudioSource>().Play();
                nextSoundTime = Mathf.Ceil(UnityEngine.Random.Range(3f, 10f));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player")) && !(gamestate.Instance.getIsPlayerHidden()))
        {
            touchingPlayer = true;
            Debug.Log("HIT THE PLAYER!");

            // TODO: Play sound to rush player. 
            // GetComponent<AudioSource>().Play();

            gamestate.Instance.inflictDamage(gamestate.Instance.getGhostDamage(), true);
            print(gamestate.Instance.getCurrentHP());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player")))
        {
            touchingPlayer = false;
        }
    }



}
