using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogSearchState : MonoBehaviour
{
    [SerializeField]WayPoint wayPoint;
    [SerializeField]GameObject player;
    [SerializeField]float Speed;
    NavMeshAgent agent;
    Vector3[] targetPos;
    const int arrayMax = 60;
    bool canSetFlg;
    int nextIndex;

    // Start is called before the first frame update
    void Start()
    {
        canSetFlg = true;
        targetPos = new Vector3[arrayMax];
        for (int i = 0; i < arrayMax; i++)
        {
            targetPos[i] = player.transform.position;
        }

        nextIndex = 0;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSetFlg == true)
        {
            StartCoroutine(TargetCoroutine());
        }
        //agent.destination = player.transform.position;
    }

    private IEnumerator TargetCoroutine()
    {
        canSetFlg = false;
        yield return new WaitForSeconds(0.1f);

        for (int i = arrayMax - 1; i > 0; i--)
        {
            targetPos[i] = targetPos[i - 1];
        }

        targetPos[0] = player.transform.position;
        agent.destination = targetPos[arrayMax - 1];
        canSetFlg = true;
    }
}
