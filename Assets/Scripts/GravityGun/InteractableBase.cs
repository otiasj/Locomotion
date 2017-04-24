using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour {
    public virtual void onGrabbedBy(GravityGun gravityGun) { }
    public virtual void onDroppedBy(GravityGun gravityGun) { }
}
