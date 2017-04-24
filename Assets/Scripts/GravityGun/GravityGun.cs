using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstraction layer to interface with Vive or Oculus
public abstract class GravityGun : MonoBehaviour
{
    void grab(GameObject collidingObject) { }
    void drop() { }
}