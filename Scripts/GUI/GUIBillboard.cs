using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIBillboard : MonoBehaviour
{
    public Transform playerCamera; // 플레이어의 카메라

    void Update()
    {
        // 체력바가 카메라를 바라보도록 설정
        transform.LookAt(playerCamera);

        // Y축만 회전하도록 설정
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}