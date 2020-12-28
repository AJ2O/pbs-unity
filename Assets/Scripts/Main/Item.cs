using PBS.Data;
using PBS.Databases;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    // General
    public string itemID { get; set; }
    public PBS.Data.Item data { 
        get 
        {
            return Items.instance.GetItemData(itemID);
        } 
    }

    public bool useable { get; set; }

    // Constructor
    public Item(
        string itemID,
        bool useable = true)
    {
        this.itemID = itemID;
        this.useable = useable;
    }

    public Item Clone()
    {
        Item cloneItem = new Item(
            itemID: itemID,
            useable: useable);
        return cloneItem;
    }

}
