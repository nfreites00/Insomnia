using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField]
    float speed = 5;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D temp = collision.GetComponent<Rigidbody2D>();

            if (Input.GetKey(KeyCode.W))
            {
                temp.velocity = new Vector2(temp.velocity.x, speed);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                temp.velocity = new Vector2(temp.velocity.x, -speed);
            }
            else
            {
                temp.velocity = new Vector2(0, 0);
            }
        }
    }
    void Update()
    {
        
    }
}
