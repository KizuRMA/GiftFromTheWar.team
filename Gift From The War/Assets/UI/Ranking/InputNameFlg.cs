using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputNameFlg : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool nameFlg = false;

    public void OnFlg()
    {
        nameFlg = true;
    }

    public void OffFlg()
    {
        nameFlg = false;
    }
}
