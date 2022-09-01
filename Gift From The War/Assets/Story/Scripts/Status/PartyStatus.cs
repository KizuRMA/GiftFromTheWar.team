using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "PartyStatus", menuName = "CreatePartyStatus")]
public class PartyStatus : ScriptableObject
{
    [SerializeField]
    private List<ButtonStatus> partyMembers = null;


    public void SetButtonStatus(ButtonStatus allyStatus)
    {
        if (!partyMembers.Contains(allyStatus))
        {
            partyMembers.Add(allyStatus);
        }
    }

    public List<ButtonStatus> GetButtonStatus()
    {
        return partyMembers;
    }

}
