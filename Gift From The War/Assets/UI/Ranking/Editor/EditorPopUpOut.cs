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

        popupout.forceOpen = EditorGUILayout.ToggleLeft("�@�V�[���J�ڎ������|�b�v�A�b�v", popupout.forceOpen);
        
        popupout.PanelObject = (GameObject)EditorGUILayout.ObjectField("�I�u�W�F�N�g", popupout.PanelObject, typeof(GameObject), true);
        popupout.PanelCanvasGroup = (CanvasGroup)EditorGUILayout.ObjectField("�L�����o�X�O���[�v", popupout.PanelCanvasGroup, typeof(CanvasGroup), true);
        popupout.ImageTransform = (RectTransform)EditorGUILayout.ObjectField("�g�����X�t�H�[��", popupout.ImageTransform, typeof(RectTransform), true);

        foldout1 = EditorGUILayout.Foldout(foldout1, "�|�b�v�A�b�v");

        if (foldout1)
        {
            popupout.upDuration = EditorGUILayout.FloatField("�A�j���[�V��������", popupout.upDuration);
            popupout.PopUpEaseType = (PopUpOut.EaseType)EditorGUILayout.EnumPopup("�C�[�W���O�^�C�v", popupout.PopUpEaseType);
        }

        foldout2 = EditorGUILayout.Foldout(foldout2, "�|�b�v�A�E�g");

        if (foldout2)
        {
            popupout.outDuration = EditorGUILayout.FloatField("�A�j���[�V��������", popupout.outDuration);
            popupout.PopOutEaseType = (PopUpOut.EaseType)EditorGUILayout.EnumPopup("�C�[�W���O�^�C�v", popupout.PopOutEaseType);
        }

    }
}

