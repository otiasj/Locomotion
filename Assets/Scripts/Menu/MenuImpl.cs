using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuImpl : MonoBehaviour, Menu
{
    public GameObject objectMenu;
    public GameObject slot0;
    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;

    private int currentPosition = 0;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private GameObject[] slotList = new GameObject[4];

    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    private float lerpTime;
    private float menuSpeed = 2f;

    public void Start()
    {
        slotList[0] = slot0;
        slotList[1] = slot1;
        slotList[2] = slot2;
        slotList[3] = slot3;
        startPosition = slot0.transform.position;
        endPosition = slot3.transform.position;
    }

    public void Update()
    {
      moveMenu();
    }

    public void enable() {
        //Debug.Log("Menu enabled");
        objectMenu.SetActive(true);
    }

    public void disable() {
        //Debug.Log("Menu disabled");
        objectMenu.SetActive(false);
    }

    public void navigateUp() {
        Debug.Log("Menu up");
    }

    public void navigateDown() {
        Debug.Log("Menu down");
    }

    public void navigateLeft() {
        Debug.Log("Menu left");
        if (!isMovingRight)
        {
            currentPosition -= 1;
            if (currentPosition < 0)
            {
                currentPosition = 0;
            }
            else
            {
                isMovingLeft = false;
                isMovingRight = true;
            }
        }
    }

    public void navigateRight() {
        Debug.Log("Menu right");
        if (!isMovingLeft)
        {
            currentPosition += 1;
            if (currentPosition > slotList.Length - 1)
            {
                currentPosition = slotList.Length - 1;
            }
            else
            {
                isMovingLeft = true;
                isMovingRight = false;
            }
        }
    }

    public void navigateSelect() {
        Debug.Log("Menu selected " + currentPosition);
    }

    private void moveMenu()
    {
        if (isMovingLeft)
        {
            foreach (GameObject slot in slotList)
            {
                slot.transform.Translate(Vector2.left * menuSpeed * Time.deltaTime);
            }
           
        } else if (isMovingRight)
        {
            foreach (GameObject slot in slotList)
            {
                slot.transform.Translate(Vector2.right * menuSpeed * Time.deltaTime);
            }

        }
    }
     
}
