using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFloater : MonoBehaviour
{
    public float AirDrag = 1;
    public float WaterDrag = 10;
    public Transform[] FloatPoints;

    protected Rigidbody Rigidbody;
    //protected Waves Waves;
}
