using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InventoryPopupUI : PopupUI
{
    [SerializeField]
    private List<DisplayRune> _equipedRunes = new List<DisplayRune>();
    [SerializeField]
    private List<Image> _equipedRuneSynergyLines = new List<Image>();

    private DisplayRune _selectedRune;

    [SerializeField]
    private Image _selectedRuneImage;
    [SerializeField]
    private TextMeshProUGUI _selectedRuneName;
    [SerializeField]
    private TextMeshProUGUI _selectedRuneDescription;

    // Add here...

    public override void InitializePopup()
    {
        DeselectRune(_selectedRune);
        UpdateEquipedRune();
    }

    public void SelectRune(DisplayRune selectedRune)
    {
        if (!selectedRune)
        {
            return;
        }

        _selectedRuneImage.color = Color.white;
        _selectedRuneImage.sprite = selectedRune.RuneData.runeSprite;
        _selectedRuneName.text = selectedRune.RuneData.runeDisplayedName;
        _selectedRuneDescription.text = selectedRune.RuneData.runeDescription;

        selectedRune.onClickRune.RemoveListener(SelectRune);
        selectedRune.onClickRune.AddListener(DeselectRune);
    }

    public void DeselectRune(DisplayRune selectedRune)
    {
        _selectedRuneImage.color = Color.clear;
        _selectedRuneImage.sprite = null;
        _selectedRuneName.text = string.Empty;
        _selectedRuneDescription.text = string.Empty;

        selectedRune?.onClickRune.RemoveListener(DeselectRune);
        selectedRune?.onClickRune.AddListener(SelectRune);
    }

    private void UpdateEquipedRune()
    {
        for (int i = 0; i < _equipedRunes.Count; ++i)
        {
            _equipedRunes[i].RuneData = RuneManager.Instance.GetEquipedRune(i);

            if (_equipedRunes[i].RuneData)
            {
                _equipedRunes[i].onClickRune.AddListener(SelectRune);

                _equipedRunes[i].RuneImage.color = Color.white;
                _equipedRunes[i].RuneImage.sprite = _equipedRunes[i].RuneData.runeSprite;
            }
            else
            {
                _equipedRunes[i].onClickRune.RemoveListener(SelectRune);

                _equipedRunes[i].RuneImage.color = Color.clear;
                _equipedRunes[i].RuneImage.sprite = null;
            }
        }
    }
}
