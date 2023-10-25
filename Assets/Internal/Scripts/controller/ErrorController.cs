using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorController : MonoBehaviour
{
    public static ErrorController instance;
    [SerializeField] private TextMeshProUGUI errorTxt;
    [SerializeField] private GameObject txtContainer;
    [SerializeField] private float timeDelay = 5f;
    float currentTimeDelay = 0f;
    bool containerEnable = false;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        errorTxt.text = string.Empty;
        txtContainer.SetActive(containerEnable);
    }
    private void Update()
    {
        currentTimeDelay = Mathf.Min(currentTimeDelay + Time.deltaTime, timeDelay);
        txtContainer.SetActive(containerEnable);
        if (currentTimeDelay == timeDelay)
        {
            containerEnable = false;
        }

    }
    public void ChangeTxt(string newTxt, Color color)
    {
        containerEnable = true;
        currentTimeDelay = 0f;
        errorTxt.text = newTxt;
        errorTxt.color = color;
    }
}
