using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var move = new Vector2(horizontal, vertical);
        if (move.magnitude > 0.2f)
        {
            MoveCMD(move);
        }
    }

    [Command]
    public void MoveCMD(Vector2 move)
    {
        if (!netIdentity.isServer) return;
        transform.Translate(move);
    }
}
