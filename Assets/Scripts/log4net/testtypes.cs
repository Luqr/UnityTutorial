using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using log4net;
public class testtypes : MonoBehaviour
{
    private ILog _log;
    // Use this for initialization
    void Start()
    {
        var logfilename = Application.dataPath + "/log4net.config.xml";
        //Debug.Log(logfilename);
        //return;
        var file = new System.IO.FileInfo(logfilename);
        if (file == null)
        {
            Debug.LogError("no config:" + logfilename);
            return;
        }

        log4net.Config.XmlConfigurator.Configure(file);
        _log = LogManager.GetLogger("Debug");


        _log.Debug("哈哈哈哈哈哈");
        _log.Debug("12333333");
        _log.Debug(this);
        Debug.Log("finish log");

    }

}
