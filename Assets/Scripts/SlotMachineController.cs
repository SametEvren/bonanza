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
    private readonly Dictionary<SlotItemModel, int> _slotItemsDictionary = new ();

    private IObjectPoolManager<SlotItemController> ObjectPoolManager => 
        ServiceLocator.Get<IObjectPoolManager<SlotItemController>>();

    private void Start()
    {
        SpawnSlotItems(5, reelControllers[0].transform);
    }

    private void LogItemToDictionary(SlotItemModel slotItemModel)
    {
        if (_slotItemsDictionary.ContainsKey(slotItemModel))
            _slotItemsDictionary[slotItemModel]++;
        else
            _slotItemsDictionary.Add(slotItemModel,1);
    }
    
    private SlotItemModel GenerateSlotData()
    {
        var slotType = spawnChances.EvaluateTypeValue(Random.Range(0f,1f));
        var skinIndex = spawnChances.EvaluateSkinIndex(slotType, Random.Range(0f, 1f));
        var slotModel = new SlotItemModel(slotType, skinIndex);
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
            spawnedItem.SetModel(model); //Rendering Stuff

            Tween.UIAnchoredPosition(rectTransform, bottom + new Vector2(0, i * unitHeight), 1f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ObjectPoolManager.ReleaseItemToPool(reelControllers[0].slotItemControllers[2]);
            ObjectPoolManager.ReleaseItemToPool(reelControllers[0].slotItemControllers[4]);
        }
    }
}