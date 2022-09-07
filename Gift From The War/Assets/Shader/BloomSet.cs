using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BloomSet : MonoBehaviour
{
    Bloom bloom;

    public bool bloomFlg;
    public bool finishFlg;
    private float speed = 0;
    private float value;

    // Start is called before the first frame update
    void Start()
    {
        bloom = ScriptableObject.CreateInstance<Bloom>();
        bloomFlg = false;
        finishFlg = false;
    }

    void Update()
    {

        if(bloomFlg)
        {
            speed += Time.deltaTime;
            value = Mathf.Lerp(1, 100, speed);

            bloom.enabled.Override(true);
            //変更したい項目に任意の数値を渡す
            bloom.intensity.Override(value);
            bloom.softKnee.Override(1.0f);
            //QuickVolume（PostProcessのレイヤー番号,Priority,効果）
            PostProcessManager.instance.QuickVolume(gameObject.layer, 1, bloom);
        }

        if(value >= 100)
        {
            finishFlg = true;
        }
    }

}
