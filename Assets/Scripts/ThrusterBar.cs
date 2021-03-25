using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrusterBar : MonoBehaviour
{
    public Slider thrusterBar;

    [SerializeField]
    private int _maxThruster = 1;
    [SerializeField]
    private int _currentThruster;

    private void Start()
    {
        _currentThruster = _maxThruster;
        thrusterBar.maxValue = _maxThruster;
        thrusterBar.value = _currentThruster;
    }

    public void UseThruster(int used)
    {
        if (_currentThruster - used >= 0)
        {
            _currentThruster -= used;
            thrusterBar.value = _currentThruster;

            StartCoroutine(Reset());
        }
        else
        {
            Debug.Log("No Thruster");
        }
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(10f);

        _currentThruster = _maxThruster;
        thrusterBar.value = _maxThruster;
    }
}
