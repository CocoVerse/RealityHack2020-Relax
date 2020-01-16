using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class FloatInPlace : MonoBehaviour
{
    [SerializeField] Transform center;
    [SerializeField] float anchorHeight;

    private Pose veryInitialPose;
    private Vector3 anchorPosition;
    private Rigidbody rb;

    private bool IsNonFree => rb.isKinematic || !rb.useGravity;

    // Start is called before the first frame update
    void Start()
    {
        veryInitialPose = new Pose(transform.position, transform.rotation);

        rb = GetComponent<Rigidbody>();
        if (rb == null) throw new MissingComponentException();

        var ie = GetComponent<Interactable>();
        if(ie != null) {
            ie.onDetachedFromHand += _=>ResetAnchor();
        }
        

        ResetAnchor();
    }

    private void ResetAnchor() {
        anchorPosition = transform.position;
        anchorPosition.y = 0;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F5) && !IsNonFree) ReturnToStart();
    }
    void ReturnToStart() {
        transform.SetPositionAndRotation(veryInitialPose.position, veryInitialPose.rotation);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void FixedUpdate() {
        if (IsNonFree) return;

        var distAboveAnchor = transform.position.y - anchorHeight;

        var antigrav = Mathf.SmoothStep(1, 0, distAboveAnchor);
        var updraft = Mathf.SmoothStep(0.25f, 0, distAboveAnchor + 0.5f);

        rb.AddForce(Time.fixedDeltaTime * (antigrav + updraft) * -Physics.gravity, ForceMode.VelocityChange);

        var horizontalDelta = anchorPosition - new Vector3(transform.position.x, 0, transform.position.z);
        rb.AddForce(Time.fixedDeltaTime * horizontalDelta, ForceMode.VelocityChange);

    }

}
