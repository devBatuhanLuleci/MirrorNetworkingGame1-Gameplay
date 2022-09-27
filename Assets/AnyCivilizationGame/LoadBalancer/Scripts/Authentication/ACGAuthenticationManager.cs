using Mirror;
using System;
using System.Collections.Generic;

namespace ACGAuthentication
{
    public class ACGAuthenticationManager : EventManagerBase
    {

        public override LoadBalancerEvent loadBalancerEvent { get; protected set; } = LoadBalancerEvent.Authentication;

        public ACGAuthenticationManager(LoadBalancer loadBalancer) : base(loadBalancer)
        {
            loadBalancer.AddEventHandler(loadBalancerEvent, this);
        }
        ~ACGAuthenticationManager()
        {
            loadBalancer.RemoveEventHandler(loadBalancerEvent, this);
        }
        internal override Dictionary<byte, Type> initResponseTypes()
        {
            var responseTypes = new Dictionary<byte, Type>();
            responseTypes.Add((byte)RequestType.Login, typeof(LoginEvent));
            responseTypes.Add((byte)RequestType.Create, typeof(RegisterRequest));
            return responseTypes;
        }


    }
}