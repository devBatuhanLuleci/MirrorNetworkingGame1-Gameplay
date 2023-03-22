using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NetworkedGameManager;

public class GemModeNetworkedGameManager : NetworkedGameManager
{

    private NetworkSpawnObjectInInterval NetworkSpawnObjectInInterval;
    //public List<playerData> CollectedCrystals = new List<playerData>();   
    public override void Awake()
    {
        base.Awake();

        if (TryGetComponent<NetworkSpawnObjectInInterval>(out NetworkSpawnObjectInInterval networkSpawnObjectInInterval))
        {

            NetworkSpawnObjectInInterval = networkSpawnObjectInInterval;

        }
    }


    public override void ServerStarted(Dictionary<int, NetworkConnectionToClient> players)
    {
        base.ServerStarted(players);
        NetworkSpawnObjectInInterval?.StartSpawnLoop();
    }

    public void OnGemCollected(int connectionID)
    {
        Debug.Log($"{connectionID} id ");
     //   CollectedCrystals.Add(new playerData() { connID = connectionID, teamTypes= })


        //connectionID
    }

}
[System.Serializable]
struct playerData
{
    public int connID;
    public NetworkIdentity networkIdentity;
    public TeamTypes teamTypes;



}
