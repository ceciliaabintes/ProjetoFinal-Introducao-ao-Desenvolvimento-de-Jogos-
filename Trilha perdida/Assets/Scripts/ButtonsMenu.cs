using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonsMenu : MonoBehaviour
{
    [Header("Buttons Animation")]
    public float Duration = .2f;
    public float Delay = .05f;
    public Ease ease = Ease.InOutBack;

    public List<GameObject> Buttons;

    private void OnEnable()
    {
        _HideAllButtons();
        _ShowButtons();
    }
    private void _HideAllButtons()
    {
        foreach (var b in Buttons)
        {
            b.transform.localScale = Vector3.zero;
            b.SetActive(false);
        }
    }
    private void _ShowButtons()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            var b = Buttons[i];
            b.SetActive(true);
            b.transform.DOScale(1, Duration).SetDelay(i * Delay).SetEase(ease);
        }
    }
}