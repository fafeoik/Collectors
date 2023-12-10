using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootbox : MonoBehaviour
{
    public bool IsDetected { get; private set; } = false;

    public void MakeDetected()
    {
        IsDetected = true;
    }
}
