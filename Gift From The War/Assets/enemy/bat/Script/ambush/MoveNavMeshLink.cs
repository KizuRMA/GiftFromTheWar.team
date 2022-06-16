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

        agent.autoTraverseOffMeshLink = false; // OffMeshLinkによる移動を禁止

        StartCoroutine(MoveNormalSpeed(agent));
       
    }

    // Update is called once per frame
    IEnumerator MoveNormalSpeed(NavMeshAgent agent)
    {
        while (true)
        {
            // OffmeshLinkに乗るまで普通に移動
            yield return new WaitWhile(() => agent.isOnOffMeshLink == false);

            if (IsEnd == true) continue;

            // OffMeshLinkに乗ったので、NavmeshAgentによる移動を止めて、
            // OffMeshLinkの終わりまでNavmeshAgent.speedと同じ速度で移動

            agent.isStopped = true;
            agent.updateUpAxis = false;
            agent.updatePosition = false;

            //体を前に傾ける
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

            //ナビメッシュの影響でY軸の値が地面の座標になっている
            Ray _ray = new Ray(transform.position, Vector3.down);
            RaycastHit _raycastHit;
            bool _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, raycastLayerMask);

            if (_hit == true)
            {
                controller.height = _raycastHit.distance;
            }

            // NavmeshAgentを到達した事にして、Navmeshを再開
            agent.CompleteOffMeshLink();
            Debug.Log("ナビメッシュリンク終了");
            agent.isStopped = false;
            agent.updateUpAxis = true;
            agent.updatePosition = true;
        }
    }
}
