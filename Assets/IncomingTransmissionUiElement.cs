using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncomingTransmissionUiElement : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 onScreenPosition;
    [SerializeField] private Text countdownTimer;
    private Color countdownTimerTextColor;
    [SerializeField] private Image alienImage;
    private RectTransform rectTransform;
    [SerializeField] private float minAlienShipSize;

    private void Awake()
    {
        this.rectTransform = (RectTransform)transform;
        this.countdownTimerTextColor = this.countdownTimer.color;
        rectTransform.anchoredPosition = this.startPosition;
    }

    public void OnAlienVoice()
    {
        StartCoroutine(nameof(this.HandleAnimations));
    }

    private IEnumerator HandleAnimations()
    {
        countdownTimer.color.LerpTo(Color.clear, 0.6f, val => this.countdownTimer.color = val, null, GlobalLerpProcessor.easeInOutCurve);
        new Vector3(this.rectTransform.anchoredPosition.x, this.rectTransform.anchoredPosition.y, 0).LerpTo(this.onScreenPosition, 0.8f, val => this.rectTransform.anchoredPosition = val, null, GlobalLerpProcessor.easeInOutCurve);

        yield return new WaitForSeconds(3f);
        new Vector3(this.rectTransform.anchoredPosition.x, this.rectTransform.anchoredPosition.y, 0).LerpTo(this.startPosition, 0.8f, val => this.rectTransform.anchoredPosition = val, null, GlobalLerpProcessor.easeInOutCurve);
        countdownTimer.color.LerpTo(this.countdownTimerTextColor, 0.6f, val => this.countdownTimer.color = val, null, GlobalLerpProcessor.easeInOutCurve);
    }
}
