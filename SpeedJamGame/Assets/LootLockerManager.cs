using System;
using System.Collections;
using LootLocker.Requests;
using TMPro;
using UnityEngine;

public class LootLockerManager : Singleton<LootLockerManager>
{
    public const int LeaderboardID = 20886;
    public TMP_InputField nameInput;
    
    private void Start()
    {
        StartCoroutine(LoginRoutine());
    }

    private IEnumerator LoginRoutine()
    {
        var done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player was logged in successfully");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                LootLockerSDKManager.SetPlayerName(nameInput.text, nameResponse => {});
                done = true;
            }
            else
            {
                Debug.Log("Could not start session");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public void SubmitScore(int score)
    {
        StartCoroutine(SubmitScoreRoutine(score));
    }
    
    private IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        var done = false;
        var playerID = PlayerPrefs.GetString("PlayerID");

        LootLockerSDKManager.GetMemberRank(LeaderboardID.ToString(), playerID, (response) =>
        {
            if (response.statusCode == 200) 
            {
                Debug.Log("Successful");
                if (scoreToUpload > response.score || response.score == 0)
                {
                    print($"Score: {scoreToUpload}, PlayerID: {playerID}");
                    LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, LeaderboardID.ToString(), (scoreResponse) =>
                    {
                        if (scoreResponse.success)
                        {
                            print($"Score: {scoreResponse.score}, PlayerID: {scoreResponse.member_id}");
                            Debug.Log("Successfully uploaded score");
                            done = true;
                        }
                        else
                        {
                            Debug.Log("Failed" + scoreResponse.errorData);
                            done = true;
                        }
                    });
                }
            } 
            else 
            {
                Debug.Log("failed: " + response.errorData);
            }
        });
        yield return new WaitWhile(() => done = false);
    }
    
    public IEnumerator FetchHighScoresRoutine()
    {
        var done = false;
        var count = 50;
        LootLockerSDKManager.GetScoreList(16693.ToString(), count, 0, (response) =>
        {
            if (response.success)
            {
                var members = response.items;

                foreach (var member in members)
                {
                    var playerName = member.player.name != "" ? member.player.name : member.player.id.ToString();
                    // Instantiate Some Prefab And Set The Name
                }
                done = true;
            }
            else
            {
                Debug.Log("Failed" + response.errorData);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}
