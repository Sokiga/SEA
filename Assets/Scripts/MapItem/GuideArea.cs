using UnityEngine;

public class GuideArea : MonoBehaviour
{
    public Vector3 guideVelocity; // 设定引导区域的环境速度

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boat")) // 检查碰撞的是否是玩家
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
        if (other.CompareTag("Boat")) // 检查碰撞的是否是玩家
        {
            PlayerController playerMovement = other.GetComponent<PlayerController>();
            if (playerMovement != null)
            {
                playerMovement.ChangeEnvironmentVelocity(-guideVelocity);
            }
        }
    }
}

