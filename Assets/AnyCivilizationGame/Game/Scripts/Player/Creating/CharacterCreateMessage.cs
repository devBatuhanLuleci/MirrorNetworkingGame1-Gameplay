using Mirror;
public struct CharacterCreateMessage : NetworkMessage
{
    public string name;

    public static CharacterCreateMessage Default
    {
        get
        {
            return new CharacterCreateMessage { name = "DefaultCharacter" };
        }
    }
}
