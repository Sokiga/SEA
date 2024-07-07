using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform point;
    public float Xspeed = 0;
    public float Zspeed = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.RotateAround(point.position, transform.right, Xspeed * Time.deltaTime);
        transform.RotateAround(point.position, transform.up, Zspeed * Time.deltaTime);
    }
}
