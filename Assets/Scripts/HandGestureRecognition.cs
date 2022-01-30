using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;


[Serializable]
public struct Gesture
{
    public string name;
    public List<float> fingerCurls;
}

public class HandGestureRecognition : MonoBehaviour
{
    public GameObject EarthSystem;
    GameObject PalmObject;

    private Gesture previousGesture;

    public List<Gesture> gestures;
    public float fist_threshold;
    public float flatpalm_threshold;
    private float curlAverage;
    private float threshold = 0.1f;

    private bool _solidForm_check;
    private bool destroy_check;

    public TMP_Text curlValues;

    public event EventHandler onFistDetected;
    public event EventHandler onFlatPalmDetected;
    public event EventHandler onPalmDisappearnce;

    MixedRealityPose pose;

    private void Start()
    {
        _solidForm_check = false;
        EarthSystem.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        Gesture currentGesture = RecognizedGesture();
        bool hasRecognized = !currentGesture.Equals(new Gesture());

        if(hasRecognized && !currentGesture.Equals(previousGesture) && !currentGesture.Equals(gestures[0]))
        {
            previousGesture = currentGesture;
        }

        float thumbCurl = HandPoseUtils.ThumbFingerCurl(Handedness.Any);
        float indexCurl = HandPoseUtils.IndexFingerCurl(Handedness.Any);
        float middleCurl = HandPoseUtils.MiddleFingerCurl(Handedness.Any);
        float ringCurl = HandPoseUtils.RingFingerCurl(Handedness.Any);
        float pinkyCurl = HandPoseUtils.PinkyFingerCurl(Handedness.Any);

        curlValues.text = thumbCurl + "\n"
            + indexCurl + "\n"
            + middleCurl + "\n"
            + ringCurl + "\n"
            + pinkyCurl + "\n"
            + currentGesture.name + "\n"
            + curlAverage;


        if (curlAverage >= 0.78 && !_solidForm_check)
        {
            _solidForm_check = true;
            onFistDetected?.Invoke(this, EventArgs.Empty);
        }

        else if (curlAverage < 0.78 && _solidForm_check )
        {
            _solidForm_check = false;
            onFlatPalmDetected?.Invoke(this, EventArgs.Empty);
        }

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Both, out pose))
        {
            EarthSystem.SetActive(true);
            destroy_check = true;
        }
        else
        {
            if(destroy_check)
            {
                destroy_check = false;
                onPalmDisappearnce?.Invoke(this, EventArgs.Empty);
                EarthSystem.SetActive(false);
            }
        }
    }

    Gesture RecognizedGesture()
    {
        float thumbCurl = HandPoseUtils.ThumbFingerCurl(Handedness.Any);
        float indexCurl = HandPoseUtils.IndexFingerCurl(Handedness.Any);
        float middleCurl = HandPoseUtils.MiddleFingerCurl(Handedness.Any);
        float ringCurl = HandPoseUtils.RingFingerCurl(Handedness.Any);
        float pinkyCurl = HandPoseUtils.PinkyFingerCurl(Handedness.Any);
        float[] currentCurlValues = { thumbCurl, indexCurl, middleCurl, ringCurl, pinkyCurl };

        curlAverage = (thumbCurl + indexCurl + middleCurl + ringCurl + pinkyCurl) / 5;
        Rock_Distance_Lerp.influence = curlAverage;

        bool inRange = false;
        for (int i = 0; i < gestures.Count; i++)
        {
            for (int j = 0; j < currentCurlValues.Length; j++)
            {
                if(currentCurlValues[j] <= gestures[i].fingerCurls[j] + threshold && currentCurlValues[j] >= gestures[i].fingerCurls[j] - threshold)
                {
                    inRange = true;
                }
                else
                {
                    inRange = false;
                    break;
                }
            }

            if(inRange)
            {
                return gestures[i];
            }
        }

        return gestures[0];
    }

    /*
    Gesture RecognizedGesture()
    {
        float thumbCurl = HandPoseUtils.ThumbFingerCurl(Handedness.Any);
        float indexCurl = HandPoseUtils.IndexFingerCurl(Handedness.Any);
        float middleCurl = HandPoseUtils.MiddleFingerCurl(Handedness.Any);
        float ringCurl = HandPoseUtils.RingFingerCurl(Handedness.Any);
        float pinkyCurl = HandPoseUtils.PinkyFingerCurl(Handedness.Any);
        float[] currentCurlValues = { thumbCurl, indexCurl, middleCurl, ringCurl, pinkyCurl };

        curlAverage = (thumbCurl + indexCurl + middleCurl + ringCurl + pinkyCurl) / 5;

        //Fist average curl values [x > 0.6]
        if(curlAverage > fist_threshold)
        {
            for (int i = 0; i < currentCurlValues.Length; i++)
            {
                if(currentCurlValues[i] > 0.4f)
                {
                    return gestures[2];
                }
            }
        }

        else if (curlAverage < flatpalm_threshold)
        {
            for (int i = 0; i < currentCurlValues.Length; i++)
            {
                if(currentCurlValues[i] < 0.05f)
                {
                    return gestures[i];
                }
            }
        }

        return gestures[0];

    }
    */
}
