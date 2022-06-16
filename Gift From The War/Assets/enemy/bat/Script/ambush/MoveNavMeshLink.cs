using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveNavMeshLink : MonoBehaviour
{
   
    [SerializeField]private NavMeshAgent agent;
    [SerializeField] public LayerMask raycastLayerMask;
    private GameObject player;
    private BatController controller;
    private Vector3 nowPos;

    public bool IsEnd => controller.state.CurrentState == (int)BatController.e_State.magnetCatch ||
                         controller.state.CurrentState == (int)BatController.e_State.dead;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        controller = GetComponent<BatController>();

        agent.autoTraverseOffMeshLink = false; // OffMeshLink�ɂ��ړ����֎~

        StartCoroutine(MoveNormalSpeed(agent));
       
    }

    // Update is called once per frame
    IEnumerator MoveNormalSpeed(NavMeshAgent agent)
    {
        while (true)
        {
            // OffmeshLink�ɏ��܂ŕ��ʂɈړ�
            yield return new WaitWhile(() => agent.isOnOffMeshLink == false);

            if (IsEnd == true) continue;

            // OffMeshLink�ɏ�����̂ŁANavmeshAgent�ɂ��ړ����~�߂āA
            // OffMeshLink�̏I���܂�NavmeshAgent.speed�Ɠ������x�ňړ�

            agent.isStopped = true;
            agent.updateUpAxis = false;
            agent.updatePosition = false;

            //�̂�O�ɌX����
            Vector3 _localAngle = transform.localEulerAngles;
            _localAngle.x = controller.forwardAngle;
            transform.localEulerAngles = _localAngle;

            nowPos = transform.position + new Vector3(0, controller.height, 0);

            yield return new WaitWhile(() =>
            {
                if (IsEnd == true) return false;

                Vector3 _targetPos = agent.currentOffMeshLinkData.endPos;

                agent.destination = player.transform.position;

                if (_targetPos.y < player.transform.position.y)
                {
                    _targetPos.y = player.transform.position.y;
                }         

                nowPos = Vector3.MoveTowards(nowPos,_targetPos, agent.speed * Time.deltaTime);
                transform.position = nowPos;
                return Vector3.Distance(nowPos, _targetPos) > 0.05f;
            });

            if (IsEnd == true) continue;

            //�i�r���b�V���̉e����Y���̒l���n�ʂ̍��W�ɂȂ��Ă���
            Ray _ray = new Ray(transform.position, Vector3.down);
            RaycastHit _raycastHit;
            bool _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, raycastLayerMask);

            if (_hit == true)
            {
                controller.height = _raycastHit.distance;
            }

            // NavmeshAgent�𓞒B�������ɂ��āANavmesh���ĊJ
            agent.CompleteOffMeshLink();
            Debug.Log("�i�r���b�V�������N�I��");
            agent.isStopped = false;
            agent.updateUpAxis = true;
            agent.updatePosition = true;
        }
    }
}
