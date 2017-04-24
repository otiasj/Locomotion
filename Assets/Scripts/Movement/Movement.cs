using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstraction layer to interface with Vive or Oculus
public interface Movement
{
    void aimFrom(Transform position);
    void moveForward();
    void enablePointer(bool enabled);
    void moveToPointer();
}