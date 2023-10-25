using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RecoverHealth : MonoBehaviour
{
    private ItemOffset itemOffset;
    private Solider sollider;
    public bool showInWorld = false;

    private void Start()
    {
        itemOffset = GetComponent<ItemOffset>();
        sollider = GetComponent<Solider>();
    }
    public void RecoverHealthItem()
    {
        if (itemOffset != null)
        {
            itemOffset.RecoverHealthItem();
            return;
        }
        if (sollider != null)
        {
            sollider.RecoverHealthItem();
            return;
        }
    }
    public string GetRecoverHealth()
    {
        StringBuilder sb = new();
        sb.Append("H to health");
        sb.Append(" (");
        if (itemOffset != null)
        {
            sb.Append(itemOffset.GetRecoverHealth());
            sb.Append("coins)");
            return sb.ToString();
        }
        if (sollider != null)
        {
            sb.Append(sollider.GetRecoverHealth());
            sb.Append("coins)");
            return sb.ToString();
        }
        return "";
    }
}
