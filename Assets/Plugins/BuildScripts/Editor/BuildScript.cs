using System;
using UnityEditor;

public class BuildScript
{
    //[MenuItem("Build/Build All")]
    //public static void BuildAll()
    //{
    //    BuildWindowsGameServer();
    //    BuildWindowsClient();
    //}

    [MenuItem("Build/Build GameServer (Windows)")]
    public static void BuildWindowsGameServer()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/AnyCivilizationGame/Scenes/Lobby.unity", "Assets/AnyCivilizationGame/Scenes/GameScene/GameScene.unity" };
        buildPlayerOptions.locationPathName = "../mobil_loadbalancer/Builds/Windows/GameServer/GameServer.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode | BuildOptions.Development;
        Console.WriteLine("Build Windows GameServer (Windows)...");

        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Console.WriteLine("Builded Succesfully Windows GameServer (Windows).");
    }

    [MenuItem("Build/GameClient(Windows)")]
    public static void BuildWindowsGameClient()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/AnyCivilizationGame/Scenes/Game.unity" };
        buildPlayerOptions.locationPathName = "Builds/Windows/GameClient/GameClient.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode;

        Console.WriteLine("Build Windows GameClient (Windows)...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Console.WriteLine("Builded Succesfully Windows GameClient (Windows).");
    }

    // [MenuItem("Build/Build Server (Linux)")]
    // public static void BuildLinuxServer()
    // {
    //     BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
    //     buildPlayerOptions.scenes = new[] { "Assets/Scenes/Main.unity" };
    //     buildPlayerOptions.locationPathName = "Builds/Linux/Server/Server.x86_64";
    //     buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
    //     buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode;

    //     Console.WriteLine("Building Server (Linux)...");
    //     BuildPipeline.BuildPlayer(buildPlayerOptions);
    //     Console.WriteLine("Built Server (Linux).");
    // }


    //[MenuItem("Build/Build Client (Windows)")]
    //public static void BuildWindowsClient()
    //{
    //    BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
    //    buildPlayerOptions.scenes = new[] { "Assets/Scenes/Lobby.unity" };
    //    buildPlayerOptions.locationPathName = "Builds/Windows/Lobby/Lobby.exe";
    //    buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
    //    buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;

    //    Console.WriteLine("Build Windows Client (Windows)...");
    //    BuildPipeline.BuildPlayer(buildPlayerOptions);
    //    Console.WriteLine("Builded Windows Client (Windows).");
    //}

}