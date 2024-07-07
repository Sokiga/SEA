using UnityEngine;

public class Rotater : MonoBehaviour
{
    public Transform point;
    public float rotationSpeed = 100f; // 转动速度
    private float rotationProgress = 0f;
    private float fullRotation = 360f;
    public bool isLeftOar = true; // 指示是左船桨还是右船桨
    private bool isRotating = false;

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
    }

    public void RotateOnce()
    {
        isRotating = true;
    }
}

