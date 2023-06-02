using log4net;
using Mirror;
using System;
using System.Collections.Generic;

namespace ACGAuthentication
{
    public class ACGAuthenticationManager : EventManagerBase
    {

        public override LoadBalancerEvent loadBalancerEvent { get; protected set; } = LoadBalancerEvent.Authentication;
        public static ILog log = LogManager.GetLogger(typeof(ACGAuthenticationManager));

        public ACGAuthenticationManager(LoadBalancer loadBalancer) : base(loadBalancer)
        {
            loadBalancer.AddEventHandler(loadBalancerEvent, this);
        }
        ~ACGAuthenticationManager()
        {
            loadBalancer.RemoveEventHandler(loadBalancerEvent, this);
        }
        public void Debug(string msg)
        {
            log.Debug(msg);

        }
        internal override Dictionary<byte, Type> initResponseTypes()
        {
            var responseTypes = new Dictionary<byte, Type>();
            responseTypes.Add((byte)AuthenticationEvent.Login, typeof(LoginEvent));
            responseTypes.Add((byte)AuthenticationEvent.Create, typeof(RegisterRequest));
            responseTypes.Add((byte)AuthenticationEvent.LoginResultEvent, typeof(LoginResultEvent));

            return responseTypes;
        }


    }
}