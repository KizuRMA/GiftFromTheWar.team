using UnityEngine;
public class CalcVelocityExample : MonoBehaviour
{
    // 1フレーム前の位置
    private Vector3 _prevPosition;
    public float moveDistance;
    public float timeCounter;
    public float nowSpeed;

    private void Start()
    {
        // 初期位置を保持
        _prevPosition = transform.position;
        timeCounter = 0;
        moveDistance = 0;
        nowSpeed = 0;
    }

    private void Update()
    {
        //1フレーム前から動いた距離を取得
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
