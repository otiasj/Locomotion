using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
** Put this Object on the vive left and/or right controller then set the InputController interface implementation.
*/
public class ViveControllerManager : MonoBehaviour {

    public SteamVR_TrackedObject viveController;
    public GameObject inputController;

    private SteamVR_Controller.Device device;
    private InputController inputControllerImpl;

    // Use this for initialization
    void Start () {
        viveController = GetComponent<SteamVR_TrackedObject>();
        inputControllerImpl = (InputController)inputController.GetComponent("ControllerInputManager");
    }
	
	// Update is called once per frame
	void Update () {
        device = SteamVR_Controller.Input((int)viveController.index);
        if (device.GetPress(SteamVR_Controller.ButtonMask.Grip))
        {
            inputControllerImpl.moveForward();
        }

        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            inputControllerImpl.enablePointer(true);
            inputControllerImpl.aimFrom(gameObject.transform);
        }

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            inputControllerImpl.enablePointer(false);
            inputControllerImpl.moveToPointer();
        }

        if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            inputControllerImpl.enableMenu();
        }

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            detectDirection();
        }

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            inputControllerImpl.disableMenu();
        }
    }

    Vector2 touchpadLoc;
    private void detectDirection()
    {
        //Read the touchpad values
        touchpadLoc = device.GetAxis();
        //Debug.Log(touchpadLoc);

        double targetUpXOld = 0.0; //Old target up value.
        float targetUpX = (float)targetUpXOld;

        double targetUpYOld = 0.8;
        float targetUpY = (float)targetUpYOld;

        Vector2 targetUp = new Vector2(targetUpX, targetUpY);

        if (Vector2.Distance(touchpadLoc, targetUp) < .15) //If the distance between finger (touchpadLoc) and the up zone (targetUp) is more than 0 do something.
        {
            Debug.Log("Up position met.");
        }

        double targetDownXOld = -0.0; //Old target down value.
        float targetDownX = (float)targetDownXOld;

        double targetDownYOld = -0.8;
        float targetDownY = (float)targetDownYOld;

        Vector2 targetDown = new Vector2(targetUpX, targetUpY);

        if (Vector2.Distance(touchpadLoc, targetDown) > .15)
        {
            Debug.Log("Down position met.");
        }

        double targetLeftXOld = -0.9; //Old target left value.
        float targetLeftX = (float)targetLeftXOld;

        double targetLextYOld = -0.0;
        float targetLeftY = (float)targetLextYOld;

        Vector2 targetLeft = new Vector2(targetLeftX, targetLeftY);

        if (Vector2.Distance(touchpadLoc, targetLeft) > .15)
        {
            Debug.Log("Left position met.");
        }

        double targetRightXOld = 0.9; //Old target Right value.
        float targetRightX = (float)targetRightXOld;

        double targetRightYOld = 0.0;
        float targetRightY = (float)targetRightYOld;

        Vector2 targetRight = new Vector2(targetRightX, targetRightY);

        if (Vector2.Distance(touchpadLoc, targetRight) > .15)
        {
            Debug.Log("Right position met.");
        }
    }

}
