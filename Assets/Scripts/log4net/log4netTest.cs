using System.Collections;
using System.Collections.Generic;
using log4net;
using UnityEngine;

public class log4netTest : MonoBehaviour
{
    private static readonly ILog Log = LogManager.GetLogger(typeof(log4netTest));

    void Awake()
    {
        Log.Debug("log4netTest Awake");
    }

    void Start()
    {
        Log.Debug("log4netTest Start");
        Log.Error("Error");
    }

    void Update()
    {
        Log.Debug("log4netTest Update");
    }

    void OnEnable()
    {
        Log.Debug("log4netTest OnEnable");
    }

    void OnDisable()
    {
        Log.Debug("log4netTest OnDisable");
    }
}
