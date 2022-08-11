using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class playerAbnormalcondition : MonoBehaviour
{
    enum e_Abnormal
    {
        howling,
    }

    struct Abnormal
    {
        public float time;
        public float complateCureTime;
        public bool completeCureFlg;
    }


    float unrivaledTime;
    bool unrivaledFlg;
    Abnormal[] abnormal = new Abnormal[System.Enum.GetValues(typeof(e_Abnormal)).Length];
    [SerializeField] public float life;
    [SerializeField] public PostProcessVolume volume;
    [SerializeField] public UltrasoundTex ultrasoundImage;
    [SerializeField] float cureTime;
    [SerializeField] Material material;

    // Start is called before the first frame update
    void Start()
    {
        unrivaledTime = 2.0f;
        unrivaledFlg = false;
        for (int i = 0; i < abnormal.Length; i++)
        {
            abnormal[i].time = 0;
            abnormal[i].complateCureTime = 0;
            abnormal[i].completeCureFlg = true;
        }

        if (material != null)
        {
            material.SetFloat("_Range", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < abnormal.Length; i++)
        {
            e_Abnormal _abnormal = (e_Abnormal)System.Enum.ToObject(typeof(e_Abnormal), i);

            switch (_abnormal)
            {
                case e_Abnormal.howling:
                    UpdateHowling();
                    break;
            }
        }
    }

    private void UpdateHowling()
    {
        ref Abnormal howling = ref abnormal[(int)e_Abnormal.howling];

        if (volume != null && howling.completeCureFlg == true)
        {
            if (volume.weight > 0.0f)
            {
                float dif = Time.deltaTime * 3.0f;
                volume.weight = Mathf.Max(volume.weight - dif, 0.0f);
                if (volume.weight <= 0.0f)
                {
                    volume.enabled = false;
                }
            }
            return;
        }

        howling.time += Time.deltaTime;

        if (material != null)
        {
            material.SetFloat("_Range", (1 - (howling.time / howling.complateCureTime)) * 0.05f);
        }

        if (howling.time - howling.complateCureTime > 0)
        {
            howling.time = 0;
            howling.completeCureFlg = true;
        }
    }

    public bool IsHowling()
    {
        return abnormal[(int)e_Abnormal.howling].completeCureFlg == false;
    }

    public void AddHowlingAbnormal()
    {
        ref Abnormal howling = ref abnormal[(int)e_Abnormal.howling];

        if (volume != null)
        {
            volume.enabled = true;
            volume.weight = 1.0f;
        }

        if (material != null)
        {
            material.SetFloat("_Range", 0.05f);
        }

        //if (ultrasoundImage != null && howling.completeCureFlg == true)
        if (ultrasoundImage != null && howling.time >= (howling.complateCureTime * 0.2f))
        {
            ultrasoundImage.OnDisplay();
        }

        howling.time = 0;
        howling.complateCureTime = cureTime;
        howling.completeCureFlg = false;
    }

    public void Damage(float _damage)
    {
        if (life < 0 || unrivaledFlg == true) return;

        life -= _damage;
        unrivaledFlg = true;
        StartCoroutine(DamageCoroutine());
        AudioManager.Instance.PlaySE("damage2",vol:0.2f);

        if (life > 0) return;
    }

    private IEnumerator DamageCoroutine()
    {
        yield return new WaitForSeconds(unrivaledTime);
        unrivaledFlg = false;
    }
}
