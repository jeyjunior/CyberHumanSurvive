using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MagicStone"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                other.gameObject.GetComponent<MagicStone>().ActiveMagicStone();
            }
        }
    }
}
