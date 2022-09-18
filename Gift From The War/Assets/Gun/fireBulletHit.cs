using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBulletHit : MonoBehaviour
{
    private Transform trans;
    public GameObject targetImageObj = null;

    [SerializeField] private float scaleSpeed;
    [SerializeField] private float scaleFirst;
    [SerializeField] private float scaleMax;
    private float nowScale;

    //オブジェクトの移動
    [SerializeField] private float movePowerBase;
    [SerializeField] private float movePowerMax;
    [SerializeField] private float movePowerMin;
    [SerializeField] private float hitTime;
    private float movePower;
    private float time;


    void Start()
    {
        time = 0;
        trans = transform;
        nowScale = scaleFirst;
        AudioManager.Instance.PlaySE("BulletExp", gameObject, isLoop: false);
    }

    void Update()
    {
        time += Time.deltaTime;

        nowScale += scaleSpeed * Time.deltaTime;

        nowScale = nowScale > scaleMax ? scaleMax : nowScale;

        trans.localScale = new Vector3(nowScale, nowScale, nowScale);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (time >= hitTime) return;

        if (other.gameObject.tag == "Bat" || other.gameObject.tag == "Dog1")
        {
            var enemyInter = other.GetComponent<EnemyInterface>();

            if (enemyInter != null)
            {
                enemyInter.ExpDamage(1,transform.position);

                if (targetImageObj != null)
                {
                    targetImageObj.GetComponent<TargetSetting>().HitAnime();
                }
            }
        }

        if (other.gameObject.tag == "Boss")
        {
            var enemyInter = other.GetComponent<EnemyInterface>();

            if (enemyInter != null)
            {
                enemyInter.ExpDamage(1, transform.position);

                if (targetImageObj != null)
                {
                    targetImageObj.GetComponent<TargetSetting>().HitAnime(new Color(0.9921569f, 0.6321112f, 0, 1));
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody RD = other.attachedRigidbody;

        if (RD == null || RD.isKinematic) return;   //動かないオブジェクトだったら処理しない

        Vector3 moveVec = RD.gameObject.transform.position - trans.position;    //移動方向を算出

        //移動する力の計算
        movePower = movePowerBase - moveVec.magnitude;
        movePower = movePower > movePowerMax ? movePowerMax : movePower;
        movePower = movePower < movePowerMin ? movePowerMin : movePower;

        moveVec = moveVec.normalized * movePower;   //ベクトル生成

        RD.AddForce(moveVec, ForceMode.Impulse);
    }
}
