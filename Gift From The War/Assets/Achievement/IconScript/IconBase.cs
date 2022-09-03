using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconBase : MonoBehaviour
{
    [SerializeField] protected float alphaThin;
    

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void DownAlpha()
    {
        if (this.GetComponent<Image>().color.a < alphaThin) return;
        this.GetComponent<Image>().color += new Color(0, 0, 0, -alphaThin);
    }
    
    public void UpAlpha()
    {
        this.GetComponent<Image>().color += new Color(0, 0, 0, 1.0f);
    }
}
