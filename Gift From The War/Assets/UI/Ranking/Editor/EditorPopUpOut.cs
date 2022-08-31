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


            foldout1 = EditorGUILayout.Foldout(foldout1, "ポップアップ");

            if (foldout1)
            {
                var popupout= target as PopUpOut;

                popupout.upDuration = EditorGUILayout.FloatField("経過時間", popupout.upDuration);
                popupout.PopUpEaseType = (PopUpOut.EaseType)EditorGUILayout.EnumPopup("イージングタイプ", popupout.PopUpEaseType);
             
            }

        foldout2 = EditorGUILayout.Foldout(foldout2, "ポップアウト");

        if (foldout2)
        {
            var popupout = target as PopUpOut;

            popupout.outDuration = EditorGUILayout.FloatField("経過時間", popupout.outDuration);
            popupout.PopOutEaseType = (PopUpOut.EaseType)EditorGUILayout.EnumPopup("イージングタイプ", popupout.PopOutEaseType);

        }


    }
    }

