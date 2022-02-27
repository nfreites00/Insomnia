using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class wallLantern : MonoBehaviour
{
    [SerializeField] public bool lit;
    private bool insideArea; 
    // Start is called before the first frame update
    void Start()
    {
        lit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && (!lit) && insideArea)
        {
            print("Candle lit!");
            Light2D lantern = GetComponent<Light2D>();
            lantern.intensity = .8f;
            gamestate.Instance.decrementLanternsRemaining();
            print(gamestate.Instance.getLanternsRemaining());
            lit = true;

            AudioSource source = GetComponent<AudioSource>();
            source.volume = PlayerPrefs.GetFloat("sfx");
            source.Play();
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
