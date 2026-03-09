using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UltraBlast : Powerups
{
    public float timeBonus = 5f;

    protected override void Start()
    {
        base.Start();

        //bottomTextMsg = "You picked up a " + timeBonus.ToString("0.###") + " second Time Modifier Bonus!";
    }

    protected override void UsePowerup()
    {
        Marble.instance.ActivateTimeTravel(timeBonus);
    }
}
