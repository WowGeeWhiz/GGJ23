using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    //attach to player

    Camera cam;

    public Texture2D sawCursor, flameCursor, cancelCursor, normalCursor;

    float material; //the amount of building material the player has

    //bool for if the build mode is active
    internal bool buildModeActive;
    internal bool removeModeActive;
    private bool validPlacement;

    private PlayerController player;

    GameObject structurePrefab;
    float structureCost;

    public GameObject objectToPlace, tempObject, selectedObject, tempSelectedObject;
    public GameObject sawPrefab, flamethrowerPrefab;
    public Saw permaSaw;
    public Flamethrower permaFlame;

    void Start()
    {
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
        cam = Camera.main;

        validPlacement = true;
        player = GetComponent<PlayerController>();
    }


    void Update()
    {
        material = player.wood;

        if (player.lockMovement && !player.GodMode) return;

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
            ActivateRemoveMode();
        }

        /*if(cursor is in bounds && not inside the radius of another structure)
        {
            validPlacement == true;
        }*/

        if (buildModeActive && Input.GetMouseButtonDown(0))
        {
            Placement(structurePrefab);
        }

        if(removeModeActive && Input.GetMouseButtonDown(0))
        {
            RemoveTower();
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
        if (removeModeActive)
        {
            CancelRemoveMode();
        }

        if (buildModeActive) //makes it so that if its already in build mode, pressing the button a second time will close the build mode
        {
            CancelBuildMode();
        }
        else if (!buildModeActive)
        {
            buildModeActive = true;


            switch (structure)
            {
                case "saw":
                    structurePrefab = sawPrefab;
                    structureCost = permaSaw.cost;
                    Cursor.SetCursor(sawCursor, Vector2.zero, CursorMode.Auto);
                    break;
                case "flamethrower":
                    structurePrefab = flamethrowerPrefab;
                    structureCost = permaFlame.cost;
                    Cursor.SetCursor(flameCursor, Vector2.zero, CursorMode.Auto);
                    break;
            }
            /*mouse cursor becomes tempObject
            */
        }
    }

    public void Placement(GameObject structurePrefab)
    {
        if (!validPlacement)
        {
            Debug.Log("You cannot place that here.");
        }
        else
        {
            if (structureCost > material)
            {
                Debug.Log("You do not have enough material.");
            }
            else if (structureCost <= material)
            {
                //Destroy(tempObject);
               GameObject temp = Instantiate(structurePrefab, GetMousePos(), Quaternion.identity);
                player.wood -= (int)structureCost;


                var tempSaw = temp.GetComponent<Saw>();
                var tempFlame = temp.GetComponent<Flamethrower>();
                if (tempSaw != null || tempFlame != null)
                {
                    tempSaw.player = player;
                }
            }
        }
    }

    public void ActivateRemoveMode()
    {
        if (buildModeActive) //makes it so that if its already in build mode, pressing the button will close the build mode
        {
            CancelBuildMode();
        }

        if (removeModeActive)
        {
            CancelRemoveMode();
        }
        else if (!removeModeActive)
        {
            removeModeActive = true;
            Cursor.SetCursor(cancelCursor, Vector2.zero, CursorMode.Auto);
            //mouse cursor becomes x
        }
    }


    public void RemoveTower()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            if(hit.collider.gameObject.tag == "Tower")
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }

    public void CancelBuildMode()
    {
        //cancel the build mode
        buildModeActive = false;
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
    }

    public void CancelRemoveMode()
    {
        //cancel the remove mode
        removeModeActive = false;
    }
}
