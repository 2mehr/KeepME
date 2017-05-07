using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CharBase : MonoBehaviour, IShootable, Ishooter
{
    float m_HpSum;
    public float HpSum
    {
        get
        {
          return  m_HpSum;
        }
        set
        {
            m_HpSum = value;
        }
    }
    int m_CurrentWeaponID;
    
    public int CurrentWeaponID
    {
        get
        {
            return m_CurrentWeaponID;
        }

        set
        {
            m_CurrentWeaponID = value;
        }
    }
    [SerializeField]
    List<Weapon> m_Weapons;
    public List<Weapon> Weapons { get { return m_Weapons; }set { m_Weapons = value; } }
    [SerializeField]
    CharcterType m_CType;
    public CharcterType CType
    {
        get
        {
            return m_CType;
        }


        set
        {
            m_CType = value;
        }
    }
    [SerializeField]
    float m_HP;

    public float HP
    {
        get
        {
            return m_HP;

        }
        set
        {
            m_HP = value;
        }
    }

    float m_Armor;
    public float Armor { get { return m_Armor; } set { m_Armor = value; } }

    List<Reward> m_Rewards = new List<Reward>();
    public List<Reward> Rewards { get { return m_Rewards; } set { m_Rewards = value; } }
    
    
    bool m_IsMelee;
    public bool IsMelee
    {
        get { return m_IsMelee; }
        set { m_IsMelee = value; }
    }
    [SerializeField]
    Transform m_Target;
    public Transform Target
    {
        get
        {
            return m_Target;
        }

        set
        {
            m_Target = value;
        }
    }

    float m_Speed;

    public abstract event OnDeathDelegate OnDeath;

    public float Speed
    {
        get
        {
            return m_Speed;
        }

        set
        {
            m_Speed = value;
        }
    }

 [SerializeField]
    bool m_IsDaed;
    public bool IsDaed
    {
        get
        {
            return m_IsDaed;
        }

        set
        {
            m_IsDaed = value;
        }
    }
    bool m_IsAttack;
    public bool IsAttack
    {
        get { return m_IsAttack; }
        set { m_IsAttack = value; }
    }
    [SerializeField]
    Transform m_ShooterPoint;
    public Transform ShooterPoint
    {
        get
        {
            return m_ShooterPoint;
        }

        set
        {
            m_ShooterPoint = value;
        }
    }

    public abstract void Shoot(int currentWepID);

  

    public abstract void Die( CharBase killer);
}