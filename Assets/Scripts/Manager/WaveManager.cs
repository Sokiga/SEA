using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    public float amplitude = 1f;    //振幅
    public float length = 2f;       //波长
    public float speed = 1f;        //速度
    public float offset = 0f;       //偏移

    private void Update()
    {
        offset += Time.deltaTime * speed;
    }

    //获得某一点波的波高
    public float GetWaveHeight(float _x)
    {
        return amplitude * Mathf.Sin(_x / length + offset);
    }
}
