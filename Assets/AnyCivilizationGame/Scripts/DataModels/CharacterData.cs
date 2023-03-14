using System.Collections.Generic;
using System;

[Serializable]
public class CharacterData
{
    public int Id;
    public string Name;
    public Dictionary<string, CharacterAttribute> Attributes;

    public CharacterData()
    {
        Attributes = new Dictionary<string, CharacterAttribute>();
    }
    public CharacterData(string name) : this()
    {
        this.Name = name;
    }


    internal void AddSkill(CharacterAttribute speed)
    {
        this.Attributes.Add(speed.Name, speed);
    }
}

