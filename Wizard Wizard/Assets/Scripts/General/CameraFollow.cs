using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform toFollow;



    // Update is called once per frame
    void Update()
    {
        if(toFollow != null)
        {
            Vector3 newPos = new Vector3(toFollow.position.x, toFollow.position.y, -10);
            transform.position = newPos;
        }
    }
}
