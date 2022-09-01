using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BaseStatus : ScriptableObject
{
    //　ボタンの名前
    [SerializeField]
    private string buttonName = "";

    [SerializeField]
    private string loadFileName = "";

    public void SetButtonName(string buttonName)
    {
        this.buttonName = buttonName;
    }

    public string GetButtonName()
    {
        return buttonName;
    }

    public void SetFileName(string fileName)
    {
        this.loadFileName = fileName;
    }

    public string GetFileName()
    {
        return loadFileName;
    }
}
