using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour
{
    [SerializeField]
    private GameObject Panel;

    private CanvasGroup panelCanvasGroup;

    [System.NonSerialized] public bool showFlg = false;
    private void Awake()
    {
        panelCanvasGroup = Panel.GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Panel.SetActive(false);
        panelCanvasGroup.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
       if(ScenarioManager.Instance.endFlg)
        {
            Panel.SetActive(false);
        }

       if(showFlg)
        {
            CursorManager.Instance.SetCursorLock(false);
        }
    }

    public void ShowPanel()
    {
        Panel.SetActive(true);
        showFlg = true;
        SystemSetting.Instance.Pause(SystemSetting.e_PauseType.Document);
    }

    public void ClosePanel()
    {
        SystemSetting.Instance.Resume();

        Panel.SetActive(false);
        showFlg = false;
    }
}
