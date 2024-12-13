using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GetReadyPlane : MonoBehaviour
{
    private const string AddText = "...";
    
    [SerializeField] private TMP_Text _countDownText;

    private int _startValue = 3;
    private int _interval = 1;

    public event Action CountdownComplete;

    private void OnEnable()
    {
        _startValue = 3;
        StartCoroutine(StartCountDown());
    }

    private void OnDisable()
    {
        StopCoroutine(StartCountDown());
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
    
    private IEnumerator StartCountDown()
    {
        WaitForSeconds interval = new WaitForSeconds(_interval);
        
        while (_startValue > 0)
        {
            _countDownText.text = _startValue + AddText;
            _startValue--;
            yield return interval;
        }
        
        CountdownComplete?.Invoke();
    }
}
