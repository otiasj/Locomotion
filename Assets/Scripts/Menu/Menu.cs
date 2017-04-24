using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstraction layer to interface with Vive or Oculus
public interface Menu
{
    void enable();
    void disable();
    void navigateUp();
    void navigateDown();
    void navigateLeft();
    void navigateRight();
    void navigateSelect();
}