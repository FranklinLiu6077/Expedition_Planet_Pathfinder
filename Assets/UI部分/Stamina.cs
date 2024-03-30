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
                
                DOTween.To(() => HP.anchoredPosition, x => HP.anchoredPosition = x, new Vector2(-620+value*6.2f,0), 0.5f);
                _percentage = value;
            }
        }
    }
    private RectTransform HP;
    private RectTransform HPContinar;
    // Start is called before the first frame update
    void Start()
    {
        HP = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            Sminus(10);
        }
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
