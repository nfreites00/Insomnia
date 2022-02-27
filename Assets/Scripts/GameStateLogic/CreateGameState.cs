using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGameState : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print("Starting game");
        DontDestroyOnLoad(gamestate.Instance);
        print("Exploding");
        Destroy(this.gameObject);
    }
}
