using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlatform : MonoBehaviour
{
    public GameObject mainPlatform; // ��������ƽ̨
    public float rotationSpeed = 40f; // ��ת�ٶ�

    public Transform playerDefTransform;

    private void Start()
    {
        playerDefTransform = GameObject.FindGameObjectWithTag("Player").transform.parent;
    }

    void Update()
    {
        // ��Сƽ̨Χ�Ƴ�������ƽ̨��ת
        Vector3 center = mainPlatform.transform.position;
        Vector3 offset = transform.position - center;
        float angle = rotationSpeed * Time.deltaTime;
        transform.position = center + Quaternion.Euler(0, 0, angle) * offset;
    }
}
