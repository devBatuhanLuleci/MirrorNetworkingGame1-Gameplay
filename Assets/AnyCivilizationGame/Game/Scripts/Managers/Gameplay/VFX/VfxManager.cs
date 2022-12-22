using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxManager : NetworkBehaviour
{
    public static VfxManager instance;
    public NetworkIdentity NetworkIdentity { get; private set; }
    private void Awake()
    {
        NetworkIdentity = GetComponent<NetworkIdentity>();
        instance = this; 
    }
      

    //[ClientRpc]
    //public void NetworkInstantiateRPC(string prefab)
    //{
    //    var prefabObject = Resources.Load<GameObject>(prefab);
    //    Instantiate(prefabObject);
    //}


}
