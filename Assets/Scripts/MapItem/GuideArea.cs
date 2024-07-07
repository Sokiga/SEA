using UnityEngine;

public class GuideArea : MonoBehaviour
{
    public Vector3 guideVelocity; // �趨��������Ļ����ٶ�

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boat")) // �����ײ���Ƿ������
        {
            PlayerController playerMovement = other.GetComponent<PlayerController>();
            if (playerMovement != null)
            {
                playerMovement.ChangeEnvironmentVelocity(guideVelocity);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boat")) // �����ײ���Ƿ������
        {
            PlayerController playerMovement = other.GetComponent<PlayerController>();
            if (playerMovement != null)
            {
                playerMovement.ChangeEnvironmentVelocity(-guideVelocity);
            }
        }
    }
}

