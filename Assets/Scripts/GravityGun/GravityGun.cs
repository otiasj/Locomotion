using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstraction layer to interface with Vive or Oculus
public interface GravityGun
{
    void grab();
    void drop();
    void onTriggerEnter(InteractableBase collidingItem);
    void onTriggerExit(InteractableBase collidingItem);

}