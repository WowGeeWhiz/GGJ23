using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRenderSort : MonoBehaviour
{
    [SerializeField]
    private int sortingOrderBase = 5000;

    [SerializeField]
    private int offset = 0;

    [SerializeField]
    private bool stationary; //check if object is not supposed to move position

    private Renderer MyRenderer;
    public Vector3 bottomMarker;

    private void Awake()
    {
        MyRenderer = gameObject.GetComponent<SpriteRenderer>();

        bottomMarker = new Vector3(0, -MyRenderer.bounds.max.y, 0);
    }

    private void LateUpdate()
    {
        bottomMarker = MyRenderer.transform.position + new Vector3(0, -MyRenderer.bounds.max.y, 0);
        MyRenderer.sortingOrder = (int)(sortingOrderBase - transform.position.y - bottomMarker.y);
        if (stationary)
        {
            Destroy(this); //deletes this script from any object that is marked as stationary
        }
    }
}
