using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Reward : MonoBehaviour {
    public int Aamount;
    RewardType m_rewardType;

    public RewardType RewardType
    {
        get
        {
            return m_rewardType;
        }

        set
        {
            m_rewardType = value;
        }
    }
   
}
public enum RewardType
{
    Com,
    Bullet,
    Firsod
}
