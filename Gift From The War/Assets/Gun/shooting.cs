using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class shooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shotSpeed;
    [SerializeField] private int bulletMax;
    private int shotCount;
    private float shotInterval;

    private void Start()
    {
        shotCount = bulletMax;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            shotInterval += 1;

            if (shotInterval % 5 == 0 && shotCount > 0)
            {
                shotCount -= 1;

                GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));
                Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                bulletRb.AddForce(transform.forward * shotSpeed);

                //射撃されてから3秒後に銃弾のオブジェクトを破壊する.
                Destroy(bullet, 3.0f);
            }

        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            shotCount = bulletMax;
        }

    }
}
