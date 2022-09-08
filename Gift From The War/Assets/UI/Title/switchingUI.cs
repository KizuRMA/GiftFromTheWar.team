using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SwitchingUI : MonoBehaviour
{
    public List<CanvasGroup> canvasGroups;
    public float durationTime;
    public float firstDelay;
    private int nowIndex;

    void Start()
    {
        nowIndex = 0;

        foreach (var _canvasGroups in canvasGroups)
        {
            _canvasGroups.gameObject.SetActive(false);
            _canvasGroups.alpha = 0.0f;
            _canvasGroups.blocksRaycasts = false;
        }

    }

    public void AppFirstUI()
    {
        nowIndex = 0;
        //Å‰‚ÌUI‚ð•\Ž¦‚³‚¹‚é
        canvasGroups[nowIndex].gameObject.SetActive(true);
        canvasGroups[nowIndex].DOFade(endValue: 1f, duration: durationTime).SetEase(Ease.OutCubic).SetDelay(firstDelay).OnComplete(() => OnRayCast()).Play();
    }
    public void NextIndex()
    {
        //¡‚ÌUI‚ð–³Œø‚É‚µAŽŸ‚ÌUI‚ð—LŒøA•\Ž¦‚³‚¹‚é
        canvasGroups[nowIndex].gameObject.SetActive(false);
        nowIndex += 1;
        canvasGroups[nowIndex].gameObject.SetActive(true);
        canvasGroups[nowIndex].DOFade(endValue: 1f, duration: durationTime).SetEase(Ease.OutCubic).OnComplete(() => OnRayCast()).Play();
    }
    private void OnRayCast()
    {
        canvasGroups[nowIndex].blocksRaycasts = true;
    }

    public void CloseUI()
    {
        //Œ»Ý•\Ž¦‚³‚ê‚Ä‚¢‚éUI‚ð•Â‚¶‚é
        canvasGroups[nowIndex].blocksRaycasts = false;//Œë‘€ì—\–h‚Ìˆ×æ‚ÉØ‚é
        canvasGroups[nowIndex].DOFade(endValue: 0f, duration: durationTime).SetEase(Ease.OutCubic).Play();
        canvasGroups[nowIndex].gameObject.SetActive(false);

    }
    public void NextUI()
    {

        if (canvasGroups.Count > nowIndex + 1)
        {
            canvasGroups[nowIndex].blocksRaycasts = false;//Œë‘€ì—\–h‚Ìˆ×æ‚ÉØ‚é
            canvasGroups[nowIndex].DOFade(endValue: 0f, duration: durationTime).SetEase(Ease.OutCubic).OnComplete(() => NextIndex()).Play();
        }

    }
}
