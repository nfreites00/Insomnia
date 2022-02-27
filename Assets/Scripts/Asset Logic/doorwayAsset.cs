using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorwayAsset : MonoBehaviour
{
    [SerializeField] int keyNumber;
    [SerializeField] bool isReturnDoor;
    [SerializeField] GameObject destination;
    [SerializeField] CanvasGroup overlay; 
    private bool insideArea;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (insideArea && Input.GetKeyDown(KeyCode.E) && !isReturnDoor)
        {
            if (gamestate.Instance.GetKeyTracker().getKey(this.keyNumber).isFound())
            {
                print("Traveling through doorway");
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.transform.position = destination.transform.position;
                player.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            }

            else
            { print("You still need the " + gamestate.Instance.GetKeyTracker().getKey(this.keyNumber).getColor() + " key"); }

        }

        else if (insideArea && Input.GetKeyDown(KeyCode.E) && isReturnDoor)
        {
            print("Returning through door");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = destination.transform.position;
            player.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            insideArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            insideArea = false;
        }
    }
}
