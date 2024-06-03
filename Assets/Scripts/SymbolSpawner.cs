using System;
using System.Collections.Generic;
using GameplayControllers;
using Reel;
using SlotItem;
using SymbolScriptables;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

public class SymbolSpawner : MonoBehaviour
{
    public Action<SymbolType> onSpawnedItem;
    
    private IObjectPoolManager<SlotItemController> ObjectPoolManager => 
        ServiceLocator.Get<IObjectPoolManager<SlotItemController>>();
    
    [SerializeField] private SpawnChances spawnChances;
    [SerializeField] private SymbolLibrary symbolLibrary;
    [SerializeField] private FreeSpinController freeSpinController;

    private void Awake()
    {
        ServiceLocator.Add(this);
    }

    public List<SlotItemController> SpawnOnTop(ReelController reelController, int amount)
    {
        var bottom = new Vector2(0, 5);
        var spawnedItems = new List<SlotItemController>();
        
        for (int i = 0; i < amount; i++)
        {
            var spawnedItem = ObjectPoolManager.GetItemFromPool();
            var rectTransform = spawnedItem.GetComponent<RectTransform>();

            var unitHeight = rectTransform.sizeDelta.y;
            spawnedItem.transform.SetParent(reelController.transform);
            rectTransform.anchoredPosition =
                bottom + Vector2.up * (unitHeight * (5 + i));
            reelController.slotItemControllers.Add(spawnedItem);
            var model = GenerateSymbolData();
            spawnedItem.SetModel(model); // Rendering stuff
            spawnedItems.Add(spawnedItem);
            onSpawnedItem?.Invoke(spawnedItem.SymbolType);
        }

        return spawnedItems;
    }
    
    private SymbolData GetRandomSymbolData(SlotItemType slotType)
    {
        var rand = Random.Range(0f, 1f);
        return symbolLibrary.GetTargetSymbol(slotType, rand);
    }
    
    private SlotItemModel GenerateSymbolData()
    {
        var slotType = spawnChances.EvaluateTypeValue(Random.Range(0f, 1f));
        
        if (slotType == SlotItemType.Multiplier)
        {
            slotType = freeSpinController.IsFreeSpinActive ? SlotItemType.Multiplier : SlotItemType.Common;
        }
        
        var symbolData = GetRandomSymbolData(slotType);
        var slotModel = new SlotItemModel(slotType, symbolData);
        return slotModel;
    }
}