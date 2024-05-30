using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : Equip
{
    public Light flashlight;

    public bool canTurnOff;

    private void Awake()
    {
        flashlight = GetComponentInChildren<Light>();
    }

    public override void UseFlashlight()
    {
        if(canTurnOff)
        {
            canTurnOff = false;
            flashlight.enabled = false;
        }
        else
        {
            canTurnOff = true;
            flashlight.enabled = true;
        }
    }
}
