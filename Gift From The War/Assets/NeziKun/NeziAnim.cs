using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NeziAnim : MonoBehaviour
{
	//アニメーション一覧
	// Standby()   待機モーション　FlgがOnにされ、その間spanで設定された時間がたつごとにアニメーションされる
	// Talk()      話しかけた時に上を向くモーション
	// Return()    高速回転して地面に埋まるモーション　埋まる値などは何となくなので要設定

	//ねじ君にAnimatorコンポーネントをつける（コントローラー部分にはNeziKunACをつける）
	//このスクリプトもつける

	public float span = 20f; //待機モーションのスパン時間
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

		//ちょっと浮く→高速回転
		var BackNeziAnime = DOTween.Sequence()
			.Append(transform.DOMove(endValue: new Vector3(0f, 0.4f, 0f), duration: 0.5f).SetRelative(true).SetEase(Ease.OutCubic))
			.Join(transform.DORotate(endValue: new Vector3(0f, 3600.0f, 0f), duration: 2.2f, RotateMode.FastBeyond360).SetEase(Ease.OutCubic));

		//時間差で下にもぐる
		BackNeziAnime.InsertCallback(0.501f, () =>
		{
			transform.DOMove(endValue: new Vector3(0f, -1.8f, 0f), duration: 1.5f).SetEase(Ease.OutCubic).SetRelative(true).Play();
		});
	}
}