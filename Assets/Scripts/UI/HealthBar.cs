using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform PercentText;
    [SerializeField] private RectTransform HBar;

    private void Update()
    {
        Player Pl = FindAnyObjectByType<Player>();
        float HPPercent = Pl is not null? (float)Pl.HealthPoints / Pl.StartHealth : 0;
        HBar.localScale = new Vector2 (HPPercent, 1.0f);
        PercentText.GetComponent<TMPro.TextMeshProUGUI>().text = $"{Mathf.Round(HPPercent * 100)}%";
    }
}
