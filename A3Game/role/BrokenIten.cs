using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class BrokenIten : MonoBehaviour
{
    void Start()
    {
        Rigidbody rigi = gameObject.AddComponent<Rigidbody>();
        rigi.mass = 150;

        Invoke("dispose", 3);
    }


    void dispose()
    {
        Destroy(gameObject);
    }

}

