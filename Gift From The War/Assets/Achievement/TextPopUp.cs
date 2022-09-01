using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextPopUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _helpMessage;

    [SerializeField] private Text _text;
    private Vector2 POPUP_OFFSET = new Vector2(0, 10);
    public void OnPointerEnter(PointerEventData eventData)
    {
        _text.transform.position = eventData.position;
        _text.text = _helpMessage;
        _text.transform.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.transform.gameObject.SetActive(false);
    }
}
