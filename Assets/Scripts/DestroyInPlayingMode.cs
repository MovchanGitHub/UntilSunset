using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInPlayingMode : MonoBehaviour
{
    private void Awake()
    {
        Destroy(this.gameObject);
    }
}
