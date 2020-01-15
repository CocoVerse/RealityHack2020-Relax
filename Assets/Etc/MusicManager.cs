using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] List<MusicGroup> musicGroups;

    private int selectedIndex;
    private ReactiveProperty<MusicGroup> _selectedMusicGroup = new ReactiveProperty<MusicGroup>();
    public IObservable<MusicGroup> SelectedMusicGroup => _selectedMusicGroup.Where(g => g != null);


    // Start is called before the first frame update
    void Start() {
        SetSelectedMusicGroup(UnityEngine.Random.Range(0, musicGroups.Count));
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) NextSong();
    }

    private void SetSelectedMusicGroup(int i) {
        selectedIndex = i;
        _selectedMusicGroup.Value = musicGroups[selectedIndex];
    }

    public void NextSong() {
        selectedIndex++;
        selectedIndex %= musicGroups.Count;
        _selectedMusicGroup.Value = musicGroups[selectedIndex];
    }
}
