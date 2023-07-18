using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildScript
{

    const string parentFolder = "../mobil_loadbalancer/Builds";
    const string clientPath = parentFolder + "/Windows/GameClient";
    const string serverPath = parentFolder + "/Windows/GameServer";
    const string apkPath = parentFolder + "/Android/GameClient";
    static string[] scensArray => new string[] { "Assets/AnyCivilizationGame/Scenes/ProductionScenes/Lobby.unity", "Assets/AnyCivilizationGame/Scenes/ProductionScenes/GameScene.unity" };
    static BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
   [MenuItem("Build/Build GameServer (Windows)")]
    public static void BuildWindowsGameServer()
    {
        ClosingExes();
        BuildServerExe();
        EditorUtility.RevealInFinder(parentFolder);
    }

    [MenuItem("Build/Build GameServer and GameClient (Windows)")]
    public static void BuildWindowsGameServerAndClient()
    {
        ClosingExes();
        BuildServerExe();
        BuildClientExe();
        EditorUtility.RevealInFinder(parentFolder);
    }


    [MenuItem("Build/Build GameServer and GameClient (Windows) and APK")]
    public static void BuildWindowsGameServerAndClientAndAPK()
    {
        ClosingExes();
        BuildServerExe();
        BuildClientExe();
        BuildClientAPK();
        EditorUtility.RevealInFinder(parentFolder);
    }
    #region Functions
    private static void ClosingExes()
    {
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
    }
    private static void BuildServerExe()
    {
        if (Directory.Exists(serverPath))
        {
            Directory.Delete(serverPath, true);
        }
        Directory.CreateDirectory(serverPath);

        buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scensArray,
            locationPathName = serverPath + "/GameServer.exe",
            target = BuildTarget.StandaloneWindows64,
            subtarget = (int)StandaloneBuildSubtarget.Server,
            options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode | BuildOptions.Development
        };

        UnityEngine.Debug.Log("Build Windows GameServer (Windows)...");

        BuildPipeline.BuildPlayer(buildPlayerOptions);
        UnityEngine.Debug.Log("Builded Succesfully Windows GameServer (Windows).");
    }
    private static void BuildClientExe()
    {
        if (Directory.Exists(clientPath))
        {
            Directory.Delete(clientPath, true);
        }
        Directory.CreateDirectory(clientPath);

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scensArray,
            locationPathName = clientPath + "/GameClient.exe",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode | BuildOptions.Development
        };
        UnityEngine.Debug.Log("Build Windows GameClient (Windows)...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        UnityEngine.Debug.Log("Builded Succesfully Windows GameClient (Windows).");
    }
    private static void BuildClientAPK()
    {
        if (Directory.Exists(apkPath))
        {
            Directory.Delete(apkPath, true);
        }
        Directory.CreateDirectory(apkPath);

        buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scensArray,
            locationPathName = apkPath + "/Warbots.apk",
            target = BuildTarget.Android,
            options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode | BuildOptions.Development

        };
        UnityEngine.Debug.Log("Build APK GameClient (Android)...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        UnityEngine.Debug.Log("Builded Succesfully GameClient (Android).");
    }
    #endregion
}