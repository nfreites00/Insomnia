using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hidingPlace : MonoBehaviour
{
    private int remainingUses;
    private bool insideArea;

    private SpriteRenderer openSprite;
    private SpriteRenderer closedSprite;

    // Start is called before the first frame update
    void Start()
    {
        remainingUses = gamestate.Instance.getMaxHideAttempts();
        insideArea = false;

        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in renderers)
        {
            if (s.name == "open")
            {
                openSprite = s;
                openSprite.enabled = true;
            }
            else if (s.name == "closed")
            {
                closedSprite = s;
                closedSprite.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (insideArea && Input.GetKeyDown(KeyCode.E))
        {
            if (gamestate.Instance.getIsPlayerHidden())
            {
                print("Un-hidden!");
                gamestate.Instance.setIsPlayerHidden(false);

                switchSprites();
            }
            else if (!gamestate.Instance.getIsPlayerHidden())
            {
                if (remainingUses > 0)
                {
                    print("Hidden!");
                    // Logic to hide player
                    gamestate.Instance.setIsPlayerHidden(true);
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    player.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                    remainingUses--;

                    switchSprites();
                }
                else
                {
                    print("Out of attempts");
                    // Play sounds or message showing out of attempts

                    if (closedSprite != null)
                    {
                        closedSprite.enabled = true;
                        openSprite.enabled = false;
                    }

                    gamestate.Instance.displayMessage("Oh no! I can't hide here anymore!", 2);
                }
            }
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("Inside hiding place");
            insideArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("Left hiding place");
            insideArea = false;
            gamestate.Instance.setIsPlayerHidden(false);
        }
    }

    private void switchSprites()
    {
        if (openSprite == null || closedSprite == null)
        {
            print("Open/Close sprites do not exist.");
            return;
        }

        openSprite.enabled = openSprite.enabled ? false : true;
        closedSprite.enabled = closedSprite.enabled ? false : true;
    }

}
