using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : Throwable , INetworkPooledObject
{
    public Action ReturnHandler { get ; set; }

    private void OnTriggerEnter(Collider other)
    {



        if (other.TryGetComponent<PlayerController>(out var otherPlayerController) && isServer )
        {
            if (otherPlayerController.IsLive)
            {
               // Debug.Log($"{otherPlayerController.connectionToClient.connectionId} id ");

                GemModeNetworkedGameManager gemModeNetworkedGameManager = NetworkedGameManager.Instance as GemModeNetworkedGameManager;
                gemModeNetworkedGameManager.OnGemCollected(otherPlayerController.connectionToClient.connectionId);
                //PlayerController OurPlayer =   MatchNetworkManager.Instance.GetPlayerByConnectionID(OwnerConnectionId);

                //   otherPlayerController.ChangeDamageTakenStatus(PlayerController.DamageTakenStatus.IsTakenDamage);
                //otherPlayerController.TakeDamage(damage, otherPlayerController.netIdentity.connectionToClient);

                // OurPlayer.ultimateSkill.IncreaseCurrentUltimateFillAmount(OurPlayer.netIdentity.connectionToClient, damage * 4);
                Debug.Log(" hitted by this man " + OwnerName);
                NetworkServer.UnSpawn(gameObject);
                ReturnHandler();

            }

        }
    }
    public override void OnObjectSpawn()
    {
        Debug.Log("I SPAWNED");
        base.OnObjectSpawn();
    }
    public override void OnArrived()
    {
        base.OnArrived();
    }


}
