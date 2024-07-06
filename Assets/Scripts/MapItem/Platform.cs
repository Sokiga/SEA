using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("有物体进来了");
        if (collision.gameObject.CompareTag("Boat"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boat"))
        {
            collision.transform.SetParent(null);
        }
    }
}
