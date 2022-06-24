using UnityEngine;
public class CalcVelocityExample : MonoBehaviour
{
    // 1�t���[���O�̈ʒu
    private Vector3 _prevPosition;
    public float moveDistance;
    public float timeCounter;
    public float nowSpeed;

    private void Start()
    {
        // �����ʒu��ێ�
        _prevPosition = transform.position;
        timeCounter = 0;
        moveDistance = 0;
        nowSpeed = 0;
    }

    private void Update()
    {
        //1�t���[���O���瓮�����������擾
        moveDistance += ((transform.position - _prevPosition) / Time.deltaTime).magnitude;
        timeCounter += Time.deltaTime;

        if (timeCounter >= 0.1f)
        {
            nowSpeed = moveDistance;
            moveDistance = 0;
            timeCounter = 0;
        }

        _prevPosition = transform.position;
    }
}
