using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogWriterOpen : MonoBehaviour
{
    void Start()
    {
        string path = "log";
        var logWriter = new LogWriter(path, this.GetCancellationTokenOnDestroy());

        DontDestroyOnLoad(this);
    }

    void Update()
    {
    }
}
