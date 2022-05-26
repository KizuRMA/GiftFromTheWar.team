using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogMoveState : MonoBehaviour
{
    [SerializeField]WayPoint wayPoint;
    [SerializeField]GameObject player;
    [SerializeField]float Speed;
    NavMeshAgent agent;
    int nextIndex;

    // Start is called before the first frame update
    void Start()
    {
        nextIndex = 0;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;
    }

    // Update is called once per frame
    void Update()
    {
       StartCoroutine(TargetCoroutine());
        //agent.destination = player.transform.position;
    }

    private IEnumerator TargetCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        agent.destination = player.transform.position;
    }
}
