using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_Distance_Lerp : MonoBehaviour
{
    [Range(0.01f, 0.99f)]
    public static float influence = 0.01f;

    public GameObject referenceObject;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    public Vector3 axis;
 
    private float distance; 
    public float rotationSpeed = 100f;
    public float raduisSpeed = 0.5f;

    private void Start()
    {
        initialPosition = transform.position;
        distance = Vector3.Distance(initialPosition, referenceObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(referenceObject.transform.position, axis, rotationSpeed * Time.deltaTime);
        targetPosition = (transform.position - referenceObject.transform.position).normalized * (distance * (1-influence)) + referenceObject.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * rotationSpeed);
    }
}
