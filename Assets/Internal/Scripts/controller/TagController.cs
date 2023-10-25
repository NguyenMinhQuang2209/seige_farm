using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagController : MonoBehaviour
{
    public static TagController instance;
    public static string playerTag = "Player";
    public static string characterTag = "Character";
    public static string bulletTag = "Bullet";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
