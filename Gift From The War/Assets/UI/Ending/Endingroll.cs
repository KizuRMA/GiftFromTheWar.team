using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endingroll : MonoBehaviour
{
    Vector3 Staffrollposition;
    public RectTransform rectTransform;
    public float Endpos;
    [SerializeField]
    private float speed;

    [SerializeField]
    private bool spaceSkip = true;


    // Start is called before the first frame update
    void Start()
    {
        Staffrollposition = rectTransform.anchoredPosition;


    }

    // Update is called once per frame
    void Update()
    {

        if (rectTransform.anchoredPosition.y < Endpos)
        {
            

            if(spaceSkip)//スペースキーでスキップ可能な場合
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Staffrollposition.y += speed * 2.5f;
                }
                else
                {
                    Staffrollposition.y += speed;
                }
            }
            else
            {
                Staffrollposition.y += speed;
            }
            
            rectTransform.anchoredPosition = Staffrollposition;
        }

    }
}