using MoralisUnity.Kits.AuthenticationKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoralisStates : MonoBehaviour
{
    public void OnConnected()
    {
        AuthenticationManager.Instance.MoralisOnConnected();
    }
    public void OnDisconnected()
    {
        AuthenticationManager.Instance.MoralisOnDisconnected();

    }
    public void OnStateChanged(AuthenticationKitState state)
    {
        AuthenticationManager.Instance.MoralisOnStateChanged(state);
    }
}
