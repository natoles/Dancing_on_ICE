﻿using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongScrollView : ScrollRect
{
    [SerializeField]
    protected SongEntry SongEntryGameObject = null;
    
    [System.NonSerialized]
    public List<SongEntry> entries = new List<SongEntry>();

    private int currentSelection = 0;

    protected override void Start()
    {
        if (Application.isPlaying)
        {
            UpdateSongsList();

            // Center list on a random song
            SelectRandomSong();
        }
    }

    public void UpdateSongsList()
    {
        entries.Clear();

        if (content.childCount > 0)
        {
            GameObject[] children = new GameObject[content.childCount];
            int i = 0;
            foreach (Transform childTransform in content)
            {
                children[i] = childTransform.gameObject;
                i++;
            }
            foreach (GameObject child in children)
            {
                Destroy(child);
            }
        }

        string[] songFolders = Directory.GetDirectories(Application.streamingAssetsPath + "/Songs");
        foreach (string songFolder in songFolders)
        {
            string[] bmFiles = Directory.GetFiles(songFolder, "*.icebm");
            foreach (string bmFile in bmFiles)
            {
                SongEntry entry = Instantiate(SongEntryGameObject, content);
                entry.gameObject.name = entries.Count + " - " + Path.GetFileNameWithoutExtension(bmFile);
                entry.ScrollView = this;
                entry.SetSong(entries.Count, bmFile);
                entries.Add(entry);
            }
        }

        if (entries.Count > 0)
        {
            // Add some padding on top and bottom of the list
            VerticalLayoutGroup vlg = content.GetComponent<VerticalLayoutGroup>();
            RectTransform viewportRect = viewport.GetComponent<RectTransform>();
            RectTransform entryRect = SongEntryGameObject.GetComponent<RectTransform>();

            int padding = (int)((viewportRect.rect.height / 2) - (entryRect.rect.height / 2));
            if (padding > 0)
            {
                vlg.padding.top = padding;
                vlg.padding.bottom = padding;
            }

        }
    }

    public void ScrollToSong(int songID)
    {
        if (entries.Count > 0)
        {
            verticalNormalizedPosition = 1 - ((float)songID) / (entries.Count - 1);
        }
    }

    public void SelectSong(int songID)
    {
        if (entries.Count > 0)
        {
            entries[songID].Select();
            currentSelection = songID;
        }
    }

    public void SelectRandomSong()
    {
        int id = currentSelection;
        if (entries.Count > 1)
        {
            while (id == currentSelection)
                id = Random.Range(0, entries.Count);
        }

        SelectSong(id);
    }
}