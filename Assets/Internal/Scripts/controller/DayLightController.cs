using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayLightController : MonoBehaviour
{

    public static DayLightController instance;

    [SerializeField] private float startHour = 7f;
    [SerializeField] private float sunRiseHour = 5f;
    [SerializeField] private float sunSetHour = 17f;
    [SerializeField] private float timeSpeed = 1f;
    [SerializeField] private Light sunLight;
    [SerializeField] private Material dayMaterial;
    [SerializeField] private Material nightMaterial;
    [SerializeField] private Material sunRiseMaterial;
    [SerializeField] private Material sunSetMaterial;
    [SerializeField] private TextMeshProUGUI timeText;

    TimeSpan timeSunRise;
    TimeSpan timeSunSet;
    DateTime startTime;
    DateTime currentTime;
    Material currentMaterials;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
        timeSunRise = TimeSpan.FromHours(sunRiseHour);
        timeSunSet = TimeSpan.FromHours(sunSetHour);
        startTime = DateTime.Now.Date;
    }
    private void Update()
    {
        currentTime = currentTime.AddSeconds(timeSpeed * Time.deltaTime);
        if (timeText != null)
        {
            timeText.text = "Day:" + ((currentTime - startTime).Days + 1) + " - " + currentTime.ToString("HH:mm");
        }
        LightRotation();
    }
    private void LightRotation()
    {
        float currentRotation;
        if (currentTime.TimeOfDay > timeSunRise && currentTime.TimeOfDay < timeSunSet)
        {
            if (timeSunSet - currentTime.TimeOfDay <= TimeSpan.FromHours(1))
            {
                if (currentMaterials != sunSetMaterial)
                {
                    RenderSettings.skybox = sunSetMaterial;
                }
                currentMaterials = sunSetMaterial;
            }
            else
            {
                if (currentMaterials != dayMaterial)
                {
                    RenderSettings.skybox = dayMaterial;
                }
                currentMaterials = dayMaterial;
            }
            TimeSpan timeRunRiseToSunSetDuraction = timeSunSet - timeSunRise;
            TimeSpan timeWasPassed = currentTime.TimeOfDay - timeSunRise;
            currentRotation = Mathf.Lerp(0f, 180f, (float)(timeWasPassed / timeRunRiseToSunSetDuraction));
        }
        else
        {
            if (currentMaterials != nightMaterial)
            {
                RenderSettings.skybox = nightMaterial;
            }
            currentMaterials = nightMaterial;
            TimeSpan timeRunSetToSunRiseDuraction = TimeSpan.FromHours(24) - (timeSunSet - timeSunRise);
            TimeSpan currentTimeout = currentTime.TimeOfDay < timeSunRise ? currentTime.TimeOfDay + TimeSpan.FromHours(24) : currentTime.TimeOfDay;
            TimeSpan timeWasPassed = currentTimeout - timeSunSet;
            currentRotation = Mathf.Lerp(180f, 360f, (float)(timeWasPassed / timeRunSetToSunRiseDuraction));
        }
        sunLight.transform.rotation = Quaternion.Euler(currentRotation, 0f, 0f);
    }
    public DateTime GetCurrentDateTime()
    {
        return currentTime;
    }
    public DateTime GetStartTime()
    {
        return startTime;
    }
}
