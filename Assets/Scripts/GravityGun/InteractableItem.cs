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
    private Material defaultMaterial;

    // The controller this object is picked up by
    private GameObject anchorObject;

    // The point at which the object was grabbed when picked up
    private Transform interactionPoint;

    // Use this for initialization
    protected void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        interactionPoint = new GameObject().transform;
        velocityFactor /= rigidBody.mass;
        rotationFactor /= rigidBody.mass;
        defaultMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (anchorObject != null && currentlyInteracting)
        {
            posDelta = anchorObject.transform.position - interactionPoint.position;
            this.rigidBody.velocity = posDelta * velocityFactor * Time.fixedDeltaTime;

            rotationDelta = anchorObject.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
            rotationDelta.ToAngleAxis(out angle, out axis);

            if (angle > 180)
            {
                angle -= 360;
            }

            this.rigidBody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
        }
    }

    public override void onGrabbedBy(GameObject anchorObject, Material grabbedMaterial)
    {
        this.anchorObject = anchorObject;
        interactionPoint.position = this.anchorObject.transform.position;
        interactionPoint.rotation = this.anchorObject.transform.rotation;
        interactionPoint.SetParent(transform, true);
        GetComponent<Renderer>().material = grabbedMaterial;
        currentlyInteracting = true;
    }

    public override void onDroppedBy(GameObject anchorObject)
    {
        if (anchorObject == this.anchorObject)
        {
            this.anchorObject = null;
            currentlyInteracting = false;
            GetComponent<Renderer>().material = defaultMaterial;
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
