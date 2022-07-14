using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatManager : BaseEnemyManager
{
    protected override bool IsChasing()
    {
        //�q�I�u�W�F�N�g��S�Ď擾����
        GameObject[] _ChildObjects = new GameObject[gameObject.transform.childCount];
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            _ChildObjects[i] = gameObject.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            BatController _state = _ChildObjects[i].GetComponent<BatController>();
            if (_state == null) continue;

            if (_state.IsChasing() == true)
            {
                return true;
            }
        }
        return false;
    }
}
