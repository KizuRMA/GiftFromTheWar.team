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


            foldout1 = EditorGUILayout.Foldout(foldout1, "�|�b�v�A�b�v");

            if (foldout1)
            {
                var popupout= target as PopUpOut;

                popupout.upDuration = EditorGUILayout.FloatField("�o�ߎ���", popupout.upDuration);
                popupout.PopUpEaseType = (PopUpOut.EaseType)EditorGUILayout.EnumPopup("�C�[�W���O�^�C�v", popupout.PopUpEaseType);
             
            }

        foldout2 = EditorGUILayout.Foldout(foldout2, "�|�b�v�A�E�g");

        if (foldout2)
        {
            var popupout = target as PopUpOut;

            popupout.outDuration = EditorGUILayout.FloatField("�o�ߎ���", popupout.outDuration);
            popupout.PopOutEaseType = (PopUpOut.EaseType)EditorGUILayout.EnumPopup("�C�[�W���O�^�C�v", popupout.PopOutEaseType);

        }


    }
    }

