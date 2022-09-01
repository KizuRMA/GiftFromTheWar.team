using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjActiveOnOff : MonoBehaviour
{
    [SerializeField]
    private GameObject gameobject;

    void Update()
    {
        //�f�o�b�O�p
        if (Input.GetKeyDown(KeyCode.O))
        {
            gameobject.SetActive(false);
        }
    }

    public void ActiveOnOff(bool flag)
    {
        gameobject.SetActive(flag);
    }
}
