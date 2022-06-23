using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpenNavmeshBake : MonoBehaviour
{
    private bool openFlg = false;

    // Start is called before the first frame update
    void Start()
    {
        openFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (openFlg == true) return;
        CheckObstacle();
    }

    private void CheckObstacle()
    {
        //親オブジェクトが存在する場合は早期リターン
        if (transform.parent != null) return;
        NavMeshObstacle obstacle = transform.GetComponent<NavMeshObstacle>();
        obstacle.carving = false;
    }
}
