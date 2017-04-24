using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : InteractableBase
{
    protected Rigidbody rigidBody;
    protected bool currentlyInteracting;
    protected uint itemId;

    // velocity_obj = (hand_pos - obj_pos) * velocityFactor / rigidbody.mass
    private float velocityFactor = 20000f;
    private Vector3 posDelta; // posDelta = (hand_pos - obj_pos)

    private float rotationFactor = 600f;
    private Quaternion rotationDelta;
    private float angle;
    private Vector3 axis;

    // The controller this object is picked up by
    private GravityGun gravityGun;

    // The point at which the object was grabbed when picked up
    private Transform interactionPoint;

    // Use this for initialization
    protected void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        interactionPoint = new GameObject().transform;
        velocityFactor /= rigidBody.mass;
        rotationFactor /= rigidBody.mass;
    }

    // Update is called once per frame
    // TODO: Use FixedUpdate for rigidbody manipulation
    protected void Update()
    {
        if (gravityGun != null && currentlyInteracting)
        {
            posDelta = gravityGun.transform.position - interactionPoint.position;
            this.rigidBody.velocity = posDelta * velocityFactor * Time.fixedDeltaTime;

            rotationDelta = gravityGun.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
            rotationDelta.ToAngleAxis(out angle, out axis);

            if (angle > 180)
            {
                angle -= 360;
            }

            this.rigidBody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
        }
    }

    public override void onGrabbedBy(GravityGun gravityGun)
    {
        this.gravityGun = gravityGun;
        interactionPoint.position = gravityGun.transform.position;
        interactionPoint.rotation = gravityGun.transform.rotation;
        interactionPoint.SetParent(transform, true);

        currentlyInteracting = true;
    }

    public override void onDroppedBy(GravityGun gravityGun)
    {
        if (gravityGun == this.gravityGun)
        {
            this.gravityGun = null;
            currentlyInteracting = false;
        }
    }

    public bool IsInteracting()
    {
        return currentlyInteracting;
    }

    private void OnDestroy()
    {
        // Destroy the empty game object associated with interaction point
        if (interactionPoint)
        {
            Destroy(interactionPoint.gameObject);
        }
    }
}
