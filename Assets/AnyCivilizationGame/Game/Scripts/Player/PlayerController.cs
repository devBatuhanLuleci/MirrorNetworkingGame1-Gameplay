using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private void Update()
    {

        if (!netIdentity.isLocalPlayer) return;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var move = new Vector2(horizontal, vertical);
        if (move.magnitude > 0.2f)
        {
            MoveCMD(move);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeColorCMD();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowVfxCMD();
        }
    }

    [Command]
    public void MoveCMD(Vector2 move)
    {
        if (!netIdentity.isServer) return;
        transform.Translate(move);
    }

    [Command]
    public void ChangeColorCMD()
    {
        if (!netIdentity.isServer) return;
        Color color = Random.ColorHSV();
        GetComponentInChildren<MeshRenderer>().material.color = color;
    }

    [Command]
    public void ThrowVfxCMD()
    {
        var testVfx = Resources.Load<GameObject>("test");
        if (testVfx == null)
        {
            Debug.LogError("test is null");
        }
        else
        {
            var test = Instantiate(testVfx);
            NetworkServer.Spawn(test);
        }
    }


}
