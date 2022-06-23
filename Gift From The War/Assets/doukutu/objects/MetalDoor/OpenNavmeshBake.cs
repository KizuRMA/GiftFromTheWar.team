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
        //���^���h�A�̐e�̐e����X�e�[�W�����擾����
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
        //�e�I�u�W�F�N�g�����݂���ꍇ�͑������^�[��
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

    private void NavBake()  //�i�r���b�V�����x�C�N������
    {
        foreach (var _navs in navs)
        {
            _navs.BuildNavMesh();
        }
    }
}
