using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TeamIndicatorHandler : MonoBehaviour
{
    public enum TeamIndicatorType { Me,Ally, Enemy }
    public List<TeamIndicatorStats> teamIndicatorStats = new List<TeamIndicatorStats>();

    [SerializeField]
    private SpriteRenderer teamIndicatorRenderer;

   

    public void ChangeTeamIndicatorType(string teamType)
    {
        //  int index = teamIndicatorStats.FindIndex(t => t.teamType.Equals(teamType));
        foreach (var item in teamIndicatorStats)
        {
            Debug.Log(item.teamType.ToString());

        }
        var element = teamIndicatorStats.Find(t => t.teamType.ToString().Equals(teamType));
      //  teamIndicatorStats.
      if(element!=null)
        {

        teamIndicatorRenderer.color = element.Indicator_Color;

        }


    }
}

[System.Serializable]
public class TeamIndicatorStats
{
    public TeamIndicatorHandler.TeamIndicatorType teamType;
    public Color Indicator_Color;

}
