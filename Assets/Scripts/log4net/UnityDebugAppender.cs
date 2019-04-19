using log4net.Appender;
using log4net.Core;
using UnityEngine;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
public class UnityDebugAppender : AppenderSkeleton
{

    protected override void Append(LoggingEvent loggingEvent)
    {
        switch (loggingEvent.Level.Name)
        {
            case "DEBUG":
                Debug.Log(RenderLoggingEvent(loggingEvent));
                break;
            case "ERROR":
                Debug.LogError(RenderLoggingEvent(loggingEvent));
                break;
            case "WARNING":
                Debug.LogWarning(RenderLoggingEvent(loggingEvent));
                break;
        }
    }
}
