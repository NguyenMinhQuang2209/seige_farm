using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtCoin;
    public static CoinController instance;
    [SerializeField] private float currentCoin = 0;
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
        txtCoin.text = currentCoin.ToString();
    }
    private void Update()
    {
        currentCoin = Mathf.Round(currentCoin * Mathf.Pow(10, 1)) / Mathf.Pow(10, 1);
    }
    public void UpdateCoin(float coin)
    {
        currentCoin = Mathf.Max(0, currentCoin + coin);
        txtCoin.text = currentCoin.ToString();
    }
    public bool CheckMinusCoin(float coin, bool currentAdding = false)
    {
        if (currentCoin >= coin)
        {
            if (currentAdding)
            {
                currentCoin -= coin;
                txtCoin.text = currentCoin.ToString();
            }
            return true;
        }
        return false;
    }
    public void AddCoin(float coin)
    {
        currentCoin += coin;
        txtCoin.text = currentCoin.ToString();
    }
    public void ChangeCurrentCoin(float coin)
    {
        currentCoin = coin;
        txtCoin.text = currentCoin.ToString();
    }
    public float GetCurrentCoint()
    {
        return currentCoin;
    }
}
