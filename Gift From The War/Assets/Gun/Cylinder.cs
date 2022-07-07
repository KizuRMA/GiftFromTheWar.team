using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
    [SerializeField] bulletChange bullet;
    [SerializeField] public GameObject windAmmo;
    [SerializeField] public GameObject magnetAmmo;
    [SerializeField] public GameObject fireAmmo;
    [SerializeField] float rotateSpeed;
    float targetAngle;
    public bool nowChanging;


    void Update()
    {
        targetAngle = (int)bullet.nowBulletType * 120.0f;

        Quaternion target = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y,targetAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,target, rotateSpeed * Time.deltaTime);
        Vector3 dif = transform.rotation.eulerAngles - target.eulerAngles;

        if (dif.magnitude <= 0.01f)
        {
            nowChanging = false;
        }
        else
        {
            nowChanging = true;
        }
    }
}
