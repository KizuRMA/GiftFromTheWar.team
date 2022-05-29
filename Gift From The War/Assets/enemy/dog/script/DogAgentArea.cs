using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogAgentArea : MonoBehaviour
{
    [SerializeField] GameObject dog;
    [SerializeField] GameObject stage;
    [SerializeField] List<string> useAgentNames;

    NavMeshSurface[] navMesh;
    Dictionary<string,NavMeshSurface> navMeshes = new Dictionary<string,NavMeshSurface>();


    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = dog.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        //犬と当たったときのみ処理を行
        Debug.Log(other.gameObject.tag);

        if (other.gameObject.tag != "Dog1") return;

        GameObject dogObject = other.gameObject;
        NavMeshAgent agent = dogObject.GetComponent<NavMeshAgent>();
        string _agentName = NavMesh.GetSettingsNameFromID(agent.agentTypeID);

        Debug.Log(11111111111111);

        foreach (string str in navMeshes.Keys)
        {
            if (_agentName != str)
            {
                navMeshes[str].BuildNavMesh();
            }
        }
    }
}
