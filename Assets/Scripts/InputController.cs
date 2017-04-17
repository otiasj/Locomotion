using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstraction layer to interface with Vive or Oculus
public interface InputController
{
    //movement
    void aimFrom(Transform position);
    void moveForward();
    void enablePointer(bool enabled);
    void moveToPointer();

    //Grabbing stuff
    void grab();
    void release();

    //menu
    void enableMenu();
    void disableMenu();
    void menuUp();
    void menuDown();
    void menuLeft();
    void menuRight();
    void menuSelect();
}