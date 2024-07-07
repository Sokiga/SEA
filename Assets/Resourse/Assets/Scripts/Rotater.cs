using UnityEngine;

public class Rotater : MonoBehaviour
{
    public Transform point;
    public float rotationSpeed = 100f; // ת���ٶ�
    private float rotationProgress = 0f;
    private float fullRotation = 360f;
    public bool isLeftOar = true; // ָʾ���󴬽������Ҵ���
    private bool isRotating = false;

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
    }

    public void RotateOnce()
    {
        isRotating = true;
    }
}

