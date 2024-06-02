using System.Collections.Generic;
using PrimeTween;
using Reel;
using SlotItem;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

public class SlotMachineController : MonoBehaviour
{
    [SerializeField] private SpawnChances spawnChances;
    [SerializeField] private List<ReelController> reelControllers;
    [SerializeField] private SerializableDictionary<SlotItemModel, int> _slotItemsDictionary = new();
    private readonly List<int> _removedIndices = new();

    private IObjectPoolManager<SlotItemController> ObjectPoolManager => 
        ServiceLocator.Get<IObjectPoolManager<SlotItemController>>();

    private void Start()
    {
        foreach (var reelController in reelControllers)
        {
            SpawnSlotItems(5, reelController.transform);
        }
    }

    private void LogItemToDictionary(SlotItemModel slotItemModel)
    {
        if (_slotItemsDictionary.ContainsKey(slotItemModel))
        {
            _slotItemsDictionary[slotItemModel]++;
        }
        else
        {
            _slotItemsDictionary.Add(slotItemModel, 1);
        }
    }

    private void RemoveFromDictionary(SlotItemModel slotItemModel)
    {
        if (_slotItemsDictionary.ContainsKey(slotItemModel))
        {
            if (_slotItemsDictionary[slotItemModel] > 1)
            {
                _slotItemsDictionary[slotItemModel]--;
            }
            else
            {
                _slotItemsDictionary.Remove(slotItemModel);
            }
        }
    }

    private SlotItemModel GenerateSlotData()
    {
        var slotType = spawnChances.EvaluateTypeValue(Random.Range(0f, 1f));
        var symbolIndex = spawnChances.EvaluateSymbolIndex(slotType, Random.Range(0f, 1f));
        var slotModel = new SlotItemModel(slotType, symbolIndex);
        return slotModel;
    }

    public void SpawnSlotItems(int amount, Transform reelTransform)
    {
        var bottom = new Vector2(0, 5);
        var reelController = reelTransform.GetComponent<ReelController>();

        for (int i = 0; i < amount; i++)
        {
            var spawnedItem = ObjectPoolManager.GetItemFromPool();
            var rectTransform = spawnedItem.GetComponent<RectTransform>();

            var unitHeight = rectTransform.sizeDelta.y;
            spawnedItem.transform.SetParent(reelTransform);
            rectTransform.anchoredPosition =
                bottom + Vector2.up * (unitHeight * (5 + i));
            reelController.slotItemControllers.Add(spawnedItem);
            var model = GenerateSlotData();
            LogItemToDictionary(model);
            spawnedItem.SetModel(model); // Rendering stuff

            Tween.UIAnchoredPosition(rectTransform, bottom + new Vector2(0, i * unitHeight), 1f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            CheckAndRemoveItems();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (var reelController in reelControllers)
            {
                ClearAndRespawnReel(reelController);
            }
        }
    }

    private void CheckAndRemoveItems()
    {
        List<SlotItemModel> itemsToRemove = new List<SlotItemModel>();

        foreach (var entry in _slotItemsDictionary)
        {
            if (entry.Value >= 8)
            {
                itemsToRemove.Add(entry.Key);
            }
        }

        bool anyRemoved = false;

        foreach (var reelController in reelControllers)
        {
            List<int> indicesToRemove = new List<int>();

            for (int i = 0; i < reelController.slotItemControllers.Count; i++)
            {
                var slotItemController = reelController.slotItemControllers[i];
                if (itemsToRemove.Contains(slotItemController.Model))
                {
                    indicesToRemove.Add(i);
                }
            }

            if (indicesToRemove.Count > 0)
            {
                DetermineAndRemoveSlotItems(reelController, indicesToRemove);
                RefillSlotItems(reelController);
                anyRemoved = true;
            }
        }

        if (!anyRemoved)
        {
            foreach (var reelController in reelControllers)
            {
                ClearAndRespawnReel(reelController);
            }
        }
    }

    private void ClearAndRespawnReel(ReelController reelController)
    {
        foreach (var slotItem in reelController.slotItemControllers)
        {
            RemoveFromDictionary(slotItem.GetComponent<SlotItemController>().Model);
            ObjectPoolManager.ReleaseItemToPool(slotItem);
        }

        reelController.slotItemControllers.Clear();

        SpawnSlotItems(5, reelController.transform);
    }

    private void DetermineAndRemoveSlotItems(ReelController reelController, List<int> indicesToRemove)
    {
        indicesToRemove.Sort();

        _removedIndices.Clear();
        _removedIndices.AddRange(indicesToRemove);

        for (int i = _removedIndices.Count - 1; i >= 0; i--)
        {
            int index = _removedIndices[i];
            if (index < 0 || index >= reelController.slotItemControllers.Count)
            {
                _removedIndices.RemoveAt(i);
                continue;
            }

            var slotItem = reelController.slotItemControllers[index];
            RemoveFromDictionary(slotItem.GetComponent<SlotItemController>().Model);
            ObjectPoolManager.ReleaseItemToPool(slotItem);
            reelController.slotItemControllers.RemoveAt(index);
        }

        var bottom = new Vector2(0, 5);
        for (int i = 0; i < reelController.slotItemControllers.Count; i++)
        {
            var rectTransform = reelController.slotItemControllers[i].GetComponent<RectTransform>();
            var unitHeight = rectTransform.sizeDelta.y;
            var newPosition = new Vector2(rectTransform.anchoredPosition.x, bottom.y + (unitHeight * i));
            Tween.UIAnchoredPosition(rectTransform, newPosition, 1f);
        }
    }

    private void RefillSlotItems(ReelController reelController)
    {
        var bottom = new Vector2(0, 5);

        foreach (var index in _removedIndices)
        {
            var spawnedItem = ObjectPoolManager.GetItemFromPool();
            var rectTransform = spawnedItem.GetComponent<RectTransform>();
            var unitHeight = rectTransform.sizeDelta.y;

            var currentTopPosition = bottom + Vector2.up * (reelController.slotItemControllers.Count * unitHeight);
            var nearestSpawnPosition = bottom + Vector2.up * (6 * unitHeight);
            var spawnPosition = nearestSpawnPosition + Vector2.up * (unitHeight * index);

            spawnedItem.transform.SetParent(reelController.transform);
            rectTransform.anchoredPosition = spawnPosition;
            reelController.slotItemControllers.Add(spawnedItem);

            var model = GenerateSlotData();
            LogItemToDictionary(model);
            spawnedItem.SetModel(model); // Rendering stuff

            Tween.UIAnchoredPosition(rectTransform, currentTopPosition, 1f);
        }

        _removedIndices.Clear();
    }
}
