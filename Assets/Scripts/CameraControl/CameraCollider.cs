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
        // ��������
        var backDetectionPos = transform.TransformPoint(backNormalPos * detectionDistance);
        if (Physics.Linecast(transform.position, backDetectionPos, out var backHit, layers, QueryTriggerInteraction.Ignore))
        {
            //��ǰ����currentoffset����
            return -forwardDistance;
        }

        // ������ǰ��
        var forwardDetectionPos = transform.TransformPoint(forwardNormalPos * detectionDistance);
        if (Physics.Linecast(transform.position, forwardDetectionPos, out var forwardHit, layers, QueryTriggerInteraction.Ignore))
        {
            //�������currentoffset����
            return backDistance;
        }

        // ���ǰ��û�м�⵽�ϰ������ 0
        return 0;
    }
}

