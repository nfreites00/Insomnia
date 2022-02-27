using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorwayRandom : MonoBehaviour
{
    public int doorNumber;
    public int keyNumber;
    public GameObject destination;
    [SerializeField] CanvasGroup overlay;
    private bool insideArea;
    private bool fadeIn;
    private bool fadeOut;
    [SerializeField] bool isSpawn;
    [SerializeField] AudioSource doorwaySound;
    private bool audioPlaying;
    // Start is called before the first frame update
    void Start()
    {
        insideArea = false;
        audioPlaying = false;
        fadeIn = false;
        fadeOut = false;
        overlay.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (insideArea && Input.GetKeyDown(KeyCode.E) || fadeIn)
        {
            if (((this.keyNumber > -1) && gamestate.Instance.GetKeyTracker().getKey(this.keyNumber).isFound()) && (destination != null))
            {
                fadeIn = true;
                overlay.alpha = overlay.alpha + Time.deltaTime * 1.2f;
                if (overlay.alpha >= 1) // if its fully opaque
                {
                    doorwaySound.volume = PlayerPrefs.GetFloat("sfx");
                    doorwaySound.Play();
                    print("Traveling through doorway");
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    player.transform.position = destination.transform.position;
                    player.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                    fadeIn = false;
                    fadeOut = true;
                }
            }

            else if (this.keyNumber == -1 && (destination != null))
            {
                fadeIn = true;
                overlay.alpha = overlay.alpha + Time.deltaTime * 1.2f;
                if (overlay.alpha >= 1) // if its fully opaque
                {
                    doorwaySound.volume = PlayerPrefs.GetFloat("sfx");
                    doorwaySound.Play();
                    print("Returning through door");
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    player.transform.position = destination.transform.position;
                    player.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                    fadeIn = false;
                    fadeOut = true;
                }

            }

            else if (this.keyNumber > -1)
            { gamestate.Instance.displayMessage("Looks like I still need to find the " + gamestate.Instance.GetKeyTracker().getKey(this.keyNumber).getColor() + " key", 2); }
            else
            { gamestate.Instance.displayMessage("Hmm... there's no key for this door.", 2); }

        }
        else if (fadeOut)
        {
            overlay.alpha = overlay.alpha - Time.deltaTime * 1.2f;
            if (overlay.alpha <= 0)
            {
                overlay.alpha = 0;
                fadeOut = false;
            }
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
