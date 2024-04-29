using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroButtonController : MonoBehaviour
{
    [SerializeField, SerializedDictionary("Category", "Buttons")]
    private SerializedDictionary<string, List<Button>> _introButtons = new SerializedDictionary<string, List<Button>>();

    public void OnButtonClick(string category)
    {
        foreach (var buttons in _introButtons)
        {
            foreach (var button in buttons.Value)
            {
                button.gameObject.SetActive(buttons.Key == category);
            }
        }
    }
}
