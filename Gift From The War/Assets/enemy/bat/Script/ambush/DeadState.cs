using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeadState : BaseState
{
    private NavMeshAgent agent;
    [SerializeField] private GameObject BoxPrefab;
    [SerializeField] public LayerMask raycastLayerMask;

    // Start is called before the first frame update

    private void Awake()
    {
        myController = GetComponent<BatController>();
        agent = GetComponent<NavMeshAgent>();
        CurrentState = (int)BatController.e_State.dead;
    }

    public override void Start()
    {
        //ナビメッシュによるオブジェクトの回転を更新しない
        agent.isStopped = true;
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        agent.updatePosition = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        GameObject game = Instantiate(BoxPrefab, transform.position, transform.rotation);

        //倒された原因が爆発の時
        if (myController.causeOfDead == BatController.e_CauseOfDead.Explosion)
        {
            DestructionScript dead = game.GetComponent<DestructionScript>();
            dead.ExpBlownAway(myController.hypocenter);
        }

        Destroy(gameObject);
    }
}
