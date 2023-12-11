using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseBuilder), typeof(CollectorMover), typeof(LootboxGatherner))]
public class Collector : MonoBehaviour
{
    private BaseBuilder _baseBuilder;
    private LootboxGatherner _gatherner;

    public bool IsFree { get; private set; } = true;

    private void Awake()
    {
        _baseBuilder = GetComponent<BaseBuilder>();
        _gatherner = GetComponent<LootboxGatherner>();
    }

    public void Init(Base collectorBase,LootboxScanner scanner)
    {
        _gatherner.Init(collectorBase);
        _baseBuilder.Init(scanner);
    }

    public void GatherLootbox(Lootbox targetLootbox)
    {
        IsFree = false;

        _gatherner.LootboxGathered += BecomeFree;
        _gatherner.StartGathering(targetLootbox);
    }

    public void BuildBase(Flag flag)
    {
        IsFree = false;

        _baseBuilder.BuildCompleted += BecomeFree;
        _baseBuilder.StartBuilding(flag);
    }

    private void BecomeFree() => IsFree = true;
}