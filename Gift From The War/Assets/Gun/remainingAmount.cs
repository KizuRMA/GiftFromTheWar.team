using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class remainingAmount : MonoBehaviour
{
    [SerializeField] GameObject amount;
    [SerializeField] GameObject amount2;
    [SerializeField] Transform trans;
    [SerializeField] MoveWindGun windGun;
    [SerializeField] Material mat1;
    [SerializeField] Material mat2;
    Vector3 firstPos;

    [SerializeField] float upSpeed;

    private float nowRemainingAmount;
    private float useAmount;
    private float allRemainingAmount;
    [SerializeField] private float amountMax;
    [SerializeField] private float amountMin;

    // Start is called before the first frame update
    void Start()
    {
        firstPos = transform.position;
        allRemainingAmount = amountMax - amountMin;
    }

    // Update is called once per frame
    void Update()
    {
        if (useAmount == 0)
        {
            trans.localPosition += new Vector3(0, 0, -(upSpeed * allRemainingAmount - allRemainingAmount) * Time.deltaTime);
        }
        else
        {
            trans.localPosition += new Vector3(0, 0, (useAmount * allRemainingAmount - allRemainingAmount) * Time.deltaTime);
            Material[] tmp = gameObject.GetComponent<Renderer>().materials;
            tmp[0] = mat1;
            amount2.GetComponent<Renderer>().materials = tmp;
        }

        if (trans.localPosition.z > amountMax)
        {
            trans.localPosition = new Vector3(0, 0, amountMax);

            Material[] tmp = gameObject.GetComponent<Renderer>().materials;
            tmp[0] = mat2;
            amount2.GetComponent<Renderer>().materials = tmp;
        }
        else if (trans.localPosition.z < amountMin)
        {
            trans.localPosition = new Vector3(0, 0, amountMin);
        }

        nowRemainingAmount = 1.0f - (amountMax - trans.localPosition.z) / allRemainingAmount;
    }

    public float GetSetNowAmount
    {
        get { return nowRemainingAmount; }
        set { useAmount = value; }
    }
}
