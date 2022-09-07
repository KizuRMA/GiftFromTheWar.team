using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class BloomSet : MonoBehaviour
{
    Bloom bloom;

    // Start is called before the first frame update
    void Start()
    {
        
        bloom = ScriptableObject.CreateInstance<Bloom>();
    }

    void Update()
    {

        //���N���b�N
        if (Input.GetMouseButton(0))
        {
            set();
        }
        //�E�N���b�N
        if (Input.GetMouseButton(1))
        {
            setback();
        }
    }
    //�����i���j�֐�
    private void set()
    {
        //��������OK
        bloom.enabled.Override(true);
        //�ύX���������ڂɔC�ӂ̐��l��n��
        bloom.intensity.Override(100f);
        //QuickVolume�iPostProcess�̃��C���[�ԍ�,Priority,���ʁj
        PostProcessManager.instance.QuickVolume(gameObject.layer, 1, bloom);
    }
    //�����i��j�֐��i���j
    private void setback()
    {
        bloom.enabled.Override(true);
        bloom.intensity.Override(1f);
        PostProcessManager.instance.QuickVolume(gameObject.layer, 1, bloom);
    }
}
