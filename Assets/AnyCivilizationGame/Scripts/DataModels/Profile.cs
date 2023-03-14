using System.Collections.Generic;

public class Profile
{
    public string UserName { get; set; }
    public Dictionary<string, CharacterProfileData> Characters { get; set; }
}
