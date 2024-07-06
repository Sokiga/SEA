using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;       //排水量

    public void FixedUpdate()
    {
        //如果当前位置y值 < 0，说明小船现在在水下，给小船施加浮力
        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
        if(transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01( (waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;        //计算位移乘数？在0-1之间
            rigidBody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration); //给浮力
        }
    }
}
