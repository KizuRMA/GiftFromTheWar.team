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

        //左クリック
        if (Input.GetMouseButton(0))
        {
            set();
        }
        //右クリック
        if (Input.GetMouseButton(1))
        {
            setback();
        }
    }
    //発光（強）関数
    private void set()
    {
        //書き換えOK
        bloom.enabled.Override(true);
        //変更したい項目に任意の数値を渡す
        bloom.intensity.Override(100f);
        //QuickVolume（PostProcessのレイヤー番号,Priority,効果）
        PostProcessManager.instance.QuickVolume(gameObject.layer, 1, bloom);
    }
    //発光（弱）関数（略）
    private void setback()
    {
        bloom.enabled.Override(true);
        bloom.intensity.Override(1f);
        PostProcessManager.instance.QuickVolume(gameObject.layer, 1, bloom);
    }
}
