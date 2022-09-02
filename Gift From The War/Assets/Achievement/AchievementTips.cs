using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementTips : MonoBehaviour
{
   // public GameObject target;
    public RectTransform target;
    public Vector2 offset;
    private bool onceFlg = false;
    

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<RectTransform>().position = new Vector3(target.position.x + offset.x, target.position.y + offset.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(!onceFlg)
        {
            onceFlg = true;
            this.GetComponent<RectTransform>().position = new Vector3(target.position.x + offset.x, target.position.y + offset.y, 0);
        }
    }
}
