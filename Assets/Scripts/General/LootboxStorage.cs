using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LootboxStorage : MonoBehaviour
{
    [SerializeField] private BotsCreator _botsCreator;
    [SerializeField] private int _botPrice;
    [SerializeField] private int _basePrice;
    [SerializeField] private int _lootboxAmount;

    public event UnityAction LootboxAmountChanged;

    public int BotPrice => _botPrice;
    public int BasePrice => _basePrice;
    public int LootboxAmount => _lootboxAmount;

    public void ChangeLootboxAmount(int amount)
    {
        _lootboxAmount += amount;
        LootboxAmountChanged?.Invoke();
    }

    public bool TryBuyBot(int money, out Bot bot)
    {
        if (money >= _botPrice)
        {
            bot = _botsCreator.Create();
            ChangeLootboxAmount(-_botPrice);
            return true;
        }

        bot = null;
        return false;
    }

    public bool TryBuyBase(int money)
    {
        if (money >= _basePrice)
        {
            ChangeLootboxAmount(-_basePrice);
            return true;
        }

        return false;
    }
}
