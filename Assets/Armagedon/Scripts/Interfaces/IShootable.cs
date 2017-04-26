using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnDeathDelegate(CharBase e,CharBase killer);

public interface IShootable
{

    bool IsDaed { get; set; }
    float HP { get; set; }

    float Armor { get; set; }

    List<Reward> Rewards { get; set; }

    event OnDeathDelegate OnDeath;
      void Die( CharBase killer);

}

