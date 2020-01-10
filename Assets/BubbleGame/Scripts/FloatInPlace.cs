using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class FloatInPlace : MonoBehaviour
{
    [SerializeField] Transform center;
    [SerializeField] float anchorHeight;
    private Vector3 anchorPosition;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
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

    void FixedUpdate()
    {
        if (rb.isKinematic || !rb.useGravity) return;

        var distAboveAnchor = transform.position.y - anchorHeight;

        var antigrav = Mathf.SmoothStep(1,0,distAboveAnchor);
        var updraft = Mathf.SmoothStep(0.25f, 0, distAboveAnchor + 0.5f);

        rb.AddForce(Time.fixedDeltaTime* (antigrav + updraft) * -Physics.gravity, ForceMode.VelocityChange);

        var horizontalDelta = anchorPosition - new Vector3(transform.position.x, 0, transform.position.z);
        rb.AddForce(Time.fixedDeltaTime * horizontalDelta, ForceMode.VelocityChange);

    }
}
