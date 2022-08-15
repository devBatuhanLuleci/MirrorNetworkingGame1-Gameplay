using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTCharacterDatabase : MonoBehaviour
{
    // Start is called before the first frame update
    public List<NFTCharacter> chars = new List<NFTCharacter>();
    [System.Serializable]
   public class NFTCharacter
    {
        public string name;
        public string price;
        public Sprite AvatarSprite;
        public bool owned=false;

    }
}
