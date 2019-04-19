using System.IO;
using log4net.Config;
using UnityEngine;

public class LoggingConfiguration
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ConfigureLogging()
    {


        XmlConfigurator.Configure(new FileInfo($"{Application.dataPath}/log4net.xml"));
    }
}
