using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VotePanel : MonoBehaviour
{
    [SerializeField]
    private VoteEntry[] choices = null;

    [SerializeField]
    private SongScrollView songs = null;

    private int[] songVotes = null;
    private int songVotesCount = 0;

    private int difficultyCumulatedVotes = 0;
    private int difficultyVotesCount = 0;

    private void Start()
    {
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
                pick = Random.Range(0, songs.entries.Count);
            } while (picks.Contains(pick));
            picks.Add(pick);
            choices[i].SongName = System.IO.Path.GetFileNameWithoutExtension(songs.entries[pick].BeatmapContainer.sourceFile);
        }
    }

    private void Update()
    {
        for (int i = 0; i < choices.Length; ++i)
        {
            choices[i].Percentage = songVotesCount == 0 ? 0 : songVotes[i] / songVotesCount;
        }
    }
}
