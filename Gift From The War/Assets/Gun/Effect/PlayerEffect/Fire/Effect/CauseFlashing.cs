using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauseFlashing : MonoBehaviour
{
    [SerializeField] private GameObject cause;
    [SerializeField] private float beforeFlashTime;
    private float flashOnTime;
    [SerializeField]private float flashOffTime;
    private bool flashFlg;

    void Start()
    {
        flashOnTime = beforeFlashTime;
        cause.SetActive(true);
        flashFlg = true;
    }

    void Update()
    {
        if(flashFlg)
        {
            StartCoroutine(ActiveOff());
        }
        else
        {
            StartCoroutine(ActiveOn());
        }
    }

    private IEnumerator ActiveOn()
    {
        yield return new WaitForSeconds(flashOffTime);

        cause.SetActive(true);

        flashFlg = true;
    }

    private IEnumerator ActiveOff()
    {
        yield return new WaitForSeconds(flashOnTime);

        cause.SetActive(false);

        flashOnTime /= 2.0f ;
        flashFlg = false;
    }
}
