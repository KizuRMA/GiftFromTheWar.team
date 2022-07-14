using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSE : MonoBehaviour
{
    // Start is called before the first frame update

    public void ClickNormal()
    {
        AudioManager.Instance.PlaySE("決定ボタンを押す14", isLoop: false);
    }
    public void ClickBack()
    {
        AudioManager.Instance.PlaySE("キャンセル1", isLoop: false);
    }

    public void ClickGoTitle()
    {
        AudioManager.Instance.PlaySE("決定ボタンを押す10", isLoop: false);
    }

}
