using System.Collections;
using UnityEngine;
using TMPro;

namespace LowPolyAngelKnight
{

    public class TextCollorChanging : MonoBehaviour
    {
        [SerializeField] private Gradient colorOverTime;
        [SerializeField] private float timeMultiplier = 0.99f;

        [SerializeField] private TextMeshProUGUI textToUse;

        [SerializeField] private bool changeColor = false;
        [SerializeField] private bool goBack = false;

        private float currentTimeStep;

        private void Start()
        {
            //if (useThisText)
            //{
            //    textToUse = GetComponent<TextMeshProUGUI>();
            //}

            if (changeColor)
            {
                StartChangingColor(textToUse, colorOverTime, timeMultiplier);
            }
        }

        private IEnumerator ChangeTextColor(TextMeshProUGUI newText, Gradient newGradient, float timeSpeed)
        {
            while (true)
            {
                if (goBack)
                {
                    currentTimeStep = Mathf.PingPong(Time.time * timeSpeed, 1);
                }
                else
                {
                    currentTimeStep = Mathf.Repeat(Time.time * timeSpeed, 1);
                }

                newText.color = newGradient.Evaluate(currentTimeStep);

                yield return null;
            }
        }

        public void StartChangingColor(TextMeshProUGUI newText, Gradient newGradient, float timeSpeed = -1.0f)
        {
            if (newText != null && newGradient != null && timeSpeed > 0.0f)
            {
                StartCoroutine(ChangeTextColor(newText, newGradient, timeSpeed));
            }
            else if (newText != null && newGradient != null)
            {
                StartCoroutine(ChangeTextColor(newText, newGradient, timeMultiplier));
            }
            else if (newGradient != null && timeSpeed > 0.0f)
            {
                StartCoroutine(ChangeTextColor(textToUse, newGradient, timeSpeed));
            }
            else if (newText != null && timeSpeed > 0.0f)
            {
                StartCoroutine(ChangeTextColor(newText, colorOverTime, timeSpeed));
            }
            else if (newText != null)
            {
                StartCoroutine(ChangeTextColor(newText, colorOverTime, timeMultiplier));
            }
            else if (newGradient != null)
            {
                StartCoroutine(ChangeTextColor(textToUse, newGradient, timeMultiplier));
            }
            else if (timeSpeed > 0.0f)
            {
                StartCoroutine(ChangeTextColor(textToUse, colorOverTime, timeSpeed));
            }
        }

        public void StopChangingColor()
        {
            StopCoroutine(ChangeTextColor(textToUse, colorOverTime, timeMultiplier));
        }
    }
}
