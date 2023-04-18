using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class NetworkedTimer:NetworkBehaviour
{

    [SerializeField, SyncVar(hook = nameof(OnCountdownChangedSync))]
    protected int countdown = 10; // başlangıç sayısı

    private float timer = 1f; // saniyede bir azaltmak için zamanlayıcı

    private bool isCountingDown = false;


    public UnityEvent onTimeFinishedAction;
    //private void Start()
    //{
    //    if (isServer)
    //    {
    //        isCountingDown = true;
    //    }
    //}

    private void Update()
    {
        if (!isServer || !isCountingDown) return;

        timer += Time.deltaTime;
        if (timer > 1f)
        {
            timer -= 1f;
            DecreaseCountdown();
        }
    }

    public void StartCountDown()
    {
        if (!isServer) return;


        isCountingDown = true;
    }

    private void DecreaseCountdown()
    {
        if (countdown <= 0)
        {
            OnCountDownFinished();
            return;
        }

        countdown--;
    }

    public virtual void OnCountDownFinished()
    {
        
        isCountingDown = false;
        onTimeFinishedAction.Invoke();
    }

    public virtual void OnCountdownChangedSync(int oldCountdown, int newCountdown)
    {
    //    Debug.Log($"Countdown changed from {oldCountdown} to {newCountdown}");

        if (newCountdown <= 0)
        {
            Debug.Log("Countdown finished");
            isCountingDown = false;
            return;
        }

        countdown = newCountdown;

        if (isServer)
        {
            isCountingDown = true;
        }
    }

}