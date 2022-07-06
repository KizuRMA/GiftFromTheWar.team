using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
    [SerializeField] bulletChange bullet;
    float targetAngle;

    void Update()
    {
        targetAngle = (int)bullet.nowBulletType * 120.0f;

        Quaternion target = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y,targetAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,target,100 * Time.deltaTime);
    }
}
