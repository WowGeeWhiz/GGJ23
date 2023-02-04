using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    //attach to player

    Camera cam;

    float material; //the amount of building material the player has

    //bool for if the build mode is active
    private bool buildModeActive, validPlacement;

    GameObject structurePrefab;
    float structureCost;

    public GameObject objectToPlace, tempObject, selectedObject, tempSelectedObject;
    public GameObject sawPrefab, flamethrowerPrefab;

    void Start()
    {
        cam = Camera.main;
        material = 10000; //placeholder

        validPlacement = true;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateBuilding("saw");
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ActivateBuilding("flamethrower");
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            RemoveTower();
        }

        /*if(cursor is in bounds && not inside the radius of another structure)
        {
            validPlacement == true;
        }*/

        if(buildModeActive && Input.GetMouseButtonDown(0))
        {
            Placement(structurePrefab);
        }

    }

    public Vector3 GetMousePos()
    {
        Vector3 currentMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        currentMousePos.z = 0;
        return currentMousePos;
    }

    public void ActivateBuilding(string structure)
    {
        if (buildModeActive) //makes it so that if its already in build mode, pressing the button a second time will close the build mode
        {
            CancelBuildMode();
        }
        else if (!buildModeActive)
        {
            buildModeActive = true;
            Debug.Log("Entering Build Mode");


            switch (structure)
            {
                case "saw":
                    structurePrefab = sawPrefab;
                    structureCost = 20;
                    break;
                case "flamethrower":
                    structurePrefab = flamethrowerPrefab;
                    structureCost = 30;
                    break;
            }
            Debug.Log("1");
            /*mouse cursor becomes tempObject
            */
        }
    }

    public void Placement(GameObject structurePrefab)
    {
        Debug.Log("2");
        if (!validPlacement)
        {
            Debug.Log("You cannot place that here.");
        }
        else if (validPlacement)
        {
            if (structureCost > material)
            {
                Debug.Log("You do not have enough material.");
            }
            else if (structureCost <= material)
            {
                //Destroy(tempObject);
                Instantiate(structurePrefab, GetMousePos(), Quaternion.identity);
                Debug.Log("placing " + structurePrefab.name);
            }
        }
    }


    public void RemoveTower()
    {

    }

    public void CancelBuildMode()
    {
        Debug.Log("Exiting Build Mode");
        //cancel the build mode
        buildModeActive = false;
    }
}
