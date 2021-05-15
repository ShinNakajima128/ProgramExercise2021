using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPanelController : MonoBehaviour
{
    [SerializeField] Transform m_camera;
    RaycastHit hit;
    GameObject targetcell;
    [SerializeField] LayerMask targetLayer;
    float maxDistance = 30.0f;

    
    void Update()
    {
        SelectCell();
    }

    public void SelectCell()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray,  out hit, maxDistance, targetLayer))
        {
            targetcell = hit.collider.gameObject;

            targetcell.gameObject.GetComponent<Renderer>().material.color = Color.red;
            
            if (targetcell.activeSelf && Input.GetMouseButtonDown(0))
            {
                targetcell.SetActive(false);
            }
        }
        else
        {
            if (targetcell != null)
            {
                targetcell.GetComponent<Renderer>().material.color  = Color.green;
            }
            targetcell = null;
        }
    }
    
    void Open()
    {

    }
}
