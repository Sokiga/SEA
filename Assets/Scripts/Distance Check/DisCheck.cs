using UnityEngine;

public class DisCheck : MonoBehaviour
{
    // Start is called before the first frame update
    public float DistanceCheck;
    public Transform other;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (other)
        {
            DistanceCheck = Vector3.Distance(other.position, transform.position);
        }
    }

    // 在编辑器中绘制Gizmos时调用
    private void OnDrawGizmos()
    {
        // 这将在Scene视图中绘制一条从当前对象到other的线
        if (other)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, other.position);
        }
    }

    // 这将在Inspector中实时更新DistanceCheck的值
    private void OnValidate()
    {
        if (other)
        {
            DistanceCheck = Vector3.Distance(other.position, transform.position);
        }
    }
}