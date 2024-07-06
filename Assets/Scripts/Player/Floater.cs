using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;       //排水量
    public int floaterCount = 1;
    public float waterDrag = 0.99f;             //水的阻力
    public float waterAngularDrag = 0.5f;       //角阻力

    public void FixedUpdate()
    {
        rigidBody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

        //如果当前位置y值 < 0，说明小船现在在水下，给小船施加浮力
        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
        if(transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01( (waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;        //计算位移乘数？在0-1之间
            rigidBody.AddForceAtPosition( new Vector3( 0f , Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f) , transform.position , ForceMode.Acceleration); //给浮力
            rigidBody.AddForce(displacementMultiplier * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rigidBody.AddTorque(displacementMultiplier * -rigidBody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
