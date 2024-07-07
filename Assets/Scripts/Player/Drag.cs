using UnityEngine;

public class Drag : MonoBehaviour
{
    public float detectionRange = 5f; // ��ⷶΧ
    public float holdDuration = 0.8f; // ��������ʱ��
    public Transform holdPoint; // �����Χ��ת�ĵ�
    public float holdDistance = 2f; // ��������ľ���
    public float rotationSpeed = 50f; // ��ת�ٶ�
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
            currentBox.transform.localPosition = new Vector3(holdDistance, 0, 0); // ������������ľ���
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
            if (other.CompareTag("Box")) // �����ײ���Ƿ������
            {
                detectedBox = other.gameObject;
            }
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box")) // �����ײ���Ƿ������
        {
            detectedBox=null;
        }
    }
}

