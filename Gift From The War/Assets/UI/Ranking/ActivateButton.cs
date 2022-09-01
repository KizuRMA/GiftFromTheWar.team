using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActivateButton : MonoBehaviour
{

	[SerializeField] private GameObject firstSelect;
	[SerializeField] private CanvasGroup canvasGroup;
	
	//private CanvasGroup canvasGroup;

	public void ButtonPushed()
	{
		//firstSelect.image.color = buttonColor;
		Debug.Log("ƒ{ƒ^ƒ“‚ª‰Ÿ‚³‚ê‚Ü‚µ‚½");
		
	}

    void Start()
    {
		EventSystem.current.SetSelectedGameObject(firstSelect);
	
	}

 //   void OnEnable()
	//{
	//	if (canvasGroup == null)
	//	{
	//		canvasGroup = GetComponent<CanvasGroup>();
	//	}
	//}

	public void ActivateOrNotActivate(bool flag)
	{
		canvasGroup.interactable = flag;
		if (flag)
		{
			EventSystem.current.SetSelectedGameObject(firstSelect);
		}
	}
}
