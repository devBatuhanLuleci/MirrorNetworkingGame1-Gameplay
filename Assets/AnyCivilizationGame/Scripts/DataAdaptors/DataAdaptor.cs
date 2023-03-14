
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

public interface DataAdaptor
{
    public Dictionary<string, CharacterData> CharacterData { get; set; }
    public Profile ProfileData { get; set; }

    #region AsyncMethods
    /// <summary>
    /// Collect data from destination
    /// </summary>
    /// <returns></returns>
    public Task<Dictionary<string, CharacterData>> GetDataAsync();
    /// <summary>
    /// Update data on destination
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Task UpdateDataAsync(Dictionary<string, CharacterData> value);


    #region Profile
    /// <summary>
    /// Collect data from destination
    /// </summary>
    /// <returns></returns>
    public Task<Profile> GetProfileAsync();
    /// <summary>
    /// Update data on destination
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Task UpdateProfileAsync(Profile value);
    #endregion
    #endregion
}

