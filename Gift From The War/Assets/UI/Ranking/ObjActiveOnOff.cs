using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjActiveOnOff : MonoBehaviour
{
    [SerializeField]
    private GameObject gameobject;


    public void ActiveOnOff(bool flag)
    {
        gameobject.SetActive(flag);
    }
}
