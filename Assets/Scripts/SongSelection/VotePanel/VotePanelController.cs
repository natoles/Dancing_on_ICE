using System.Collections.Generic;
using TwitchLib.Client.Events;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class VotePanelController : MonoBehaviour
{
    private VoteEntry[] choices = null;

    [SerializeField]
    private SongScrollView songScrollView = null;

    [SerializeField]
    private GameObject voteEntriesParent = null;

    [SerializeField]
    private VotePanelDifficultyCursorController DifficultyCursor = null;

    private int[] songVotes = null;
    private int difficultyCumulatedVotes = 0;
    private int votesCount = 0;

    private bool voteIsRunning = false;
    private float timeVoteStarted = float.MaxValue;

    [SerializeField]
    private float defaultVoteTime = 30f;

    private void Start()
    {
        choices = voteEntriesParent.GetComponentsInChildren<VoteEntry>();
        for (int i = 0; i < choices.Length; ++i)
        {
            choices[i].Id = ((char)('A' + i)).ToString();
        }

        songVotes = new int[choices.Length];
        ChooseRandomSongs();
    }

    public void ChooseRandomSongs()
    {
        List<int> picks = new List<int>();
        for (int i = 0; i < choices.Length; ++i)
        {
            int pick = 0;
            do
            {
                pick = Random.Range(0, songScrollView.entries.Count);
            } while (picks.Contains(pick));
            picks.Add(pick);
            choices[i].Song = songScrollView.entries[pick];
        }
    }

    public void StartVote()
    {
        DifficultyCursor.gameObject.SetActive(true);
        TwitchClient.Instance.SendMessage("Vote for the next song NOW ! Type a letter followed by a number in chat to submit your vote ! Example: type \"A4\" to vote for song A and difficulty 4");
        foreach (VoteEntry choice in choices)
        {
            TwitchClient.Instance.SendMessage($"{choice.Id} - {choice.Song.SongName}");
        }
        TwitchClient.Instance.OnMessageReceived += VoteHandler;
        timeVoteStarted = Time.time;
        voteIsRunning = true;
        NotificationManager.Instance.PushNotification("Vote has started", Color.white, Color.blue);
    }

    private void VoteHandler(object sender, OnMessageReceivedArgs e)
    {
        if (voteIsRunning)
        {
            string msg = e.ChatMessage.Message.Trim().ToUpperInvariant();
            if (msg.Length == 2)
            {
                int votedID = 0;
                int votedDifficulty = 0;
                if (!(char.IsLetter(msg[0]) && msg[0] - 'A' < choices.Length) || !char.IsNumber(msg[1]))
                {
                    return;
                }
                votedID = msg[0] - 'A';
                votedDifficulty = msg[1] - '0';

                songVotes[votedID]++;
                difficultyCumulatedVotes += votedDifficulty;
                votesCount++;
            }
        }
    }

    private void Update()
    {
        if (voteIsRunning && Time.time > timeVoteStarted + defaultVoteTime)
        {
            voteIsRunning = false;
            TwitchClient.Instance.OnMessageReceived -= VoteHandler;

            int winnerID = 0;
            for (int i = 1; i < songVotes.Length; ++i)
            {
                if (songVotes[i] > songVotes[winnerID])
                    winnerID = i;
            }
            float winnerDifficulty = (float)difficultyCumulatedVotes / votesCount;

            TwitchClient.Instance.SendMessage("Vote has ended, thanks for your participation !");
            TwitchClient.Instance.SendMessage($"Song {choices[winnerID].Id} with difficulty {System.Math.Round(winnerDifficulty, 1)} have been choosen !");
            NotificationManager.Instance.PushNotification("Vote has ended", Color.white, Color.blue);

            RythmGameSettings.BeatmapToLoad = choices[winnerID].Song.BeatmapContainer;
            RythmGameSettings.Difficulty = winnerDifficulty;
            SceneHistory.LoadScene("TwitchMode");
        }

        if (voteIsRunning)
        {
            for (int i = 0; i < choices.Length; ++i)
            {
                if (Input.GetKeyDown(KeyCode.A + i))
                {
                    songVotes[i]++;
                    difficultyCumulatedVotes += Random.Range(1, 5);
                    votesCount++;
                    Debug.Log(difficultyCumulatedVotes / (float)votesCount);
                }
            }

            if (votesCount != 0)
            {
                for (int i = 0; i < choices.Length; ++i)
                {
                    choices[i].Percentage = (float)songVotes[i] / votesCount;
                }
                DifficultyCursor.Difficulty = (float)difficultyCumulatedVotes / votesCount;
            }
        }
    }
}
