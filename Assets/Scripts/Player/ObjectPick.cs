using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPick : MonoBehaviour
{
    public Camera cam;

    Pickable pickedObject;
    
    void disableRigidbody(Pickable obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // rb.detectCollisions = false;
            rb.useGravity = false;
        }
    }

    void enableRigidbody(Pickable obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
            
        if (rb != null)
        {
            // rb.detectCollisions = true;
            rb.useGravity = true;
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            if (pickedObject == null)
            {
                RaycastHit hit;

                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 5))
                {  
                    Pickable pk = hit.collider.GetComponent<Pickable>();
                    if(pk)
                    {
                        pickedObject = pk;
                        disableRigidbody(pickedObject);
                    }
                }
            }
            else
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                float distance = 5;
                
                RaycastHit[] hit = Physics.RaycastAll(ray, 5);

                if (hit.Length > 0)
                {   
                    for(int i = 0; i < hit.Length; i++)
                    {
                        if(hit[i].collider.gameObject != pickedObject.gameObject)
                        {
                            distance = hit[i].distance;
                        }
                    }
                }

                Transform tf = pickedObject.transform;
                
                Vector3 position = transform.position + (ray.direction * distance);

                tf.SetPositionAndRotation(position, Quaternion.identity);

            }
        }
        else if(pickedObject != null)
        {
            enableRigidbody(pickedObject);
            pickedObject = null;
        }
    }
}
