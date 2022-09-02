using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeziKunPrefabs : MonoBehaviour
{
    [SerializeField]
    private GameObject neziKun=null;

    public Transform neziKunGroup;

    // ¶¬‚·‚éƒvƒŒƒnƒu‚Ì”
    public int prefabNum;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < prefabNum; i++)
        {
            Instantiate(neziKun, neziKunGroup) ;
            neziKun.transform.position = new Vector3(i,0,i/2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
