using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    BaseState state;
    public float hight { get; set; }
    public float forwardAngle { get; set; }
    [SerializeField] public float defaltHight;
    [SerializeField] public float defaltForwardAngle;


    // Start is called before the first frame update
    void Start()
    {
        hight = defaltHight;
        forwardAngle = defaltForwardAngle;
        //�X�e�[�g��؂�ւ���
        ChangeState(GetComponent<batMove>());
    }

    // Update is called once per frame
    void Update()
    {
        state.Update();
    }

    public void ChangeState(BaseState _state)
    {
        //���̂��폜
        state = null;
        //�V�������̂̃A�h���X������
        state = _state;
        state.Start();
    }
}
