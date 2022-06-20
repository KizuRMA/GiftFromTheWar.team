using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogManager : BaseEnemyManager
{
    //�q�̔z��ԍ��ƁA�Q�[���I�u�W�F�N�g��MAP���X�g���g�p
    private List<GameObject> objects = new List<GameObject>();
    private Dictionary<string, int> agentTypeIdDict = new Dictionary<string, int>();

    private void Awake()
    {
        for (var i = 0; i < NavMesh.GetSettingsCount(); i++)
        {
            var id = NavMesh.GetSettingsByIndex(i).agentTypeID;
            var name = NavMesh.GetSettingsNameFromID(id);
            agentTypeIdDict.Add(name, id);
        }
    }

    void Update()
    {

        //�q�I�u�W�F�N�g��S�Ď擾����
        GameObject[] _ChildObjects = new GameObject[gameObject.transform.childCount];
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            _ChildObjects[i] = gameObject.transform.GetChild(i).gameObject;
        }

        //�ǐՂ��Ă��錢������
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            DogState _state = _ChildObjects[i].GetComponent<DogState>();
            if (_state == null) continue;

            //���ݎQ�Ƃ��Ă���G���ǐՏ�Ԃł��鎞
            if (_state.IsCurrentState(e_DogState.Tracking) == true)
            {
                //�ǐՏ�Ԃ̓G���Ǘ����Ă���z��Ɍ��ݎQ�Ƃ��Ă���G�������Ă��Ȃ���
                if (objects.Contains(_ChildObjects[i]) == false)
                {
                    //�ǉ�����
                    objects.Add(_ChildObjects[i]);
                }
            }
        }

        //���݊Ǘ����Ă���z��̓G��AgentType��ύX����
        for (int i = 0; i < objects.Count; i++)
        {
            NavMeshAgent navMesh = objects[i].GetComponent<NavMeshAgent>();

            switch (i % 3)
            {
                case 0:
                    navMesh.agentTypeID = agentTypeIdDict["DogAgent"];
                    break;
                case 1:
                    navMesh.agentTypeID = agentTypeIdDict["DogAgent2"];
                    break;
                case 2:
                    break;
            }
        }

        //�Ǘ����Ă���z��Ȃ��ɒǐՂ��Ă��Ȃ��G������ꍇ
        for (int i = 0; i < objects.Count; i++)
        {
            DogState _state = objects[i].GetComponent<DogState>();
            if (_state == null) continue;

            //���ݎQ�Ƃ��Ă���G���ǐՏ�ԂłȂ���
            if (_state.IsCurrentState(e_DogState.Tracking) == false)
            {
                //�G�[�W�F���g�^�C�v��ύX���Ĕz�񂩂�폜����
                objects[i].GetComponent<NavMeshAgent>().agentTypeID = agentTypeIdDict["DogAgent"];
                objects.Remove(objects[i]);
            }
        }
    }
}
