using System;
using UnityEngine;

public interface IItem
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ItemType Type { get; set; }

    public void Use();
    public void Combine();
    public void Examine();
    public void Discard();
}

public enum ItemType
{
    Nail_Driver_Ammo,
    Pile_Driver_Ammo,
}
