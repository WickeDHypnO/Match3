using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRaycaster : MonoBehaviour
{
    RaycastHit hit;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray cast = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(cast.origin, cast.direction * 100);
            if (Physics2D.Raycast(cast.origin, cast.direction, 100))
            {
                if(hit.collider && hit.collider.GetComponent<Gem>())
                {
                    hit.collider.GetComponent<Gem>().SelectGem();
                }
            }
        }
    }


}
