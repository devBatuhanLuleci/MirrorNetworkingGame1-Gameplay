using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ACGAuthentication
{
    public abstract class EventManagerBase
    {
        internal LoadBalancer loadBalancer;
        internal Dictionary<byte, Type> responsesByType = new Dictionary<byte, Type>();
        public abstract LoadBalancerEvent loadBalancerEvent { get; protected set; }

        public EventManagerBase(LoadBalancer loadBalancer)
        {
            this.loadBalancer = loadBalancer;
            responsesByType = initResponseTypes();
        }
        #region Handlers
        //internal abstract void HandleServerEvents(NetworkReader reader);
        internal abstract Dictionary<byte, Type> initResponseTypes();

        #endregion
        internal virtual void HandleServerEvents(NetworkReader reader)
        {
            // read message type sequens
            var requestType = reader.ReadByte();
            // get reader by requestType
            Type type = responsesByType[requestType];
            // Invoke generic method for type
            reader.ReadIEvent(type)?.Invoke(this);

        }
        public virtual void SendClientRequestToServer(IEvent request)
        {
            Type type = request.GetType();
            var writer = new NetworkWriter();
            writer.WriteByte((byte)loadBalancerEvent);
            if (responsesByType.TryGetKey(request.GetType(), out var key))
            {
                writer.WriteByte((byte)key);
                writer.WriteIEvent(type, request);
                loadBalancer.ClientSend(writer.ToArraySegment());
            }
            else
            {
                throw new Exception("Request type is not found!");
            }
            //writer.WriteByte((byte)request.requestType);
            //request.Write(writer);
            //writer.WriteLoginRequest(request);
            //loadBalancer.ClientSend(writer.ToArraySegment());
        }



    }
}