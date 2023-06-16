using System;
using System.Diagnostics;
using System.IO;
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
        buildPlayerOptions.scenes = new[] { "Assets/AnyCivilizationGame/Scenes/ProductionScenes/Lobby.unity", "Assets/AnyCivilizationGame/Scenes/ProductionScenes/GameScene.unity" };
        buildPlayerOptions.locationPathName = "../mobil_loadbalancer/Builds/Windows/GameServer/GameServer.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode | BuildOptions.Development;
        Console.WriteLine("Build Windows GameServer (Windows)...");

        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Console.WriteLine("Builded Succesfully Windows GameServer (Windows).");
    }

    [MenuItem("Build/Build GameServer and GameClient (Windows)")]
    public static void BuildWindowsGameServerAndClient()
    {
        var clientPath = "../mobil_loadbalancer/Builds/Windows/GameClient";
        var serverPath = "../mobil_loadbalancer/Builds/Windows/GameServer";

        Process[] processes = Process.GetProcesses();
        Console.WriteLine($"{processes.Length} processes.Lenght");

        for (int i = 0; i < processes.Length; i++)
        {
            try
            {
                Process process = processes[i];

                string fileName = process.MainModule.FileName;
                Console.WriteLine($"{fileName} is checked");

                string clientPathExe = Path.GetFullPath(clientPath + "/GameClient.exe");
                string serverPathExe = Path.GetFullPath(serverPath + "/GameServer.exe");
                Console.WriteLine("clientPathExe ***" + clientPathExe);
                Console.WriteLine("serverPathExe ***" + serverPathExe);
                if (fileName == clientPathExe || fileName == serverPathExe)
                {
                    process.Kill();
                    process.WaitForExit();
                    Console.WriteLine($"{fileName} is killed");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        if (Directory.Exists(clientPath))
        {
            Directory.Delete(clientPath, true);
        }
        Directory.CreateDirectory(clientPath);
        if (Directory.Exists(serverPath))
        {
            Directory.Delete(serverPath, true);
        }
        Directory.CreateDirectory(serverPath);


        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/AnyCivilizationGame/Scenes/ProductionScenes/Lobby.unity", "Assets/AnyCivilizationGame/Scenes/ProductionScenes/GameScene.unity" };
        buildPlayerOptions.locationPathName = clientPath + "/GameClient.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode | BuildOptions.Development;
        Console.WriteLine("Build Windows GameClient (Windows)...");

        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Console.WriteLine("Builded Succesfully Windows GameClient (Windows).");



        buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/AnyCivilizationGame/Scenes/ProductionScenes/Lobby.unity", "Assets/AnyCivilizationGame/Scenes/ProductionScenes/GameScene.unity" };
        buildPlayerOptions.locationPathName = serverPath + "/GameServer.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode | BuildOptions.Development;
        Console.WriteLine("Build Windows GameServer (Windows)...");

        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Console.WriteLine("Builded Succesfully Windows GameServer (Windows).");
    }

    //[MenuItem("Build/GameClient(Windows)")]
    //public static void BuildWindowsGameClient()
    //{
    //    BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
    //    buildPlayerOptions.scenes = new[] { "Assets/AnyCivilizationGame/Scenes/Game.unity" };
    //    buildPlayerOptions.locationPathName = "Builds/Windows/GameClient/GameClient.exe";
    //    buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
    //    buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode;

    //    Console.WriteLine("Build Windows GameClient (Windows)...");
    //    BuildPipeline.BuildPlayer(buildPlayerOptions);
    //    Console.WriteLine("Builded Succesfully Windows GameClient (Windows).");
    //}

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