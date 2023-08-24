using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsView : MonoBehaviour
{
    [SerializeField] private GameObject rewardsContainer;
    [SerializeField] private GameObject rewardView;

    public void Continue()
    {
        if (rewardView != null)
        {
            Instantiate(rewardView, rewardsContainer.transform);
        }
    }
}
