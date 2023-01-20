using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : Panel
{
    public void SelectCharacter(string name)
    {
        var msg = new ReplanceCharacterMessage { name = name };
        NetworkClient.Send(msg);
        GameUIManager.Instance.CharacterSlected();
        Close();
    }
}
