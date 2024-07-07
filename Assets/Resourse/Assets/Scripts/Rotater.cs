using UnityEngine;

public class Rotater : MonoBehaviour
{
    public Transform point;
    public float rotationSpeed = 100f; // 转动速度
    private float rotationProgress = 0f;
    private float fullRotation = 360f;
    public bool isLeftOar = true; // 指示是左船桨还是右船桨
    private bool isRotating = false;
    private Quaternion initialLocalRotation; // 初始局部旋转
    private Vector3 initialLocalPosition; // 初始局部位置
    private float returnSpeed = 2f; // 返回速度

    private void Start()
    {
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
    }
    void Update()
    {
        if (isRotating)
        {
            // 根据船桨方向设置旋转角度
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

            // 如果位置和旋转几乎相等，停止返回
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

