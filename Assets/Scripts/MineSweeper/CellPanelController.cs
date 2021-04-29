using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPanelController : MonoBehaviour
{
    [SerializeField] Transform m_camera;
    RaycastHit hit;
    GameObject targetcell;

    void Start()
    {
    }

    void Update()
    {
        SelectCell();
    }

    public void SelectCell()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray,  out hit))
        {
            targetcell = hit.collider.gameObject;

            if (targetcell.CompareTag("UpperCell"))
            {
                targetcell.gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
        }
        else
        {
            targetcell = null;
        }
    }
    void SetCellStates(MineSweeperSystem.CellStates State)
    {
        
    }
    void Open()
    {

    }
}
