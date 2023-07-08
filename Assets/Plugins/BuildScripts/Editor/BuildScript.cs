using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

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
        var parentFolder = "../mobil_loadbalancer/Builds";
        var clientPath = parentFolder + "/Windows/GameClient";
        var serverPath = parentFolder + "/Windows/GameServer";
        var apkPath = parentFolder + "/Android/GameClient";


        Process[] processes = Process.GetProcesses();
        Console.WriteLine($"{processes.Length} processes.Lenght");

        for (int i = 0; i < processes.Length; i++)
        {
            try
            {
                Process process = processes[i];

                string fileName = process.MainModule.FileName;
                Console.WriteLine($"{fileName} is checked");

                string clientPathExe = System.IO.Path.GetFullPath(clientPath + "/GameClient.exe");
                string serverPathExe = System.IO.Path.GetFullPath(serverPath + "/GameServer.exe");
                Console.WriteLine("clientPathExe ***" + clientPathExe);
                Console.WriteLine("serverPathExe ***" + serverPathExe);
                if (fileName == clientPathExe || fileName == serverPathExe)
                {
                    process.Kill();
                    process.WaitForExit();
                    UnityEngine.Debug.Log($"{fileName} is killed");

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

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/AnyCivilizationGame/Scenes/ProductionScenes/Lobby.unity", "Assets/AnyCivilizationGame/Scenes/ProductionScenes/GameScene.unity" },
            locationPathName = clientPath + "/GameClient.exe",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode | BuildOptions.Development
        };
        UnityEngine.Debug.Log("Build Windows GameClient (Windows)...");

        BuildPipeline.BuildPlayer(buildPlayerOptions);
        UnityEngine.Debug.Log("Builded Succesfully Windows GameClient (Windows).");

        if (Directory.Exists(serverPath))
        {
            Directory.Delete(serverPath, true);
        }
        Directory.CreateDirectory(serverPath);

        buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/AnyCivilizationGame/Scenes/ProductionScenes/Lobby.unity", "Assets/AnyCivilizationGame/Scenes/ProductionScenes/GameScene.unity" },
            locationPathName = serverPath + "/GameServer.exe",
            target = BuildTarget.StandaloneWindows64,
            subtarget = (int)StandaloneBuildSubtarget.Server,
            options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode | BuildOptions.Development
        };

        UnityEngine.Debug.Log("Build Windows GameServer (Windows)...");

        BuildPipeline.BuildPlayer(buildPlayerOptions);
        UnityEngine.Debug.Log("Builded Succesfully Windows GameServer (Windows).");


        EditorUtility.RevealInFinder(parentFolder);
        Caching.ClearCache();


    }

    [MenuItem("Build/Build GameServer and GameClient (Windows) and APK")]
    public static void BuildWindowsGameServerAndClientAndAPK()
    {
        var parentFolder = "../mobil_loadbalancer/Builds";
        var clientPath = parentFolder + "/Windows/GameClient";
        var serverPath = parentFolder + "/Windows/GameServer";
        var apkPath = parentFolder + "/Android/GameClient";

        Process[] processes = Process.GetProcesses();
        Console.WriteLine($"{processes.Length} processes.Lenght");

        for (int i = 0; i < processes.Length; i++)
        {
            try
            {
                Process process = processes[i];

                string fileName = process.MainModule.FileName;
                Console.WriteLine($"{fileName} is checked");

                string clientPathExe = System.IO.Path.GetFullPath(clientPath + "/GameClient.exe");
                string serverPathExe = System.IO.Path.GetFullPath(serverPath + "/GameServer.exe");
                Console.WriteLine("clientPathExe ***" + clientPathExe);
                Console.WriteLine("serverPathExe ***" + serverPathExe);
                if (fileName == clientPathExe || fileName == serverPathExe)
                {
                    process.Kill();
                    process.WaitForExit();
                    UnityEngine.Debug.Log($"{fileName} is killed");

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

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/AnyCivilizationGame/Scenes/ProductionScenes/Lobby.unity", "Assets/AnyCivilizationGame/Scenes/ProductionScenes/GameScene.unity" },
            locationPathName = clientPath + "/GameClient.exe",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode | BuildOptions.Development
        };
        UnityEngine.Debug.Log("Build Windows GameClient (Windows)...");

        BuildPipeline.BuildPlayer(buildPlayerOptions);
        UnityEngine.Debug.Log("Builded Succesfully Windows GameClient (Windows).");

        if (Directory.Exists(serverPath))
        {
            Directory.Delete(serverPath, true);
        }
        Directory.CreateDirectory(serverPath);

        buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/AnyCivilizationGame/Scenes/ProductionScenes/Lobby.unity", "Assets/AnyCivilizationGame/Scenes/ProductionScenes/GameScene.unity" },
            locationPathName = serverPath + "/GameServer.exe",
            target = BuildTarget.StandaloneWindows64,
            subtarget = (int)StandaloneBuildSubtarget.Server,
            options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode | BuildOptions.Development
        };

        UnityEngine.Debug.Log("Build Windows GameServer (Windows)...");

        BuildPipeline.BuildPlayer(buildPlayerOptions);
        UnityEngine.Debug.Log("Builded Succesfully Windows GameServer (Windows).");


        if (Directory.Exists(apkPath))
        {
            Directory.Delete(apkPath, true);
        }
        Directory.CreateDirectory(apkPath);

        buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/AnyCivilizationGame/Scenes/ProductionScenes/Lobby.unity", "Assets/AnyCivilizationGame/Scenes/ProductionScenes/GameScene.unity" },
            locationPathName = apkPath + "/Warbots.apk",
            target = BuildTarget.Android,
            options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode | BuildOptions.Development

        };
        UnityEngine.Debug.Log("Build APK GameClient (Android)...");

        BuildPipeline.BuildPlayer(buildPlayerOptions);
        UnityEngine.Debug.Log("Builded Succesfully GameClient (Android).");

        EditorUtility.RevealInFinder(parentFolder);
        Caching.ClearCache();


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