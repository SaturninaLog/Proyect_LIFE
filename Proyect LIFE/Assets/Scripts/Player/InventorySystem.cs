using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> items = new Dictionary<string, int>();

    public void AddItem(string itemName, int amount)
    {
        if (!items.ContainsKey(itemName))
            items[itemName] = 0;

        items[itemName] += amount;
        Debug.Log($"🧺 Añadido {amount}x {itemName}. Total: {items[itemName]}");
    }

    public int GetItemCount(string itemName)
    {
        return items.ContainsKey(itemName) ? items[itemName] : 0;
    }

    public void PrintInventory()
    {
        Debug.Log("📦 Inventario actual:");
        foreach (var kvp in items)
            Debug.Log($" - {kvp.Key}: {kvp.Value}");
    }
}
