using Assets.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


    public abstract class PostRequestBase : IPostRequest
    {
        public abstract string Url { get; }
        public abstract WWWForm ToForm();
    }

