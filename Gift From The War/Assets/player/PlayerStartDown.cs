using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartDown : MonoBehaviour
{
    [SerializeField] UIBlinking text;

    private CharacterController characterController;
    public bool isAuto;
    float angle;
    float time;

    enum e_AnimState
    {
        GetUp,
        StartLookAround,
        LookAround,
        EndLookAround,
    }

    e_AnimState state;

    private void Awake()
    {
        characterController = transform.GetComponent<CharacterController>();
        time = 0;

        state = e_AnimState.GetUp;
    }

    private void Start()
    {
        isAuto = SaveManager.Instance.nowSaveData.saveSpotNum == SaveManager.SaveSpotNum.none;
        if (isAuto == true)
        {
            transform.rotation *= Quaternion.Euler(90.0f, 0, 0);    //ÉvÉåÉCÉÑÅ[ÇâÒì]
        }
    }

    private void Update()
    {
        if (Mathf.Approximately(Time.timeScale, 0f)) return;
        if (isAuto == false) return;
        characterController.Move(Vector3.down * 5);
        time += Time.deltaTime;
        if (time < 2.0f) return;


        switch (state)
        {
            case e_AnimState.GetUp:
                GetUp();
                break;
            case e_AnimState.StartLookAround:
                StartLookAround();
                break;
            case e_AnimState.LookAround:
                LookAround();
                break;
            case e_AnimState.EndLookAround:
                EndLookAround();
                break;
        }
    }

    private void GetUp()
    {
        characterController.Move(Vector3.down * 5);    //â∫Ç…à⁄ìÆ
        characterController.Move(-(transform.forward * 0.5f) * Time.deltaTime);    //å„ÇÎÇ…â∫Ç™ÇÈ

        Quaternion rotate = Quaternion.Euler(0, 0, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, 30.0f * Time.deltaTime);
        Vector3 dif = transform.rotation.eulerAngles - rotate.eulerAngles;

        if (dif.magnitude <= 0.01f)
        {
            state = e_AnimState.StartLookAround;
        }
    }

    private void StartLookAround()
    {
        angle += Time.deltaTime * 60.0f;
        angle = Mathf.Min(angle,180);

        float rot = (1 - Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * angle))) * 20.0f;

        transform.rotation *= Quaternion.Euler(0, rot * Time.deltaTime, 0);

        if (angle >= 180.0f)
        {
            angle = 0;
            state = e_AnimState.LookAround;
        }
    }

    private void LookAround()
    {
        angle += Time.deltaTime * 60.0f;
        angle = Mathf.Min(angle, 180);

        float rot = -(1 - Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * angle))) * 80.0f;

        transform.rotation *= Quaternion.Euler(0, rot * Time.deltaTime, 0);

        if (angle >= 180.0f)
        {
            angle = 0;
            state = e_AnimState.EndLookAround;
        }
    }

    private void EndLookAround()
    {
        angle += Time.deltaTime * 60.0f;
        angle = Mathf.Min(angle, 180);

        float rot = (1 - Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * angle))) * 40.0f;

        transform.rotation *= Quaternion.Euler(0, rot * Time.deltaTime, 0);

        if (angle >= 180.0f)
        {
            isAuto = false;
            text.SetActive();
        }
    }
}
