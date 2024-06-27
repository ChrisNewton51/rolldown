using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;

public class LobbyUIManager : MonoBehaviour
{
    public GameObject memberTemplate;
    public GameObject membersPanel;

    private List<GameObject> memberTexts = new List<GameObject>();

    public void UpdateLobbyMembers(CSteamID lobbyID)
    {
        // Clear existing member UI elements
        foreach (var memberText in memberTexts)
        {
            Destroy(memberText);
        }
        memberTexts.Clear();

        // Include the lobby owner (creator)
        CSteamID ownerID = SteamMatchmaking.GetLobbyOwner(lobbyID);
        string ownerName = SteamFriends.GetFriendPersonaName(ownerID);

        // Create a UI element for the lobby owner
        CreatePlayerNameText(ownerName + " (Host)");

        // Get the number of members in the lobby
        int memberCount = SteamMatchmaking.GetNumLobbyMembers(lobbyID);

        // Debug log to check the number of members
        Debug.Log("Member Count: " + memberCount);

        // Loop through each member and create a UI element for them
        for (int i = 0; i < memberCount; i++)
        {
            CSteamID memberID = SteamMatchmaking.GetLobbyMemberByIndex(lobbyID, i);
            string memberName = SteamFriends.GetFriendPersonaName(memberID);

            // Skip adding the owner again
            if (memberID == ownerID)
            {
                continue;
            }

            CreatePlayerNameText(memberName);
        }
    }

    private void CreatePlayerNameText(string name)
    {
        GameObject newPlayerNameText = Instantiate(memberTemplate, membersPanel.transform);
        newPlayerNameText.GetComponent<TextMeshProUGUI>().text = name;
        newPlayerNameText.SetActive(true);
        memberTexts.Add(newPlayerNameText);
    }
}
