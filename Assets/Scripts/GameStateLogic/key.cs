using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key
{
    private bool found;
    private string color;
    private bool active;

    public Key()
    {
        found = false;
        active = false;
    }

    public void setActive(bool input)
    {
        this.active = input;
    }

    public bool getActive()
    {
        return active; 
    }

    public bool isFound()
    {
        return found;
    }

    public void setFound(bool input)
    {
        found = input;
    }

    public string getColor()
    {
        return this.color;
    }

    public void setColor(string color)
    {
        this.color = color;
    }
}
