using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class infoText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gamestate.Instance.assignInfoText(this.GetComponent<Text>());
    }
}
