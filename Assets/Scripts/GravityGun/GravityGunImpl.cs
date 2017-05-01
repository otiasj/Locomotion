using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGunImpl : MonoBehaviour, GravityGun
{
    public Material onGrabMaterial;
    //public Collider selfCollider;
    public GameObject anchorObject; //where the grabbed object will be anchored
    private HashSet<InteractableBase> collidingObjects = new HashSet<InteractableBase>();
    private InteractableBase closestItem;
    private InteractableBase interactingItem;

    public void grab()
    {
        //selfCollider.enabled = false;
        // Find the closest item to the hand in case there are multiple and interact with it
        float minDistance = float.MaxValue;

        float distance;
        foreach (InteractableBase item in collidingObjects)
        {
            distance = (item.transform.position - transform.position).sqrMagnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                closestItem = item;
            }
        }

        interactingItem = closestItem;
        closestItem = null;

        if (interactingItem)
        {
            interactingItem.onGrabbedBy(anchorObject, onGrabMaterial);
        }
    }

    public void drop()
    {
        //selfCollider.enabled = true;
        if (interactingItem != null)
        {
            interactingItem.onDroppedBy(anchorObject);
        }
    }

    public void onTriggerEnter(InteractableBase collidingItem)
    {
        collidingObjects.Add(collidingItem);
    }

    public void onTriggerExit(InteractableBase collidingItem)
    {
        collidingObjects.Remove(collidingItem);
    }

}
