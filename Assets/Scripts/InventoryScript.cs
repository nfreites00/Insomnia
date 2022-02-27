using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InventoryScript : MonoBehaviour
{
    public int keyNumber;

    // Update is called once per frame
    void Update()
    {
        if (gamestate.Instance.GetKeyTracker().getKey(keyNumber).isFound())
        {
            string color = gamestate.Instance.GetKeyTracker().getKey(keyNumber).getColor();
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Custom Sprites/Keys/key_" + color);
            //#if UNITY_EDITOR
            //this.GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Custom Sprites/Keys/key_" + color + ".png");
            //#endif
            this.GetComponent<Image>().color = Color.white;
        }
    }
}
