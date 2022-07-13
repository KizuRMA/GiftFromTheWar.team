using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : SingletonMonoBehaviour<CursorManager>
{
    //カーソルロック
    public bool cursorLock = false;
    public Texture2D handCursor;
    private void Start()
    {
        cursorLock = false;
        DontDestroyOnLoad(this);
        ChangeCursor();
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




    void ChangeCursor()
    {
        //Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto);
    }
}
