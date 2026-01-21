using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[Serializable]
public class InfoEntry
{
    public int idPhase;
    public string text;
}

[Serializable]
public class InfoDatabase
{
    public InfoEntry[] infos;
}

public class DisplayerIntroText : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private TextAsset jsonFile;
    [SerializeField] private int phaseToPlay = 0;

    [Header("UI")]
    [SerializeField] private TMP_Text _tmpText;
    [SerializeField] private Button _btnStartPhase;
    [SerializeField] private Button _btnRestartGame;
    [SerializeField] private Button _btnGoToCity;

    
    private InfoDatabase _database;
    private int _currentIndex = 0;
    private bool _sequenceFinished = false;

    private void Awake()
    {
        LoadJson();
        ResetSequenceToPhase();
        DisplayCurrentEntry();
    }

    private void LoadJson()
    {
        if (jsonFile == null)
        {
            Debug.LogError("JsonInfoSequence : Aucun TextAsset JSON assigné.");
            return;
        }

        try
        {
            _database = JsonUtility.FromJson<InfoDatabase>(jsonFile.text);

            if (_database == null || _database.infos == null || _database.infos.Length == 0)
            {
                Debug.LogWarning("JsonInfoSequence : JSON chargé mais vide ou mal formé.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("JsonInfoSequence : Erreur de parsing JSON -> " + e.Message);
        }
    }

    /// <summary>
    /// Place le curseur (_currentIndex) sur la première entrée correspondant à phaseToPlay.
    /// Si rien n'est trouvé, la séquence est marquée comme terminée.
    /// </summary>
    private void ResetSequenceToPhase()
    {
        _sequenceFinished = false;
        _currentIndex = 0;

        if (_database == null || _database.infos == null || _database.infos.Length == 0)
        {
            _sequenceFinished = true;
            return;
        }

        while (_currentIndex < _database.infos.Length &&
               _database.infos[_currentIndex].idPhase != phaseToPlay)
        {
            _currentIndex++;
        }

        if (_currentIndex >= _database.infos.Length)
        {
            Debug.LogWarning($"JsonInfoSequence : aucune entrée trouvée pour idPhase = {phaseToPlay}.");
            _sequenceFinished = true;
        }
        
        _btnStartPhase.gameObject.SetActive(false);
        _btnRestartGame.gameObject.SetActive(false);
        _btnGoToCity.gameObject.SetActive(false);
        DisplayCurrentEntry();
    }


    public void ShowNext()
    {
        if (_sequenceFinished)
        {
            OnSequenceFinished();
            return;
        }

        if (_database == null || _database.infos == null || _database.infos.Length == 0)
        {
            Debug.LogWarning("JsonInfoSequence : base de données vide.");
            _sequenceFinished = true;
            OnSequenceFinished();
            return;
        }

        int len = _database.infos.Length;

        int i = _currentIndex + 1;

        while (i < len && _database.infos[i].idPhase != phaseToPlay)
        {
            i++;
        }

        if (i >= len)
        {
            _sequenceFinished = true;
            OnSequenceFinished();
            return;
        }

        _currentIndex = i;
        DisplayCurrentEntry();

        if (!HasNextEntryInPhaseFromIndex(_currentIndex))
        {
            _sequenceFinished = true;
            OnSequenceFinished();
        }
    }

    public void ShowPrevious()
    {
        if (_database == null || _database.infos == null || _database.infos.Length == 0)
            return;

        _sequenceFinished = false;

        int i = _currentIndex - 1;

        if (i < 0)
        {
            ResetSequenceToPhase();
            DisplayCurrentEntry();
            return;
        }

        while (i >= 0 && _database.infos[i].idPhase != phaseToPlay)
        {
            i--;
        }

        if (i < 0)
        {
            ResetSequenceToPhase();
            DisplayCurrentEntry();
            return;
        }

        _currentIndex = i;
        DisplayCurrentEntry();
    }

    
    private void DisplayCurrentEntry()
    {
        var entry = _database.infos[_currentIndex];
        _tmpText.text = entry.text;
    }

    private bool HasNextEntryInPhaseFromIndex(int index)
    {
        if (_database == null || _database.infos == null)
            return false;

        int len = _database.infos.Length;
        for (int i = index + 1; i < len; i++)
        {
            if (_database.infos[i].idPhase == phaseToPlay)
                return true;
        }

        return false;
    }


    private void OnSequenceFinished()
    {
        if (phaseToPlay == 0)
        {
            _btnStartPhase.gameObject.SetActive(true);
        }
        if (phaseToPlay == 1)
        {
            _btnGoToCity.gameObject.SetActive(true);
        }
        else if (phaseToPlay >= 100)
        {
            _btnRestartGame.gameObject.SetActive(true);
        }
    }

    public void SetPhaseToPlay(int newPhase)
    {
        phaseToPlay = newPhase;
        ResetSequenceToPhase();
    }
}
