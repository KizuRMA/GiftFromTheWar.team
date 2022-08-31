using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PopUpOut))]
public class EditorPopUpOut : Editor
{
    private bool foldout1;
    private bool foldout2;
    

    public override void OnInspectorGUI()
    {
        var popupout = target as PopUpOut;

        popupout.forceOpen = EditorGUILayout.ToggleLeft("　シーン遷移時強制ポップアップ", popupout.forceOpen);
        
        popupout.PanelObject = (GameObject)EditorGUILayout.ObjectField("オブジェクト", popupout.PanelObject, typeof(GameObject), true);
        popupout.PanelCanvasGroup = (CanvasGroup)EditorGUILayout.ObjectField("キャンバスグループ", popupout.PanelCanvasGroup, typeof(CanvasGroup), true);
        popupout.ImageTransform = (RectTransform)EditorGUILayout.ObjectField("トランスフォーム", popupout.ImageTransform, typeof(RectTransform), true);

        foldout1 = EditorGUILayout.Foldout(foldout1, "ポップアップ");

        if (foldout1)
        {
            popupout.upDuration = EditorGUILayout.FloatField("アニメーション時間", popupout.upDuration);
            popupout.PopUpEaseType = (PopUpOut.EaseType)EditorGUILayout.EnumPopup("イージングタイプ", popupout.PopUpEaseType);
        }

        foldout2 = EditorGUILayout.Foldout(foldout2, "ポップアウト");

        if (foldout2)
        {
            popupout.outDuration = EditorGUILayout.FloatField("アニメーション時間", popupout.outDuration);
            popupout.PopOutEaseType = (PopUpOut.EaseType)EditorGUILayout.EnumPopup("イージングタイプ", popupout.PopOutEaseType);
        }

    }
}

