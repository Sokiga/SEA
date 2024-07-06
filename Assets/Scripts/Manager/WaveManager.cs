using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    public float amplitude = 1f;    //���
    public float length = 2f;       //����
    public float speed = 1f;        //�ٶ�
    public float offset = 0f;       //ƫ��

    private void Update()
    {
        offset += Time.deltaTime * speed;
    }

    //���ĳһ�㲨�Ĳ���
    public float GetWaveHeight(float _x)
    {
        return amplitude * Mathf.Sin(_x / length + offset);
    }
}
