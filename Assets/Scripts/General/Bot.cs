using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseBuilder), typeof(LootboxCollector))]
public class Bot : MonoBehaviour
{
    private BaseBuilder _baseBuilder;
    private LootboxCollector _collector;

    public bool IsFree { get; private set; } = true;

    private void Awake()
    {
        _baseBuilder = GetComponent<BaseBuilder>();
        _collector = GetComponent<LootboxCollector>();
    }

    private void OnDisable()
    {
        _baseBuilder.BuildCompleted -= OnBuildComplete;
        _collector.LootboxCollected -= OnLootboxCollected;
    }

    public void Init(Base collectorBase, LootboxScanner scanner, LootboxStorage storage)
    {
        _collector.Init(collectorBase, storage);
        _baseBuilder.Init(scanner);
    }

    public void GatherLootbox(Lootbox targetLootbox)
    {
        IsFree = false;

        _collector.LootboxCollected += OnLootboxCollected;
        _collector.StartCollecting(targetLootbox);
    }

    public void BuildBase(Flag flag)
    {
        IsFree = false;

        _baseBuilder.BuildCompleted += OnBuildComplete;
        _baseBuilder.StartBuilding(flag);
    }


    private void OnLootboxCollected() => IsFree = true;

    private void OnBuildComplete(Base newBase)
    {
        IsFree = true;
        newBase.AddBot(this);
    }
}