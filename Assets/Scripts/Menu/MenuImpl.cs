using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuImpl : MonoBehaviour, Menu
{

    public void enable() {
        //Debug.Log("Menu enabled");
    }

    public void disable() {
        //Debug.Log("Menu disabled");
    }

    public void navigateUp() {
        Debug.Log("Menu up");
    }

    public void navigateDown() {
        Debug.Log("Menu down");
    }

    public void navigateLeft() {
        Debug.Log("Menu left");
    }

    public void navigateRight() {
        Debug.Log("Menu right");
    }

    public void navigateSelect() {
        Debug.Log("Menu selected");
    }
}
