using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NeziAnim : MonoBehaviour
{
	//�A�j���[�V�����ꗗ
	// Standby()   �ҋ@���[�V�����@Flg��On�ɂ���A���̊�span�Őݒ肳�ꂽ���Ԃ������ƂɃA�j���[�V���������
	// Talk()      �b�����������ɏ���������[�V����
	// Return()    ������]���Ēn�ʂɖ��܂郂�[�V�����@���܂�l�Ȃǂ͉��ƂȂ��Ȃ̂ŗv�ݒ�

	//�˂��N��Animator�R���|�[�l���g������i�R���g���[���[�����ɂ�NeziKunAC������j
	//���̃X�N���v�g������

	public float span = 20f; //�ҋ@���[�V�����̃X�p������
	private float currentTime = 0f;

	private bool StandbyFlg = true;

	private Animator anim;  

	void Start()
	{
		anim = gameObject.GetComponent<Animator>();
		anim.Play("Stand-by", 0, 0f);
	}

	void Update()
	{

		if (StandbyFlg)
        {
			currentTime += Time.deltaTime;

			if (currentTime > span)
			{
				anim.Play("Stand-by", 0, 0f);
				currentTime = 0f;
			}
		}
	}

	public void Standby()
	{
		StandbyFlg = true;
	}

	public void Talk()
	{
		StandbyFlg = false;
		anim.Play("Talk", 0, 0f);
	}

	public void Return()
	{
		StandbyFlg = false;

		anim.SetFloat("Speed", -5f);
		anim.Play("Return", 0, 1f);

		//������ƕ�����������]
		var BackNeziAnime = DOTween.Sequence()
			.Append(transform.DOMove(endValue: new Vector3(0f, 0.4f, 0f), duration: 0.5f).SetRelative(true).SetEase(Ease.OutCubic))
			.Join(transform.DORotate(endValue: new Vector3(0f, 3600.0f, 0f), duration: 2.2f, RotateMode.FastBeyond360).SetEase(Ease.OutCubic));

		//���ԍ��ŉ��ɂ�����
		BackNeziAnime.InsertCallback(0.501f, () =>
		{
			transform.DOMove(endValue: new Vector3(0f, -1.8f, 0f), duration: 1.5f).SetEase(Ease.OutCubic).SetRelative(true).Play();
		});
	}
}