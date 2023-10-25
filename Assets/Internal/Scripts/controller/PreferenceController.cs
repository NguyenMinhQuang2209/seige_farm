using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PreferenceController : MonoBehaviour
{
    private GameObject player;
    private GameObject character;
    public static PreferenceController instance;
    public GameObject Player { get { return player; } }
    public GameObject Character { get { return character; } }

    public GameObject box;
    public GameObject inventory;
    public GameObject rabish;
    public GameObject shop;
    public TextMeshPro viewText;
    public GameObject inventoryContainer;
    public GameObject enemyTarget;
    public GameObject bullet;
    public GameObject switchButton;
    public GameObject crafting;
    public GameObject present;
    public GameObject solliderUI;
    public GameObject setting;
    public GameObject howtoplay;
    public int inventorySlots = 1;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        inventorySlots += 1;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagController.playerTag);
        character = GameObject.FindGameObjectWithTag(TagController.characterTag);
    }
    public void AddText(Vector3 pos, string textContent, Color color)
    {
        TextMeshPro textPreview = Instantiate(viewText, pos, viewText.rectTransform.rotation);
        textPreview.text = textContent;
        textPreview.color = color;
    }
}
