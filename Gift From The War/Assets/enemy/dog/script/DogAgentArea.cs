using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogAgentArea : MonoBehaviour
{
  
    [SerializeField] List<string> useAgentNames;
    [SerializeField] NavMeshModifierVolume volume;
    BoxCollider boxCollider;
    GameObject baseDogObject = null;
    GameObject stage;

    NavMeshSurface[] navMesh;
    Dictionary<string,NavMeshSurface> navMeshes = new Dictionary<string,NavMeshSurface>();

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = transform.GetComponent<BoxCollider>();
        stage = transform.parent.GetComponent<DogAgentAreaRoot>().stage;
        //�X�e�[�W�ɂ���S�Ă�NavMeshSurface�̏����擾����
        navMesh = stage.GetComponents<NavMeshSurface>();

        for (int i = 0; i < navMesh.Length; i++)
        {
            //NavMeshSurface����Agent�̖��O���擾����
            string _name = NavMesh.GetSettingsNameFromID(navMesh[i].agentTypeID);

            //�g�p����Agent�̖��O��S�ĎQ�Ƃ��邽�߂̃��[�v
            foreach (string str in useAgentNames)
            {
                //���O����v���镨���������ꍇ��Dictionary�z��ɒǉ�����
                if (str == _name)
                {
                    navMeshes.Add(str, navMesh[i]);
                }
            }
        }

        //Collider�̃T�C�Y��NavmeshVolume�ɍ��킹��
        boxCollider.center = volume.center;
        boxCollider.size = volume.size;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Dog1" || baseDogObject != null) return;

        baseDogObject = other.gameObject;
        UsingDogArea _dogArea = baseDogObject.GetComponent<UsingDogArea>();
        NavMeshAgent agent = baseDogObject.GetComponent<NavMeshAgent>();

        //��ɂ��錢�����ɑ��̃G���A����ɂ��Ă���ꍇ
        if (_dogArea.area != null)
        {
            //�O�Ɋ�ɂ��Ă���DogAgentArea�����Z�b�g����
            _dogArea.area.ResetAffectsAgentType();
            _dogArea.area = this;
        }
        else
        {
            _dogArea.area = this;
        }


        string _agentName = NavMesh.GetSettingsNameFromID(agent.agentTypeID);

        foreach (string str in navMeshes.Keys)
        {
            if (_agentName != str)
            {
                volume.AddAffectsAgentType(navMeshes[str].agentTypeID);
                navMeshes[str].BuildNavMesh();
            }
        }
    }

    public void ResetAffectsAgentType()
    {
        volume.AllRemoveAffectsAgentType();
        baseDogObject = null;
    }
}
