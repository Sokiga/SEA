using UnityEngine;

public class Drag : MonoBehaviour
{
    public float detectionRange = 5f; // 检测范围
    public float holdDuration = 0.8f; // 长按持续时间
    public Transform holdPoint; // 玩家周围旋转的点
    public float holdDistance = 2f; // 箱子离轴的距离
    public float rotationSpeed = 50f; // 旋转速度
    public float pressStartTime;
    public bool isPressing = false;
    public bool isHoldingBox = false;
    public GameObject currentBox;
    public GameObject detectedBox;
    void Update()
    {
        HandleSpace();
        if (isHoldingBox && currentBox != null)
        {
            RotateBoxAroundPlayer();
        }
    }

    private void HandleSpace()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressStartTime = Time.time;
            isPressing = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isPressing = false;
            float pressDuration = Time.time - pressStartTime;

            if (pressDuration >= holdDuration)
            {
                if (isHoldingBox)
                {
                    ReleaseBox();
                }
                else
                {
                    TryPickupBox();
                }
            }
        }

    }


    private void TryPickupBox()
    {
        if (detectedBox != null)
        {
            currentBox = detectedBox;
            currentBox.transform.SetParent(holdPoint);
            currentBox.transform.GetComponent<Rigidbody>().useGravity = false;
            currentBox.transform.localPosition = new Vector3(holdDistance, 0, 0); // 设置箱子离轴的距离
            isHoldingBox = true;
            detectedBox = null;
        }
    }

    private void ReleaseBox()
    {
        if (currentBox != null)
        {
            currentBox.transform.SetParent(null);
            currentBox.transform.GetComponent<Rigidbody>().useGravity = true;
            currentBox = null;
            isHoldingBox = false;
        }
    }

    private void RotateBoxAroundPlayer()
    {
        holdPoint.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if(isPressing && !isHoldingBox)
        {
            if (other.CompareTag("Box")) // 检查碰撞的是否是玩家
            {
                detectedBox = other.gameObject;
            }
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box")) // 检查碰撞的是否是玩家
        {
            detectedBox=null;
        }
    }
}

