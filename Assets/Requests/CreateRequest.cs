using Assets.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CreateRequest : IPostRequest
{
    public string Url => "Auth/create";

    public string moralisId;
    public string email;

    public CreateRequest(string moralisId, string email)
    {
        this.moralisId = moralisId;
        this.email = email;
    }

    public WWWForm ToForm()
    {
        WWWForm form = new WWWForm();
        form.AddField("moralisId", moralisId);
        form.AddField("email", email);

        return form;
    }
}

