using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPatrolAI : MonoBehaviour
{

    [SerializeField] float moveSpeed = 1f;

    // Cache references
    Rigidbody2D myRigidbody;
    public Animator animator;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        moveSpeed = Mathf.Abs(moveSpeed);
    }

    void Update()
    {
        animator.SetFloat("MoveSpeed", Mathf.Abs(moveSpeed));

        if (!IsFacingRight())
        {
            // move left
            myRigidbody.velocity = new Vector2(moveSpeed, Mathf.Sin(Time.deltaTime * .5f) * 2);
        }
        else
        {
            // move right
            myRigidbody.velocity = new Vector2(-moveSpeed, -Mathf.Sin(Time.deltaTime * .5f) * 2);
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Turn
        // Debug.Log("Box collider has been triggered.");
        transform.localScale = new Vector2((Mathf.Sign(myRigidbody.velocity.x)) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }
}
