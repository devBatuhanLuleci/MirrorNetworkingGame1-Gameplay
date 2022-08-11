using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NFTCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    public Image AvatarImage;
    public string NFTAvatarName;
    public string NFTCharacterPrice;
    public TMP_Text NFTCharacterNameText;
    public TMP_Text NFTCharacterPriceText;
    public enum CharacterOwnedStatus { unOwned, Owned };
    public CharacterOwnedStatus characterOwnedStatus;

    public Button GetCharacterNFTButton;
    public TMP_Text OwnedText;
    void Awake()

    {
        GetCharacterNFTButton.onClick.AddListener(GetNFTCharacter);
    }
    public void SetCharacterInfo(string charName, string charPrice=null)
    {
        NFTAvatarName = charName;
        NFTCharacterPrice = charPrice;

        NFTCharacterNameText.text = NFTAvatarName;
        if(charPrice!=null)
        NFTCharacterPriceText.text = NFTCharacterPrice;

    }
    public void GetNFTCharacter()
    {
        PlayerPrefs.SetString(NFTAvatarName, "owned");
        NFTCharacter pickableCharacter = Instantiate(this, ACG_LoginPanelManager.Instance.PickCharacterPanel.GetComponent<CharacterPanelManager>().CharactersGrid) ;
        pickableCharacter.transform.SetAsFirstSibling();
        pickableCharacter.NFTCharacterPriceText.gameObject.SetActive(false);
        pickableCharacter.GetCharacterNFTButton.gameObject.SetActive(false);


        InitilizeStatus();
    }
    public void CheckOwnedThisCharacter()
    {
        if (PlayerPrefs.GetString(NFTAvatarName).Equals("owned"))
        {
            InitilizeStatus();
        }
        else
        {
            SetOwnedStatusAttributes();
        }
    }
    public void InitilizeStatus()
    {
        characterOwnedStatus = CharacterOwnedStatus.Owned;
        SetOwnedStatusAttributes();
    }
    public void SetOwnedStatusAttributes()
    {
        if (IsOwned())
        {
            OwnedText.gameObject.SetActive(true);
            GetCharacterNFTButton.gameObject.SetActive(false);
        }
        else
        {
            GetCharacterNFTButton.gameObject.SetActive(true);
            OwnedText.gameObject.SetActive(false);

        }


    }
    public bool IsOwned()
    {

        return characterOwnedStatus == CharacterOwnedStatus.Owned;
    }

    public void SetMyAvatar(Sprite avatarSprite)
    {
        AvatarImage.sprite = avatarSprite;

    }

}
