using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : SingletonMonoBehaviour<CursorManager>
{
    //カーソルロック
    public bool cursorLock = false;
    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (cursorLock)
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else if (!cursorLock)
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
