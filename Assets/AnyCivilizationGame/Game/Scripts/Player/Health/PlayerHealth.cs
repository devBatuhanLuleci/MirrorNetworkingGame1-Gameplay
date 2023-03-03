using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    private Coroutine HealthIncreaseCoroutine;
    protected PlayerController playerController;



    public override void Awake()
    {
        base.Awake();
        playerController = GetComponent<PlayerController>();
    }

    public void IncreaseHealthOverTime()
    {

        StopHealthIncreaseCoroutine();

        HealthIncreaseCoroutine = StartCoroutine(IncreaseHealthByTimeCoroutine());

    }
    public void StopHealthIncreaseCoroutine()
    {

        if (HealthIncreaseCoroutine != null)
        {
            StopCoroutine(HealthIncreaseCoroutine);
            HealthIncreaseCoroutine = null;
        }
    }

    IEnumerator IncreaseHealthByTimeCoroutine()
    {


        while (Value < 100)
        {

            yield return new WaitForSeconds(0.5f);



            Value = ((Value + 20) > 100) ? 100 : Value + 20;

            playerController.AnimateOtherHealthBarEffects(netIdentity.connectionToClient);
            if (Value == 100)
            {
                playerController.ChangeDamageTakenStatus(PlayerController.DamageTakenStatus.Idle);
                
            }

            //    ChangeHealth(Value, true);
        }

        yield return null;
    }
    public override void Update()
    {
        base.Update();

        if (netIdentity.isServer)
        {


            if (Input.GetKeyDown(KeyCode.T))
                IncreaseHealthOverTime();
        }


    }

}
