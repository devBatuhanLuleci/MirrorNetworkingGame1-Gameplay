using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterNFTMintPanel : Panel
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
        MainPanelUIManager.Instance.PickCharacterPanelShow();
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
        var mintedCharacterNFT = Instantiate(MintedNFTCharacterPrefab, MintedNFTCharacterPrefab.transform.position, MintedNFTCharacterPrefab.transform.rotation, MintedCharactersGrid)
            .GetComponent<NFTCharacterCard>();
        var nftCharacter = NFTCharacterDatabase.chars.ElementAt(index);
        mintedCharacterNFT.Initialize(nftCharacter);

    }


}
