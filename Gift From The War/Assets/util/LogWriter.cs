using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System;
using Cysharp.Threading.Tasks;

public class LogWriter
{
    public readonly string logPath;

    public LogWriter(string _logPath, CancellationToken cancellation)
    {
        logPath = $"{_logPath}/{DateTimeOffset.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.log";
        Application.logMessageReceivedThreaded += OnLogMessageReceivedThreaded;
        LogWriteLoop(cancellation).Forget();
    }

    private readonly Dictionary<int, string> LogLevelCase = new Dictionary<int, string>()
    {
        {(int) LogType.Error,"E" },
        {(int) LogType.Assert,"A" },
        {(int) LogType.Warning,"W" },
        {(int) LogType.Log,"L" },
        {(int) LogType.Exception,"EX" }
    };

    private const string DateTimeFormat = "yyyy/MM/dd HH;mm:ss";

    private void OnLogMessageReceivedThreaded(string condition,string stacktrace, LogType type)
    {
        logQueue.Add($"{DateTimeOffset.Now.ToString(DateTimeFormat)} [{LogLevelCase[(int)type]}] {condition}");
    }

    private readonly BlockingCollection<string> logQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());

    private async UniTaskVoid LogWriteLoop(CancellationToken cancellationToken)
    {
        StreamWriter writer = null;
        try 
        {
            var parentDir = new FileInfo(logPath).Directory;
            if(parentDir != null && !parentDir.Exists)
            {
                parentDir.Create();
            }
            writer = new StreamWriter(logPath, true, Encoding.UTF8);
            while(true)
            {
                if(cancellationToken.IsCancellationRequested) break;

                await UniTask.SwitchToThreadPool();
                var log = logQueue.Take(cancellationToken);
                await writer.WriteLineAsync(log);
            }
        }
        finally
        {
            if(writer != null)
            {
                await writer.FlushAsync();
                writer.Close();
                writer.Dispose();
            }
        }
    }
}
