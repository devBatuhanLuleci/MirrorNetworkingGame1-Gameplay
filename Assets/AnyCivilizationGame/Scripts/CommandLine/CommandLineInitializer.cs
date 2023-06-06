using Oddworm.Framework;
using UnityEngine;

public class CommandLineInitializer : Singleton<CommandLineInitializer>
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    static void LoadCommandLine()
    {
        // Use commandline options passed to the application
        var text = System.Environment.CommandLine + "\n";

        // Initialize the CommandLine
        CommandLine.Init(text);
    }
    public static void LoadCommandLine(string args)
    {
        // Use commandline options passed to the application
        var text = System.Environment.CommandLine + args + "\n";

        // Initialize the CommandLine
        CommandLine.Init(text);
    }

}