using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpenNavmeshBake : MonoBehaviour
{
    private MetalDoorUseInfo info;
    private NavMeshSurface[] navs;
    private bool openFlg = false;

    private void Awake()
    {
        //メタルドアの親の親からステージ情報を取得する
        info = transform.parent.transform.parent.GetComponent<MetalDoorUseInfo>();
    }

    // Start is called before the first frame update
    void Start()
    {
        navs = info.navSurfaces;
        openFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (openFlg == true) return;
        CheckBake();
    }

    private void CheckBake()
    {
        //親オブジェクトが存在する場合は早期リターン
        if (transform.parent != null) return;

        AddModifier(gameObject);
        NavBake();
        openFlg = true;
    }

    private void AddModifier(GameObject _game)
    {
        NavMeshModifier modifier = _game.AddComponent<NavMeshModifier>();
        modifier.ignoreFromBuild = true;
    }

    private void NavBake()  //ナビメッシュをベイクし直す
    {
        foreach (var _navs in navs)
        {
            _navs.BuildNavMesh();
        }
    }
}
