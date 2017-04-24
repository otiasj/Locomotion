using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
** Put this Object on the vive left and/or right controller then set the InputController interface implementation.
*/
public class ViveController : GravityGun
{

    private static ulong TRIGGER = SteamVR_Controller.ButtonMask.Trigger;
    private static ulong GRIPS = SteamVR_Controller.ButtonMask.Grip;
    private static ulong TOUCHPAD = SteamVR_Controller.ButtonMask.Touchpad;

    public SteamVR_TrackedObject viveController;
    public GameObject movementController;
    public GameObject menuController;
    public bool isNavigationController;

    private SteamVR_Controller.Device device;
    private Vector2 touchpadPressLocation;

    private Movement movement;
    private Menu menu;

    private HashSet<InteractableBase> objectsHoveringOver = new HashSet<InteractableBase>();
    private InteractableBase closestItem;
    private InteractableBase interactingItem;

    // Use this for initialization
    void Start()
    {
        movement = (Movement)movementController.GetComponent("MovementImpl");
        menu = (Menu)menuController.GetComponent("MenuImpl");
    }

    // Update is called once per frame
    void Update()
    {
        device = SteamVR_Controller.Input((int)viveController.index);
        handleGrips();
        handleTriggers();
        handleTouchpads();
    }

    private void handleGrips() {
        if (device.GetPress(GRIPS))
        {
            movement.moveForward();
        }
    }
    
    private void handleTriggers() {
        if (device.GetPressDown(TRIGGER))
        {
            // Find the closest item to the hand in case there are multiple and interact with it
            float minDistance = float.MaxValue;

            float distance;
            foreach (InteractableBase item in objectsHoveringOver)
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
                if (device.GetPressDown(TRIGGER))
                {
                    interactingItem.onGrabbedBy(this);
                }
            }
        }

        if (device.GetPressUp(TRIGGER) && interactingItem != null)
        {
            interactingItem.onDroppedBy(this);
        }
    }

    private void handleTouchpads() { 
        if (device.GetTouch(TOUCHPAD))
        {
            if (isNavigationController)
            {
                movement.enablePointer(true);
                movement.aimFrom(viveController.transform);
            }
            else
            {
                menu.enable();
            }
        }

        if (device.GetPressDown(TOUCHPAD))
        {
            if (isNavigationController)
            {
                movement.moveToPointer();
            }
            else
            {
                this.onMenuPress();
            }
        }

        if (device.GetTouchUp(TOUCHPAD))
        {
            if (isNavigationController)
            {
                movement.enablePointer(false);
            }
            else
            {
                menu.disable();
            }
        }
    }



    //Collision detection with some objects
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("OnTriggerEnter " + collider);
        InteractableBase collidingItem = collider.GetComponent<InteractableBase>();
        if (collidingItem)
        {
            Debug.Log("Adding item "+collidingItem);
            objectsHoveringOver.Add(collidingItem);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        Debug.Log("OnTriggerExit " + collider);
        InteractableBase collidingItem = collider.GetComponent<InteractableBase>();
        if (collidingItem)
        {
            Debug.Log("Remove item " + collidingItem);
            objectsHoveringOver.Remove(collidingItem);
        }
    }
    
    //Handle touchpad presses
    private void onMenuPress()
    {
        //Read the touchpad values
        touchpadPressLocation = device.GetAxis();
        //Debug.Log(touchpadLoc);

        //Simple center click
        if ((touchpadPressLocation.x >= -0.5) && (touchpadPressLocation.x <= 0.5) &&
            (touchpadPressLocation.y >= -0.5) && (touchpadPressLocation.y <= 0.5))
        {
            menu.navigateSelect();
            return;
        }

        //Diagonals
        if ((touchpadPressLocation.x > 0.5) && (touchpadPressLocation.y > 0.5) ||    //top right
            (touchpadPressLocation.x < -0.5) && (touchpadPressLocation.y < -0.5) ||  //bottom left
            (touchpadPressLocation.x > 0.5) && (touchpadPressLocation.y < -0.5) ||   //bottom right
            (touchpadPressLocation.x < -0.5) && (touchpadPressLocation.y > 0.5))     //top left
        {
            //Diagonals are dead zones, don't do anything if pressed there
        }

        if (touchpadPressLocation.y > 0.5)
        {
            menu.navigateUp();
        }

        if (touchpadPressLocation.y < -0.5)
        {
            menu.navigateDown();
        }

        if (touchpadPressLocation.x < -0.5)
        {
            menu.navigateLeft();
        }

        if (touchpadPressLocation.x > 0.5)
        {
            menu.navigateRight();
        }
    }

}
