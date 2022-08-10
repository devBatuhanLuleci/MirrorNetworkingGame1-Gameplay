using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HttpClient
{
    public interface IHttpRequest
    {
        public abstract string Url { get; }
    }
}
