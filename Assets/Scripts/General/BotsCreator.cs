using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotsCreator : MonoBehaviour
{
    [SerializeField] private Bot _botPrefab;

    public Bot Create()
    {
        Bot newBot = Instantiate(_botPrefab, transform);
        return newBot;
    }
}
