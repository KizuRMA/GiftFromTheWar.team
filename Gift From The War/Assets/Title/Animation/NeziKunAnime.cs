using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NeziKunAnime : MonoBehaviour
{
    private Animator anim;  //Animatorをanimという変数で定義する

    // Start is called before the first frame update
    void Start()
    {
        //変数animに、Animatorコンポーネントを設定する
        anim = gameObject.GetComponent<Animator>();
    }


    public void Animate()
    {
        anim.SetFloat("Speed", 1);
        //anim.SetBool("JumpFlg", true);
        anim.Play("アーマチュア|action_jump", 0, 0f);

        var NeziAnime = DOTween.Sequence()
           .Append(transform.DOMove(endValue: new Vector3(0f, 1.8f, 0f), duration: 1.5f).SetEase(Ease.OutCubic).SetRelative(true))
           .Join(transform.DORotate(endValue: new Vector3(0f, 3600.0f, 0f), duration: 2.2f, RotateMode.FastBeyond360).SetEase(Ease.OutCubic));
        //.Append(transform.DORotate(endValue: new Vector3(0f, 3600.0f, 0f), duration: 2.2f, RotateMode.FastBeyond360).SetEase(Ease.OutCubic));

        NeziAnime.InsertCallback(1.501f, () =>
        {
            transform.DOMove(endValue: new Vector3(0f, -0.4f, 0f), duration: 0.5f).SetRelative(true).SetEase(Ease.InQuint).Play();
        });
    }
    public void BackAnimate()
    {
        anim.SetFloat("Speed", -5f);
        //anim.SetBool("JumpFlg", true);
        anim.Play("アーマチュア|action_jump", 0, 1f);

        var BackNeziAnime = DOTween.Sequence()
            .Append(transform.DOMove(endValue: new Vector3(0f, 0.4f, 0f), duration: 0.5f).SetRelative(true).SetEase(Ease.OutCubic))
            .Join(transform.DORotate(endValue: new Vector3(0f, 3600.0f, 0f), duration: 2.2f, RotateMode.FastBeyond360).SetEase(Ease.OutCubic));
            
           
        //.Append(transform.DORotate(endValue: new Vector3(0f, 3600.0f, 0f), duration: 2.2f, RotateMode.FastBeyond360).SetEase(Ease.OutCubic));

        BackNeziAnime.InsertCallback(0.501f, () =>
        {
            transform.DOMove(endValue: new Vector3(0f, -1.8f, 0f), duration: 1.5f).SetEase(Ease.OutCubic).SetRelative(true).Play();
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            
            Animate();
        }
        if (Input.GetKey(KeyCode.O))
        {

            BackAnimate();
        }
    }
  
}
