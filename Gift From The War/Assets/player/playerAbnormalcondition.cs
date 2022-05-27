using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]public float life { set; get; }

    // Start is called before the first frame update
    void Start()
    {
        unrivaledTime = 2.0f;
        unrivaledFlg = false;
        life = 3;
        for (int i = 0; i < abnormal.Length; i++)
        {
            abnormal[i].time = 0;
            abnormal[i].complateCureTime = 0;
            abnormal[i].completeCureFlg = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i  = 0; i < abnormal.Length; i++)
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

        if (howling.completeCureFlg == true) return;

        howling.time += Time.deltaTime;
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

        howling.time = 0;
        howling.complateCureTime = 10;
        howling.completeCureFlg = false;
    }

    public void Damage(float _damage)
    {
        if (life < 0 || unrivaledFlg == true) return;

        life -= _damage;
        unrivaledFlg = true;
        StartCoroutine(DamageCoroutine());
        if (life > 0) return;
    }

    private IEnumerator DamageCoroutine()
    {
        yield return new WaitForSeconds(unrivaledTime);
        unrivaledFlg = false;
    }
}
