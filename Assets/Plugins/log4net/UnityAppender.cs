using log4net.Appender;
using log4net.Core;
using UnityEngine;

public class UnityAppender : FileAppender
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
        }
        base.Append(loggingEvent);

    }
}
