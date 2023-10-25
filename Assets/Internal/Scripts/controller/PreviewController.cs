using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewController : MonoBehaviour
{
    [SerializeField] private PreviewDetail preview;
    public static PreviewController instance;
    [SerializeField] private float offsetX = 0.5f;
    [SerializeField] private float offsetY = 0.5f;
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
        preview.gameObject.SetActive(false);
    }
    public void ChangePreviewTxt(string txt, Vector3 pos)
    {

        preview.transform.position = pos + new Vector3(offsetX, offsetY, 0f);
        if (txt.Equals(string.Empty))
        {
            preview.gameObject.SetActive(false);
        }
        else
        {
            preview.ChangeTextView(txt);
            preview.gameObject.SetActive(true);
        }
    }
}
