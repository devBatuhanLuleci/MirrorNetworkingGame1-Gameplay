using log4net.Config;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Bootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ConfigureLogging()
    {
        string dataPath = Application.dataPath.Replace('/', '\\');
        string workingDirevtory = Path.Combine(Directory.GetParent(dataPath).FullName, @"Configs");
        var path = Path.Combine(workingDirevtory, "log4net.xml");
        if (!File.Exists(path)) { Directory.CreateDirectory(workingDirevtory); }
        Debug.Log(path);
        XmlConfigurator.Configure(new FileInfo(path));
    }
}
