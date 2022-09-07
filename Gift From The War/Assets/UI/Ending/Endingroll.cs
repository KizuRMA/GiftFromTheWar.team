using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Endingroll : MonoBehaviour
{
    Vector3 Staffrollposition;
    public RectTransform rectTransform;
    public float Endpos;

    [SerializeField]
    private float defaultSpeed;

    private float speed;

    [SerializeField]
    private bool spaceSkip = true;

    private bool onceFlg = false;

    void Start()
    {
        Staffrollposition = rectTransform.anchoredPosition;
        speed = defaultSpeed;
        onceFlg = false;
    }

    void Update()
    {

        if (rectTransform.anchoredPosition.y < Endpos)
        {
            if(spaceSkip)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    speed = defaultSpeed * 3.0f;
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    speed = defaultSpeed;
                }
            }
            else
            {
                speed = defaultSpeed;
            }
            

            Staffrollposition.y += speed * Time.deltaTime;
            rectTransform.anchoredPosition = Staffrollposition;
        }
        else
        {
            if (!onceFlg && Input.GetKey(KeyCode.Space))
            {
                //Debug.Log("‚Æ‚¨‚½‚æ");
                CursorManager.Instance.cursorLock = false;
                StartCoroutine(LoadManager.Instance.LoadScene("Scenes/TitleScene"));
                //SceneManager.LoadScene("Scenes/GameTitleScene");
                onceFlg = true;
            }
        }

    }
}