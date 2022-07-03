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
        //ステージにある全てのNavMeshSurfaceの情報を取得する
        navMesh = stage.GetComponents<NavMeshSurface>();

        for (int i = 0; i < navMesh.Length; i++)
        {
            //NavMeshSurfaceからAgentの名前を取得する
            string _name = NavMesh.GetSettingsNameFromID(navMesh[i].agentTypeID);

            //使用するAgentの名前を全て参照するためのループ
            foreach (string str in useAgentNames)
            {
                //名前が一致する物があった場合はDictionary配列に追加する
                if (str == _name)
                {
                    navMeshes.Add(str, navMesh[i]);
                }
            }
        }

        //ColliderのサイズをNavmeshVolumeに合わせる
        boxCollider.center = volume.center;
        boxCollider.size = volume.size;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Dog1" || baseDogObject != null) return;

        baseDogObject = other.gameObject;
        UsingDogArea _dogArea = baseDogObject.GetComponent<UsingDogArea>();
        NavMeshAgent agent = baseDogObject.GetComponent<NavMeshAgent>();

        //基準にする犬が既に他のエリアを基準にしている場合
        if (_dogArea.area != null)
        {
            //前に基準にしていたDogAgentAreaをリセットする
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
