using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BloomSet : MonoBehaviour
{
    Bloom bloom;
    Bloom defaultBloom;
    PostProcessVolume postProcessVolume;

    public bool bloomFlg;
    public bool finishFlg;
    private float speed = 0;
    private float value;

    // Start is called before the first frame update
    void Start()
    {
        bloomFlg = false;
        finishFlg = false;

        //PostProcessVolumeを取得
        postProcessVolume = GetComponent<PostProcessVolume>();
        
        //デフォルトの設定を保存
        defaultBloom = postProcessVolume.profile.GetSetting<Bloom>();
    }

    void Update()
    {

        if(bloomFlg)
        {
            speed += Time.deltaTime;
            value = Mathf.Lerp(1, 100, speed);

            //PostProcessVolumeを取得
            postProcessVolume = GetComponent<PostProcessVolume>();
            //PostProcessVolumeのprofileからBloomの設定を取得
            bloom = postProcessVolume.profile.GetSetting<Bloom>();

            bloom.intensity.value = value;
            bloom.softKnee.value = 1.0f;
        }

        if (value >= 100)
        {
            finishFlg = true;
            bloomFlg = false;

            bloom = defaultBloom;

        }
    }

}
