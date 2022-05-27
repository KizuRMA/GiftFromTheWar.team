using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EmiTest : MonoBehaviour
{
    // Inspector
    [SerializeField] private Material material0;
    [SerializeField] private Material material1;
    [SerializeField] SkinnedMeshRenderer original;


    public void EmiON()
    {
        Material[] mats = original.materials;
        mats[0] = material0;
        mats[1] = material1;
        original.materials = mats;
    }
    
    public void EmiOFF()
    {
        Material[] mats = original.materials;
        mats[0] = material0;
        mats[1] = null;
        original.materials = mats;
    }

}
 
