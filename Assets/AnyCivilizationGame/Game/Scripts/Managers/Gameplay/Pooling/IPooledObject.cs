using System;
using UnityEngine;

public interface IPooledObject 
{
   
    void OnObjectSpawn(float rotAngle);

   
}
public interface INetworkPooledObject : IPooledObject
{
    public Action ReturnHandler { get; set; }
}

