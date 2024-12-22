using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIBillboard : MonoBehaviour
{
    public Transform playerCamera; // �÷��̾��� ī�޶�

    void Update()
    {
        // ü�¹ٰ� ī�޶� �ٶ󺸵��� ����
        transform.LookAt(playerCamera);

        // Y�ุ ȸ���ϵ��� ����
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}