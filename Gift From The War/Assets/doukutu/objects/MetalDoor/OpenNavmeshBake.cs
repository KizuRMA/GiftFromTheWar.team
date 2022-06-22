using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpenNavmeshBake : MonoBehaviour
{
    [SerializeField] GameObject leftDoor = null;
    [SerializeField] GameObject rigitDoor = null;
    private MetalDoorUseInfo info;
    private NavMeshSurface[] navs;


    private void Awake()
    {
        info = transform.parent.GetComponent<MetalDoorUseInfo>();
        navs = info.navSurfaces;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckBake(ref leftDoor);
        CheckBake(ref rigitDoor);
    }

    private void CheckBake(ref GameObject _game)
    {
        if (_game == null) return;

        //親オブジェクトが存在する場合は早期リターン
        if (_game.transform.parent != null) return;

        AddModifier(_game);
        NavBake();
        _game = null;
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
