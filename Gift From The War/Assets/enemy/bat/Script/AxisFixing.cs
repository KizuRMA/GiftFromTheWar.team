using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisFixing : MonoBehaviour
{
    [SerializeField] bool x, y, z;
    // Start is called before the first frame update

    void Awake()
    {
    }

    void Update()
    {
        Vector3 _parent = transform.eulerAngles;
        //èCê≥â”èä
        gameObject.transform.rotation = Quaternion.Euler(_parent);
    }
}
