using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSoul : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boat")) // 检查碰撞的是否是玩家
        {
            PlayerController playerMovement = other.GetComponent<PlayerController>();
            if (playerMovement != null)
            {
                playerMovement.AddSoulAmount(1);
            }
        }
    }
}
