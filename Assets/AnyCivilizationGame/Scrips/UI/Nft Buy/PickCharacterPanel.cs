using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PickCharacterPanel : Panel
{
    public Button AddNFTCharacterButton;
    public Transform CharactersGrid;
    public GameObject CharacterNFTPrefab;
    public NFTCharacterDatabase NFTCharacterDatabase;
    private List<string> AvatarNames = new List<string>() { "Luka", "Alice", "Jack", "Venom" };
    private void Awake()
    {
        AddNFTCharacterButton.onClick.AddListener(OnClick_AddNFTCharacterButton);

        for (int i = 0; i < NFTCharacterDatabase.chars.Count; i++)
        {

            if (PlayerPrefs.GetString(NFTCharacterDatabase.chars.ElementAt(i).name).Equals("owned"))
            {
                NFTCharacterCard pickableCharacter = Instantiate(CharacterNFTPrefab, CharactersGrid).GetComponent<NFTCharacterCard>();

                pickableCharacter.SetCharacterInfo(NFTCharacterDatabase.chars.ElementAt(i).name);
                pickableCharacter.SetMyAvatar(NFTCharacterDatabase.chars.ElementAt(i).AvatarSprite);

                pickableCharacter.transform.SetAsFirstSibling();
                pickableCharacter.NFTCharacterPriceText.gameObject.SetActive(false);
                pickableCharacter.GetCharacterNFTButton.gameObject.SetActive(false);
            }
        }


    }
    public void OnClick_AddNFTCharacterButton()
    {
        ACG_LoginPanelManager.Instance.Switch_To_Character_NFTMint_Panel();
        Debug.Log("NFT Character added.");
    }
}
