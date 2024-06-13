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

    void Start()
    {
        // Ensure memberTemplate is disabled initially
        memberTemplate.SetActive(false);
    }

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
        GameObject ownerText = Instantiate(memberTemplate, membersPanel.transform);
        ownerText.GetComponent<TextMeshProUGUI>().text = ownerName + " (Host)";
        ownerText.SetActive(true);
        memberTexts.Add(ownerText);

        // Get the number of members in the lobby
        int memberCount = SteamMatchmaking.GetNumLobbyMembers(lobbyID);

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

            GameObject newMemberText = Instantiate(memberTemplate, membersPanel.transform);
            newMemberText.GetComponent<TextMeshProUGUI>().text = memberName;
            newMemberText.SetActive(true);
            memberTexts.Add(newMemberText);
        }
    }
}
