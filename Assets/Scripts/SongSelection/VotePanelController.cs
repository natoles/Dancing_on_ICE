using System.Collections.Generic;
using TwitchLib.Client.Events;
using UnityEngine;
using UnityEngine.UI;

public class VotePanelController : MonoBehaviour
{
    [SerializeField]
    private VoteEntry[] choices = null;

    [SerializeField]
    private SongScrollView songScrollView = null;

    [SerializeField]
    private Button rerollButton = null;

    [SerializeField]
    private Button startVoteButton = null;

    [SerializeField]
    private DifficultyCursorController DifficultyCursor = null;

    private int[] songVotes = null;
    private int difficultyCumulatedVotes = 0;
    private int votesCount = 0;

    private void Start()
    {
        if (rerollButton != null)
            rerollButton.onClick.AddListener(ChooseRandomSongs);

        startVoteButton.onClick.AddListener(StartVote);

        for (int i = 0; i < choices.Length; ++i)
        {
            choices[i].Id = ((char)('A' + i)).ToString();
        }

        songVotes = new int[choices.Length];
        ChooseRandomSongs();
    }

    private void ChooseRandomSongs()
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
            choices[i].SongName = System.IO.Path.GetFileNameWithoutExtension(songScrollView.entries[pick].BeatmapContainer.sourceFile);
        }
    }

    private void StartVote()
    {
        //TwitchClient.Instance.OnMessageReceived += VoteHandler;
        DifficultyCursor.gameObject.SetActive(true);
    }

    private void VoteHandler(object sender, OnMessageReceivedArgs e)
    {
        string msg = e.ChatMessage.Message.Trim().ToUpperInvariant();
        if (msg.Length >= 2)
        {
            int votedID = 0;
            int votedDifficulty = 0;
            if (!char.IsLetter(msg[0]) || !char.IsNumber(msg[1]))
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

    private void Update()
    {
        for (int i = 0; i < choices.Length; ++i)
        {
            if (Input.GetKeyDown(KeyCode.A + i))
            {
                songVotes[i]++;
                difficultyCumulatedVotes += Random.Range(1, 5);
                votesCount++;
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
