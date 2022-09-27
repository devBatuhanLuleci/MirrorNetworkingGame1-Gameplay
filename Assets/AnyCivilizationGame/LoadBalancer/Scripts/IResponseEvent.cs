using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResponseEvent : IEvent
{
    /// <summary>
    /// Write client logics
    /// </summary>
    /// <param name="authenticationManager"></param>
    public void Invoke(EventManagerBase eventManagerBase);
}
