using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{

	public float pushPower = 2.0f;

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody rb = hit.collider.attachedRigidbody;

		// ����̃I�u�W�F�N�g��Rigidbody�����Ă��Ȃ�������AisKinematic�Ƀ`�F�b�N�������Ă���ꍇ�ɂ͉����Ȃ��B
		if (rb == null || rb.isKinematic)
		{
			return;
		}

		if (hit.moveDirection.y < -0.3f)
		{
			return;
		}

		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

		rb.velocity = pushDir * pushPower;
	}
}
