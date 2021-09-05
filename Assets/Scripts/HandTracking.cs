using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;


public class HandTracking : MonoBehaviour
{
    MixedRealityPose thumbPose;
    MixedRealityPose middlePose;

    public event EventHandler OnSnapDetected;

    private float distanceMiddleThumb;
    private float gesture_timer;
    private float fingerSnap_timer; 
    private float gesture_timer_threshold = 0.3f;
    private float fingerSnap_deadline = 1.0f;

    private void Awake()
    {
        SoundManager.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            OnSnapDetected?.Invoke(this, EventArgs.Empty);
        }

        //Approach to detect a finger snap for now; until a better method can be introduced.
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, Handedness.Both, out thumbPose) && HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, Handedness.Both, out middlePose))
        {        
            distanceMiddleThumb = Vector3.Distance(thumbPose.Position, middlePose.Position);
        }

        float thumbCurl = HandPoseUtils.ThumbFingerCurl(Handedness.Both);
        float indexCurl = HandPoseUtils.IndexFingerCurl(Handedness.Both);
        float middleCurl = HandPoseUtils.MiddleFingerCurl(Handedness.Both);
        float ringCurl = HandPoseUtils.RingFingerCurl(Handedness.Both);
        float pinkyCurl = HandPoseUtils.PinkyFingerCurl(Handedness.Both);

        //Pre-Finger Snap
        if(pinkyCurl > 0.6f && ringCurl > 0.6f && middleCurl < 0.4f && distanceMiddleThumb < 0.025f)
        {
            gesture_timer += Time.deltaTime;
            if (gesture_timer >= gesture_timer_threshold)
            {
                fingerSnap_timer = 0;
            }
        }
        else
        {
            gesture_timer = 0f;
        }

        fingerSnap_timer += Time.deltaTime;

        //Small time window to detect post-snap
        if (fingerSnap_timer < fingerSnap_deadline)
        {
            //Post-Finger Snap
            if (pinkyCurl > 0.6f && ringCurl > 0.6f && middleCurl > 0.7f && distanceMiddleThumb > 0.05f)
            {
                //call event
                OnSnapDetected?.Invoke(this, EventArgs.Empty);
            }
        }
    }


}
