using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPosition : MonoBehaviour
{
    public Transform camPosition;

    private void Update()
    {
        transform.position = camPosition.transform.position;
    }

    //Creditos: www.youtube.com/watch?v=f473C43s8nE&ab_channel=Dave%2FGameDevelopment
}
