using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;


public class ExecuteManager
{

    public static Process ExecuteCommand(string command)
    {
        string dataPath = Application.dataPath.Replace('/', '\\');
#if UNITY_EDITOR
        UnityEngine.Debug.Log("dataPath" + dataPath);
        string workingDirevtory = Path.Combine(Directory.GetParent(dataPath).FullName, @"Builds\Windows\GameServer\GameServer.exe");
        // string workingDirevtory = $"cmd /k cd {dataPath}/../Builds/Windows/GameServer && GameServer.exe";
#else
        string   workingDirevtory = Path.Combine(Directory.GetParent(Directory.GetParent(dataPath).FullName).FullName, @"GameServer\GameServer.exe");

#endif
        try
        {
            /* UnityEngine.Debug.Log("dataPath" + dataPath);
               UnityEngine.Debug.Log("workingDirevtory" + workingDirevtory);
               workingDirevtory = @"C:\Users\Exop\Documents\Unity\Game Room\Builds\Windows\GameServer\GameServer.exe";
               return Process.Start("cmd.exe", workingDirevtory + " " + command);*/
            ProcessStartInfo ps1 = new ProcessStartInfo(workingDirevtory);
            ps1.Arguments = $"-port {command}";
            return Process.Start(ps1);

        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Run error" + e.ToString()); // or throw new Exception
            return null;
        }

    }


}
