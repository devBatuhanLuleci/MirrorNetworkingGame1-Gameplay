using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class JsonDataAdaptor : DataAdaptor
{


    public Dictionary<string, CharacterData> CharacterData { get; set; }
    public Profile ProfileData { get; set; }

    private string pathCharacterData = "Data/CharacterData";
    private string pathProfile = "Data/ProfileData";
    public Task<Dictionary<string, CharacterData>> GetDataAsync()
    {
        try
        {
            var json = Resources.Load<TextAsset>(pathCharacterData).text;
            if (json == null) return null;
            CharacterData = JsonConvert.DeserializeObject<Dictionary<string, CharacterData>>(json);
            return Task.FromResult(CharacterData);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return null;
        }
    }

    public Task UpdateDataAsync(Dictionary<string, CharacterData> value)
    {
        CharacterData = value;
        return Task.CompletedTask;
    }

    public Task<Profile> GetProfileAsync()
    {
        try
        {
            var json = Resources.Load<TextAsset>(pathProfile).text;
            if (json == null) return null;
            ProfileData = JsonConvert.DeserializeObject<Profile>(json);
            return Task.FromResult(ProfileData);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return null;
        }
    }

    public Task UpdateProfileAsync(Profile value)
    {
        ProfileData = value;
        return Task.CompletedTask;
    }
}
