using UnityEngine;

public class Exit : MonoBehaviour
{
    //�Q�[���I��:�{�^������Ăяo��
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
    }
}
