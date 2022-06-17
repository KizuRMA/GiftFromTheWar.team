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
        //�i�r���b�V���ɂ��I�u�W�F�N�g�̉�]���X�V���Ȃ�
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

        //�|���ꂽ�����������̎�
        if (myController.causeOfDead == BatController.e_CauseOfDead.Explosion)
        {
            DestructionScript dead = game.GetComponent<DestructionScript>();
            dead.ExpBlownAway(myController.hypocenter);
        }

        Destroy(gameObject);
    }
}
