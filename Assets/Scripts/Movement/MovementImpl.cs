using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Everything related to moving the player around the play area
public class MovementImpl : MonoBehaviour, Movement {

    //navigation pointer
    public LineRenderer laser;
    public GameObject teleporter;
    public LayerMask laserMask;

    //The Player
    public GameObject player;
    public Transform playerCam;

    public float yNudgeAmount = 0.5f; //move the cursor just up enough to not collide with the ground
    public int laserRange = 15;
    public bool useInstantTeleportation = false;

    //movement speeds
    public float dashSpeed = 0.1f;
    public float movementSpeed = 4f;

    //teleportation or dashing
    private bool isDashing;
    private float lerpTime;
    private Vector3 targetLocation;
    private Vector3 dashStartPosition;

    //walking
    private Vector3 movementDirection;
    private Vector3 movement;

    // Use this for initialization
    void Start()
    {
        laser.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            updateDashingPosition();
        }
    }

    private void updateDashingPosition()
    {
        Debug.Log("Start Position " + dashStartPosition + " dashing " + lerpTime + "->" + targetLocation);
        lerpTime += 1 * dashSpeed;
        player.transform.position = Vector3.Lerp(dashStartPosition, targetLocation, lerpTime);
        if (lerpTime >= 1)
        {
            player.transform.position = targetLocation;
            isDashing = false;
            lerpTime = 0;
            Debug.Log("End Position " + player.transform.position + "->" + targetLocation);
        }
    }

    //InputController Implementation
    public void aimFrom(Transform transformOrigin)
    {
        if (isDashing)
        {
            return;
        }
        
        //set the laser origin location
        laser.SetPosition(0, transformOrigin.position);

        //find the object colliding with our laser
        RaycastHit hit;
        if (Physics.Raycast(transformOrigin.position, transformOrigin.forward, out hit, laserRange, laserMask))
        {
            // the collision point
            Vector3 hitpoint = hit.point;

            // the laser pointer end point
            laser.SetPosition(1, hitpoint);

            // the teleport position (yNudgeAmount above the hit object)
            teleporter.transform.position = new Vector3(hitpoint.x,
                hitpoint.y + yNudgeAmount,
                hitpoint.z);
        }
        /*else { //if we do not hit anything, 

			// find the location on ground
			targetLocation = new Vector3 (transformOrigin.forward.x * laserRange + transformOrigin.position.x,
                transformOrigin.position.y * laserRange + transformOrigin.position.y,
                transformOrigin.forward.z * laserRange + transformOrigin.position.z);

			//find the ground point down at laserRange away, by sending a raycast vertically down
			RaycastHit groundRay;
			if (Physics.Raycast (targetLocation, -Vector3.up, out groundRay, laserRange, laserMask)) {
				targetLocation = new Vector3(transformOrigin.forward.x * laserRange + transformOrigin.position.x,
					groundRay.point.y,
                    transformOrigin.forward.z * laserRange + transformOrigin.position.z);
			}

			//laser end point
			laser.SetPosition (1, transformOrigin.forward * laserRange + transformOrigin.position);

			// the new position
			teleporter.transform.position = targetLocation + new Vector3 (0, yNudgeAmount, 0);
		}*/
    }

    public void moveForward()
    {
        movementDirection = playerCam.transform.forward;
        movementDirection = new Vector3(movementDirection.x, 0, movementDirection.z);
        movement = movementDirection * movementSpeed * Time.deltaTime;
        player.transform.position += movement;
    }

    public void enablePointer(bool enabled)
    {
        if (laser.gameObject.activeInHierarchy != enabled)
        {
            laser.gameObject.SetActive(enabled);
            teleporter.SetActive(enabled);
        }
    }

    public void moveToPointer()
    {
        Vector3 cameraFromRigCenter = player.transform.position - playerCam.position;
        targetLocation = teleporter.transform.position + cameraFromRigCenter;
        targetLocation.y = 0;
        if (useInstantTeleportation)
        {
            player.transform.position = teleporter.transform.position;
        }
        else
        {
            dashStartPosition = player.transform.position;
            isDashing = true;
        }
    }
}
