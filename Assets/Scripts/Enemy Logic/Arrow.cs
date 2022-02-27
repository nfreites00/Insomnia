using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // This should jsut allow the arrow to fly in a sraight line until 
    // it hits something that exists in the foreground or the actual player.
    public float speed;
    private string direction;
    public ProjDataContainer data;
    public float lifetime;
    Vector3 pos, localScale;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        localScale = transform.localScale;
        direction = data.GetDirection();
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;

        // Destroy game object when the full lifetime has been used up
        if (lifetime <= 0)
        {
            DestroyArrow();
        }

        // Debug.Log("Local Scale X Value of Arrow == " + localScale.x);
        // Debug.Log("Facing direction == *" + direction + "*");
        if (direction == "LEFT")
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            if (localScale.x <= 0)
                localScale = new Vector2(-(Mathf.Sign(transform.localScale.x)) * Mathf.Abs(transform.localScale.x), transform.localScale.y);

            transform.localScale = localScale;
        }
        else
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (localScale.x >= 0)
                localScale = new Vector2(-(Mathf.Sign(transform.localScale.x)) * Mathf.Abs(transform.localScale.x), transform.localScale.y);

            transform.localScale = localScale;
        }
        // Check if we hit a qualified object, if so, destroy this object.
        // On trigger enter.
        // Otherwise continue to fly.
    }

    public void setValues(string facingDirection)
    {
        this.direction = facingDirection;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") || other.CompareTag("Ground"))
        {

            // Only damage the player if we hit the player.
            if (other.CompareTag("Player") && !gamestate.Instance.getIsPlayerHidden())
            {
                gamestate.Instance.inflictDamage(gamestate.Instance.getSkeletonDamage(), true);
                Debug.Log("Damaged the player.");
            }

            // Audio
            AudioSource source = GetComponent<AudioSource>();
            source.volume = PlayerPrefs.GetFloat("sfx");
            source.Play();
            // Debug.Log("Arrow audio");

            GetComponent<SpriteRenderer>().enabled = false;

            while (!source.isPlaying)
            {
                DestroyArrow();
            }
        }
    }

    void DestroyArrow()
    {
        Destroy(gameObject);
    }
}
