using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MegaMarble : Powerups
{
    [SerializeField] float megaMArbleBoost = 5f;
    public class OnUseMegaMarble : UnityEvent { };
    public static OnUseMegaMarble onUseMegaMarble = new OnUseMegaMarble();
    public static bool alreadyListened = false;

    protected override void Start()
    {
        base.Start();

        if (!alreadyListened)
        {
            alreadyListened = true;
            onUseMegaMarble.AddListener(UsePowerup);
        }
    }

    public void OnDisable()
    {
        alreadyListened = false;
        onUseMegaMarble.RemoveAllListeners();
    }

    public void OnEnable()
    {
        if (!alreadyListened)
        {
            alreadyListened = true;
            onUseMegaMarble.AddListener(UsePowerup);
        }
    }
    protected override void UsePowerup()
    {
        Marble.instance.UseMegaMarble();
        GameManager.instance.megaActiveTime = Time.time;
        Movement.instance.gameObject.transform.localScale = Movement.instance.gameObject.transform.localScale * 2.25f;
        Movement.instance.marbleRadius = Movement.instance.marbleRadius * 2.25f;
        //Movement.instance.marbleVelocity += -GravitySystem.GravityDir.normalized * megaMArbleBoost;
    }
}
