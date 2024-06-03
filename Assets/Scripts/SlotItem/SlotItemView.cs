using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlotItem
{
    [RequireComponent(typeof(Image))]
    public class SlotItemView : MonoBehaviour
    {
        private Image _image;
        [SerializeField] private TextMeshProUGUI multiplierText;
        
        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void Render(SlotItemModel slotItemModel)
        {
            gameObject.SetActive(true);

            if (slotItemModel.slotSymbolData.symbolType == SymbolType.Multiplier)
            {
                multiplierText.gameObject.SetActive(true);
                multiplierText.text = "X" + slotItemModel.multiplierAmount;
            }
            else
            {
                multiplierText.gameObject.SetActive(false);
            }

            _image.sprite = slotItemModel.slotSymbolData.sprite;
        }
    }
}