
using System;

[Serializable]
public class CharacterAttribute
{
    public string Name;
    public int Level;
    public float[] Levels;
    public float Value { get => Levels[Level]; }
 
    public CharacterAttribute()
    {

    }
    public CharacterAttribute(string name)
    {
        this.Name = name;
        Level = 0;
    }
    public CharacterAttribute(string name, float[] levels) : this(name)
    {
        this.Levels = levels;
    }
    public CharacterAttribute(string name, float[] levels, int level) : this(name, levels)
    {
        this.Level = level;
    }
}