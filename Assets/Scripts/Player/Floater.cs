using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;       //��ˮ��
    public int floaterCount = 1;
    public float waterDrag = 0.99f;             //ˮ������
    public float waterAngularDrag = 0.5f;       //������

    public void FixedUpdate()
    {
        rigidBody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

        //�����ǰλ��yֵ < 0��˵��С��������ˮ�£���С��ʩ�Ӹ���
        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
        if(transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01( (waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;        //����λ�Ƴ�������0-1֮��
            rigidBody.AddForceAtPosition( new Vector3( 0f , Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f) , transform.position , ForceMode.Acceleration); //������
            rigidBody.AddForce(displacementMultiplier * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rigidBody.AddTorque(displacementMultiplier * -rigidBody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
