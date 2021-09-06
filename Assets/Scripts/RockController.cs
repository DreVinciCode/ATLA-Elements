using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{   
    public GameObject wholeObject;
    public GameObject fracturedObject;

    private void Start()
    {
        fracturedObject.SetActive(true);
        HandGestureRecognition handEvents = FindObjectOfType<HandGestureRecognition>();
        handEvents.onFistDetected += HandEvents_onFistDetected;
        handEvents.onFlatPalmDetected += HandEvents_onFlatPalmDetected;
        handEvents.onPalmDisappearnce += HandEvents_onPalmDisappearnce;
    }

    private void HandEvents_onPalmDisappearnce(object sender, EventArgs e)
    {
        Debug.Log("DestroyEarth called ");
        destroyEarth();
    }

    private void HandEvents_onFlatPalmDetected(object sender, EventArgs e)
    {
        Debug.Log("Fractured Called");
        FractureEffect();
    }

    private void HandEvents_onFistDetected(object sender, EventArgs e)
    {
        Debug.Log("Whole Called");
        WholeEffect();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            FractureEffect();
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            WholeEffect();
        }
    }

    public void FractureEffect()
    {
        wholeObject.SetActive(false);
        fracturedObject.SetActive(true);
    }

    public void WholeEffect()
    {
        wholeObject.SetActive(true);
        fracturedObject.SetActive(false);
    }

    public void destroyEarth()
    {
        wholeObject.SetActive(false);
        fracturedObject.SetActive(false);
    }

}
