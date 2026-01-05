using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fadeWhiteImageOut : MonoBehaviour
{
    [SerializeField] private float time;

    private Image image;

    private void Awake()
    {
        this.image = GetComponent<Image>();
    }

    private void Start()
    {
        this.image.color.LerpTo(new Color(1,1,1,0), this.time, val => this.image.color = val, null);
    }
}
