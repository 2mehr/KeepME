using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnDeathDelegate();

public interface IShootable
{
    float HP { get; set; }

    float Armor { get; set; }

    List<Reward> Rewards { get; set; }

    event OnDeathDelegate OnDeath; 
}

