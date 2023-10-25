using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public static BulletController instance;
    [SerializeField] private TextMeshProUGUI bulletShow;
    [SerializeField] private GameObject bulletUIShow;
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
        bulletUIShow.SetActive(false);
    }
    public void ChangeBulletTxt(string newTxt)
    {
        bulletUIShow.SetActive(!newTxt.Equals(string.Empty));
        bulletShow.text = newTxt;
    }
}
