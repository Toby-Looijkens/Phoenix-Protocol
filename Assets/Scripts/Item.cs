using System;
using UnityEngine;

public interface IItem
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }

    public void Use();
    public void Examine();
    public void Discard();
}
