using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSE : MonoBehaviour
{
    // Start is called before the first frame update

    public void ClickNormal()
    {
        AudioManager.Instance.PlaySE("����{�^��������14", isLoop: false);
    }
    public void ClickBack()
    {
        AudioManager.Instance.PlaySE("�L�����Z��1", isLoop: false);
    }

    public void ClickGoTitle()
    {
        AudioManager.Instance.PlaySE("����{�^��������10", isLoop: false);
    }

}
