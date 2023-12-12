using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(FlagCreator), typeof(LootboxStorage))]
public class Base : MonoBehaviour
{
    [SerializeField] private LootboxScanner _scanner;

    private List<Bot> _bots = new List<Bot>();
    private FlagCreator _flagCreator;
    private Flag _flag;
    private LootboxStorage _storage;

    private Coroutine _takeLootboxCoroutine;
    private Coroutine _findBotCoroutine;

    private int _maxBotsAmount = 3;
    private bool _isBotNeededToBuild = false;

    private void Start()
    {
        GetRequiredComponents();

        _takeLootboxCoroutine = StartCoroutine(MakeBotsCollect());

        _storage.LootboxAmountChanged += OnBotMoneyEnough;
        OnBotMoneyEnough();
    }

    private void OnDestroy()
    {
        if (_takeLootboxCoroutine != null)
            StopCoroutine(_takeLootboxCoroutine);

        if (_findBotCoroutine != null)
            StopCoroutine(_findBotCoroutine);

        _storage.LootboxAmountChanged -= OnBotMoneyEnough;
    }

    public void GetScanner(LootboxScanner scanner)
    {
        _scanner = scanner;
    }

    private void GetRequiredComponents()
    {
        _flagCreator = GetComponent<FlagCreator>();
        _storage = GetComponent<LootboxStorage>();
    }

    private void OnBotMoneyEnough()
    {
        if (_bots.Count < _maxBotsAmount && _storage.TryBuyBot(_storage.LootboxAmount, out Bot bot))
        {
            AddBot(bot);
        }
    }

    public void AddBot(Bot bot)
    {
        if(_storage == null)
        {
            _storage = GetComponent<LootboxStorage>();
        }

        bot.Init(this, _scanner, _storage);
        _bots.Add(bot);
    }

    public void StartBuilding(Vector3 position)
    {
        int collectorsRequiredToBuild = 2;

        if(_bots.Count >= collectorsRequiredToBuild)
        {
            if (_flagCreator.IsFlagCreated == false)
            {
                _storage.LootboxAmountChanged -= OnBotMoneyEnough;
                _storage.LootboxAmountChanged += OnBaseMoneyEnough;
            }

            _flag = _flagCreator.Create(position);
        }
    }

    private void OnBaseMoneyEnough()
    {
        if (_storage.TryBuyBase(_storage.LootboxAmount) && _isBotNeededToBuild == false)
        {
            _isBotNeededToBuild = true;
            _findBotCoroutine = StartCoroutine(MakeBotBuild());
        }
    }

    private Queue<Bot> FindFreeBots()
    {
        IEnumerable<Bot> freeBots = _bots.Where(bot => bot.IsFree);
        Queue<Bot> queueBots = new Queue<Bot>();

        foreach (Bot bot in freeBots)
        {
            queueBots.Enqueue(bot);
        }

        return queueBots;
    }

    private IEnumerator MakeBotBuild()
    {
        bool isWorking = true;
        float botSearchCooldown = 0.5f;
        var waitForCooldown = new WaitForSeconds(botSearchCooldown);

        while (isWorking)
        { 
            Queue<Bot> freeBots = FindFreeBots();

            if(freeBots.Count > 0)
            {
                Bot bot = freeBots.Dequeue();
                bot.BuildBase(_flag);
                _bots.Remove(bot);

                _storage.LootboxAmountChanged -= OnBaseMoneyEnough;
                _storage.LootboxAmountChanged += OnBotMoneyEnough;

                isWorking = false;
                _isBotNeededToBuild = false;
            }

            yield return waitForCooldown;
        }
    }

    private IEnumerator MakeBotsCollect()
    {
        float _takeNewLootboxCooldown = 0.5f;
        var waitForCooldown = new WaitForSeconds(_takeNewLootboxCooldown);

        while (enabled)
        {
            yield return waitForCooldown;

            if (_isBotNeededToBuild)
            {
                continue;
            }

            Queue<Bot> freeBots = FindFreeBots();

            for (int i = freeBots.Count - 1; i >= 0; i--)
            {
                if (_scanner.TryGetLootbox(out Lootbox lootbox))
                {
                    Bot freeBot = freeBots.Dequeue();
                    freeBot.GatherLootbox(lootbox);
                }
            }
        }
    }
}
