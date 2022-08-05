using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PostRequest : PostRequestBase
{
    public override string Url => throw new NotImplementedException();

    public override WWWForm ToForm()
    {
        return new WWWForm().Serialize(this);
    }
}

