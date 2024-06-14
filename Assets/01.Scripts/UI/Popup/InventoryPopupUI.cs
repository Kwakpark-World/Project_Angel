using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopupUI : PopupUI
{
    [SerializeField]
    private List<RuneDisplay> _equipedRunes = new List<RuneDisplay>();
    [SerializeField]
    private List<Image> _equipedRuneSynergyLines = new List<Image>();

    private RuneDisplay _selectedRune;

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

    public override void TogglePopup(bool value)
    {
        /*if (value && SceneManager.GetActiveScene().name != "GameScene")
        {
            return;
        }*/

        base.TogglePopup(value);
    }

    public void SelectRune(RuneDisplay selectedRune)
    {
        if (!selectedRune)
        {
            return;
        }

        _selectedRuneImage.color = Color.white;
        _selectedRuneImage.sprite = selectedRune.RuneData.runeSprite;
        _selectedRuneName.text = selectedRune.RuneData.runeDisplayedName;
        //_selectedRuneDescription.text = selectedRune.RuneData.runeDescription;
        _selectedRuneDescription.text = "설명 준비 중...";

        selectedRune.onClickRune.RemoveListener(SelectRune);
        selectedRune.onClickRune.AddListener(DeselectRune);
    }

    public void DeselectRune(RuneDisplay selectedRune)
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
