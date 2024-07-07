using UnityEngine;

public class Rotater : MonoBehaviour
{
    public Transform point;
    public float rotationSpeed = 100f; // ת���ٶ�
    private float rotationProgress = 0f;
    private float fullRotation = 360f;
    public bool isLeftOar = true; // ָʾ���󴬽������Ҵ���
    private bool isRotating = false;
    private Quaternion initialLocalRotation; // ��ʼ�ֲ���ת
    private Vector3 initialLocalPosition; // ��ʼ�ֲ�λ��
    private float returnSpeed = 2f; // �����ٶ�

    private void Start()
    {
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
    }
    void Update()
    {
        if (isRotating)
        {
            // ���ݴ�������������ת�Ƕ�
            float rotationAngle = (isLeftOar ? rotationSpeed : -rotationSpeed) * Time.deltaTime;
            transform.RotateAround(point.position, transform.right, rotationAngle);
            rotationProgress += Mathf.Abs(rotationAngle);
            if (rotationProgress >= fullRotation)
            {
                isRotating = false;
                rotationProgress = 0f;
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialLocalPosition, returnSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, initialLocalRotation, returnSpeed * Time.deltaTime);

            // ���λ�ú���ת������ȣ�ֹͣ����
            if (Vector3.Distance(transform.localPosition, initialLocalPosition) < 0.01f && Quaternion.Angle(transform.localRotation, initialLocalRotation) < 1f)
            {
                transform.localPosition = initialLocalPosition;
                transform.localRotation = initialLocalRotation;
            }
        }
    }

    public void RotateOnce()
    {
        isRotating = true;
    }
}

