using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour {
    public virtual void onGrabbedBy(GameObject anchorObject) { }
    public virtual void onDroppedBy(GameObject anchorObject) { }
}
