﻿using Mirror;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class NetworkedTimer:NetworkBehaviour
{

    [SerializeField, SyncVar(hook = nameof(OnCountdownChangedSync))]
    protected int countdown = 10; // başlangıç sayısı
    public int initialCountDownValue=10;
    private float timer = 1f; // saniyede bir azaltmak için zamanlayıcı

    private bool isCountingDown = false;


    public UnityEvent onTimeFinishedAction;
    public static Action OnTimeFinished;
    //private void Start()
    //{
    //    if (isServer)
    //    {
    //        isCountingDown = true;
    //    }
    //}
    private void Awake()
    {
        if (!isServer) { return; }


        countdown = initialCountDownValue;
    }

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

        ResetTimer();
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
    public void ResetTimer()
    {
        countdown = initialCountDownValue;
       

    }
    public virtual void OnCountDownFinished()
    {
        
        isCountingDown = false;
        onTimeFinishedAction.Invoke();
        OnTimeFinished?.Invoke();
        CloseApplication();
    }

    private async void CloseApplication()
    {
        await Task.Delay(3000);
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
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