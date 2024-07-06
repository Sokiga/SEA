using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    public LayerMask layers;
    public Vector3 backNormalPos;
    public Vector3 forwardNormalPos;
    public float detectionDistance;
    public float forwardDistance;
    public float backDistance;
    public Transform mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main.transform;
    }

    private void Start()
    {
        backNormalPos = new Vector3(0, 0, -1);
        forwardNormalPos = new Vector3(0, 0, 1);
    }

    public float UpdateCameraCollider()
    {
        // 检测相机后方
        var backDetectionPos = transform.TransformPoint(backNormalPos * detectionDistance);
        if (Physics.Linecast(transform.position, backDetectionPos, out var backHit, layers, QueryTriggerInteraction.Ignore))
        {
            //向前是让currentoffset减少
            return -forwardDistance;
        }

        // 检测相机前方
        var forwardDetectionPos = transform.TransformPoint(forwardNormalPos * detectionDistance);
        if (Physics.Linecast(transform.position, forwardDetectionPos, out var forwardHit, layers, QueryTriggerInteraction.Ignore))
        {
            //向后是让currentoffset增加
            return backDistance;
        }

        // 如果前后都没有检测到障碍物，返回 0
        return 0;
    }
}

