using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyTracker : MonoBehaviour
{
    private Key[] keys;
    private int numberOfKeys;
    private static keyTracker instance;

    // Start is called before the first frame update
    public static keyTracker Instance
    {
        get
        {
                return new GameObject("keyTracker").AddComponent<keyTracker>();
        }
    }


    public Key getKey(int keyNumber)
    {
        return keys[keyNumber];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startEasyState()
    {
        numberOfKeys = 10;
        keys = new Key[numberOfKeys];
        for (int i = 0; i < numberOfKeys; i++)
        {
            keys[i] = new Key();
        }
        keys[1].setColor("golden");
        keys[2].setColor("pink");
        keys[3].setColor("green");
        keys[4].setColor("blue");
        keys[5].setColor("purple");
        keys[6].setColor("orange");
        keys[7].setColor("red");
        keys[8].setColor("white");

        print("Key Tracker Started");
    }
}
