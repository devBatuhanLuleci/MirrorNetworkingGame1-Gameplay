using System;
using UnityEngine;

public interface IPooledObject 
{
   
  
   
}
public interface INetworkPooledObject : IPooledObject
{
    public Action ReturnHandler { get; set; }
}

