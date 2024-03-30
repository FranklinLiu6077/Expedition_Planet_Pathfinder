using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stamina : MonoBehaviour
{
    [SerializeField]
    private float _percentage = 100;

    public float percentage
    {
        get => _percentage;
        set
        {
            if (_percentage != value)
            {
                DOTween.To(() => HP.anchoredPosition, x => HP.anchoredPosition = x, new Vector2(-620 + value * 6.2f, 0), 0.5f);
                _percentage = value;
            }
        }
    }

    private RectTransform HP;

    void Start()
    {
        HP = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
    }

    public void OnStaminaChanged(int currentStamina, int maxStamina)
    {
        // 更新百分比值，假设体力值是从0到maxStamina
        percentage = (float)currentStamina / maxStamina * 100;
    }

    public void Sminus(float x) {
        percentage -= x;
    }
    public void Sadd(float x)
    {
        percentage += x;
    }
    public void Sset(float x)
    {
        percentage = x;
    }

}
