using UnityEngine;
using System.Collections;

public class openChest : MonoBehaviour
{
    [SerializeField] bool containsKey;
    private bool insideArea;
    private bool closed;
    private SpriteRenderer keyRenderer;
    private SpriteRenderer closedChestRenderer;
    private SpriteRenderer openChestRenderer;
    private BoxCollider2D keyCollider;
    // Use this for initialization
    void Start()
    {


        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer c in renderers)
        {
            if (c.CompareTag("KeyRandom"))
            { this.keyRenderer = c; }
            else if (c.name == "ClosedChest")
            { this.closedChestRenderer = c; }
            else if (c.name == "OpenChest")
            { this.openChestRenderer = c; }
            else
            { print("Uh oh. Unexcpected child in OpenChest prefab"); }
        }

        BoxCollider2D[] colliders = GetComponentsInChildren<BoxCollider2D>();
        foreach (BoxCollider2D c in colliders)
        {
            if (c.CompareTag("KeyRandom"))
            { this.keyCollider = c; }
            else if (c.name == "OpenChest" || c.name == "ClosedChest")
            { continue; }
            else
            { print("Uh oh. Unexpected child in OpenChest prefab"); }
        }


        closedChestRenderer.enabled = true;
        openChestRenderer.enabled = false; 
        closed = true;

    }

    // Update is called once per frame
    void Update()
    {

        if (insideArea && Input.GetKeyDown(KeyCode.E) && closed)
        {
            closedChestRenderer.enabled = false;
            openChestRenderer.enabled = true; 
            if (containsKey && gameObject.transform.GetChild(2).gameObject.activeSelf)
            {

                keyRenderer.enabled = true;
                keyCollider.enabled = true;
            }
            closed = false;

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
