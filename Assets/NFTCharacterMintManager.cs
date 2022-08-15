using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NFTCharacterMintManager : MonoBehaviour
{
    public Transform MintedCharactersGrid;
    public GameObject MintedNFTCharacterPrefab;
    public Button BackToPickCharacterPanelButton;

    private NFTCharacterDatabase NFTCharacterDatabase;

    private void Awake()
    {
        BackToPickCharacterPanelButton.onClick.AddListener(OnClick_BackToPickCharacterPanelButton);
        NFTCharacterDatabase = GetComponent<NFTCharacterDatabase>();
    }
    public void OnClick_BackToPickCharacterPanelButton()
    {
        ACG_LoginPanelManager.Instance.Switch_To_Pick_Character_Panel_From_NFTMint();
    }
    private void OnEnable()
    {
    }
    private void Start()
    {
        FillMintedCharacters();
        
    }
    public void FillMintedCharacters()
    {
        for (int i = 0; i < 4; i++)
        {
            MintCharacter(i);

        }

    }
    public void MintCharacter(int index)
    {
       GameObject mintedCharacterNFT= Instantiate(MintedNFTCharacterPrefab, MintedNFTCharacterPrefab.transform.position, MintedNFTCharacterPrefab.transform.rotation, MintedCharactersGrid);
        mintedCharacterNFT.GetComponent<NFTCharacter>().SetMyAvatar(NFTCharacterDatabase.chars.ElementAt(index).AvatarSprite);
        mintedCharacterNFT.GetComponent<NFTCharacter>().SetCharacterInfo(NFTCharacterDatabase.chars.ElementAt(index).name, NFTCharacterDatabase.chars.ElementAt(index).price);
        mintedCharacterNFT.GetComponent<NFTCharacter>().CheckOwnedThisCharacter();
    }
    
   



}
