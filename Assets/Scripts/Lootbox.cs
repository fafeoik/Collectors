using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootbox : MonoBehaviour
{
    public bool IsReserved { get; private set; } = false;

    public void MakeReserved()
    {
        IsReserved = true;
    }
}
