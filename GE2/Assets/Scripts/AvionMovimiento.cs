using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvionMovimiento : MonoBehaviour
{
    public Transform target;
    public Transform mira;
    public float velocidad = 2f;
    public float radio = 1f;
    public float angulo = 0f;

    // Update is called once per frame
    void Update()
    {
        float x = target.position.x + Mathf.Cos(angulo) * radio;
        float y = target.position.y;
        float z = target.position.z + Mathf.Sin(angulo) * radio;

        transform.position = new Vector3(x, y, z);

        transform.LookAt(mira);


        angulo += velocidad * Time.deltaTime;
    }
}
