using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjDataContainer : MonoBehaviour
{
    string direction;

    // NOTE: These functions may be overloaded based on what kind of 
    // projectile is being launced.
    public void SetValues(string facingDirection)
    {
        this.direction = facingDirection;
    }

    public string GetDirection() {return this.direction;}
}
