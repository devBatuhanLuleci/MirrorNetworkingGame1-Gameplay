using static NetworkedGameManager;

[System.Serializable]
struct GemData
{
    public int connID;
    //public NetworkIdentity networkIdentity;
    public TeamTypes teamTypes;


    public GemData(int connID/*, NetworkIdentity netIdentity*/, TeamTypes myTeam)
    {
        this.connID = connID;
        //  networkIdentity = netIdentity;
        teamTypes = myTeam;
    }

}
