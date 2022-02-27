using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class keyAsset : MonoBehaviour
{
    [SerializeField] int keyNumber;
    private bool insideArea;
    // Start is called before the first frame update
    void Start()
    {
        string color = gamestate.Instance.GetKeyTracker().getKey(this.keyNumber).getColor();
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Custom Sprites/Keys/key_" + color);
        //#if UNITY_EDITOR
        //        GetComponent<SpriteRenderer>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/Custom Sprites/Keys/key_" + color + ".png");
        //#endif

    }

    // Update is called once per frame
    void Update()
    {
        if (insideArea && Input.GetKeyDown(KeyCode.E) && !gamestate.Instance.GetKeyTracker().getKey(this.keyNumber).isFound())
        {
            gamestate.Instance.GetKeyTracker().getKey(this.keyNumber).setFound(true);
            GetComponent<SpriteRenderer>().enabled = false; 
            print("Doorway " + keyNumber + " unlocked");

            GetComponent<AudioSource>().Play();
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
