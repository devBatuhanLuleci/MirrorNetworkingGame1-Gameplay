using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatboyHealth : Health
{
    private PlayerController playerController;
    float perBarAmount = 0.333f;
    private Coroutine HealthIncreaseCoroutine;
    public override void Awake()
    {
        base.Awake();
        playerController = GetComponent<PlayerController>();
    }
    public override void RefreshUI(int oldValue, int newValue)
    {
        base.RefreshUI(oldValue, newValue);
        playerController.HealthChanged(newValue);
    }

    public void IncreaseHealthOverTime()
    {

        StopHealthIncreaseCoroutine();

        HealthIncreaseCoroutine = StartCoroutine(change());

    }
    public void StopHealthIncreaseCoroutine()
    {

        if (HealthIncreaseCoroutine != null)
        {
            StopCoroutine(HealthIncreaseCoroutine);
            HealthIncreaseCoroutine = null;
        }
    }
    IEnumerator change()
    {


        while (Value < 100)
        {
          
            yield return new WaitForSeconds(0.5f);


            
                Value = ((Value + 20)>100 ) ? 100: Value+20 ;
            
            //    ChangeHealth(Value, true);
        }

        yield return null;
    }


    public override void Update()
    {
        base.Update();

        if (netIdentity.isServer)
        {


            if(Input.GetKeyDown(KeyCode.T))
            IncreaseHealthOverTime();
        }


    }

}


