using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogVigilanceState : State<DogState>
{
    public DogVigilanceState(DogState owner) : base(owner) { }
    bool switchAnime;
    float range;
    float visibility;

    public override void Enter()
    {
        //�x���A�j���[�V�����ɕύX����
        owner.animator.SetTrigger("Vigilance");

        //�X�e�[�g�}�V�����̐؂�ւ����������Ă��Ȃ����ߕϐ���p�ӂ���
        switchAnime = true;
        range = 5.0f;
        visibility = 40.0f;
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("metarig_action_Vigilance") == false) switchAnime = false;
       
        owner.agent.isStopped = true;
        owner.agent.destination = owner.dog.transform.position;
    }

    public override void Execute()
    {
        //�A�j���[�V�������؂�ւ���Ă��Ȃ��ꍇ
        if (switchAnime == false)
        {
            CheckSwitchAnime();
            return;
        }

        //�A�j���[�V�������I�����Ă���ꍇ
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("metarig_action_Vigilance") == false)
        {
            owner.ChangeState(e_DogState.Search);
            return;
        }

        if (CheckVisibility() == true)
        {
            owner.ChangeState(e_DogState.Traking);
            return;
        }
    }

    public override void Exit()
    {
        owner.endAnimationFlg = false;
        owner.agent.isStopped = false;
        owner.StartCoroutine(CoolDownCoroutine());
    }

    private IEnumerator CoolDownCoroutine()
    {
        owner.canVigilance = false;
        yield return new WaitForSeconds(3.0f);
        owner.canVigilance = true;
    }

    void CheckSwitchAnime()
    {
        if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("metarig_action_Vigilance") == true)
        {
            switchAnime = true;
        }
    }

    public bool CheckVisibility()
    {
        Transform _dogTrans = owner.dog.transform;

        Vector3 _playerPos = owner.player.transform.position;
        Vector3 _dogHeadPos = _dogTrans.position + ((_dogTrans.forward * 0.5f) + (_dogTrans.up * 0.5f));

        float distance = Vector3.Distance(_playerPos,_dogHeadPos);

        //��������
        if (range >= distance) return false;

        Vector3 _targetVec = _playerPos - _dogHeadPos;
        Vector3 _forwardVec = owner.dog.transform.forward;

        float dot = Vector3.Dot(_targetVec,_forwardVec);
        float degAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        //�p�x����
        if (visibility >= degAngle) return false;

        Ray _ray = new Ray(_dogHeadPos, _targetVec);
        RaycastHit _raycastHit;

        bool hit = Physics.Raycast(_ray, out _raycastHit,range);

        //���C����
        if (hit == false || _raycastHit.collider.tag != "Player") return false;

        return true;
    }
}
