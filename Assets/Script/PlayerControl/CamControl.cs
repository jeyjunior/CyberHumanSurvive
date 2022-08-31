using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public float distance;
    public Transform playerPosition;

    void Update()
    {
        transform.position = new Vector3(playerPosition.position.x, playerPosition.position.y + distance, playerPosition.position.z - distance);    
    }
}
