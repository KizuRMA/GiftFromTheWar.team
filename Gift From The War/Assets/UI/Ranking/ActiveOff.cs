using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActiveOff : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;


    public void ButtonPushed()
    {
        if(canvasGroup.interactable == false)
        {
            canvasGroup.interactable = true;
        }
        else
        {
            canvasGroup.interactable = false;
        }
        
    }
}
