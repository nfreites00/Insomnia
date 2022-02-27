using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float baseSpeed = 5.0f;

    float currentSpeed;

    Rigidbody2D rb;

    public void SpeedChangeHelper(float newSpeed, float timeInSecs)
    {
        StartCoroutine(SpeedChange(newSpeed, timeInSecs));
    }

    IEnumerator SpeedChange(float newSpeed, float timeInSecs)
    {
        currentSpeed = newSpeed;
        yield return new WaitForSeconds(timeInSecs);
        currentSpeed = baseSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float horizontalValue = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontalValue, 0, 0);

        rb.velocity = direction * currentSpeed;
        if (horizontalValue > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        else if (horizontalValue < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
