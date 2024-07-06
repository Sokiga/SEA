using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlatform : MonoBehaviour
{
    public GameObject mainPlatform; // 长方形主平台
    public float rotationSpeed = 40f; // 旋转速度

    public Transform playerDefTransform;

    private void Start()
    {
        playerDefTransform = GameObject.FindGameObjectWithTag("Player").transform.parent;
    }

    void Update()
    {
        // 让小平台围绕长方形主平台旋转
        Vector3 center = mainPlatform.transform.position;
        Vector3 offset = transform.position - center;
        float angle = rotationSpeed * Time.deltaTime;
        transform.position = center + Quaternion.Euler(0, 0, angle) * offset;
    }
}
