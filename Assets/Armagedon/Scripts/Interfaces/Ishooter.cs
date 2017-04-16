using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Ishooter  {
    int CurrentWeaponID { get; set; }
  
    List<Weapon> Weapons { get; set; }
    [SerializeField]
    CharcterType CType { get; set; }

   
    Transform Target { get; set; }

    bool IsMelee { get; set; }
    
    void Shoot(int currentwepID);
    

    }
public enum CharcterType
{
    Player,
    Enamey,
    Frends

};
