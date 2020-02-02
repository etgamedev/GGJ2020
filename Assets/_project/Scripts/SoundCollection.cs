using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioDataset
{
    public string soundName;
    public AudioClip audioClip;
}

[CreateAssetMenu]
public class SoundCollection : ScriptableObject
{
    List<AudioDataset> audioDatasets = new List<AudioDataset>();

    public AudioClip GetClip(string soundName)
    {
        for (int i = 0; i < audioDatasets.Count; ++i)
        {
            if (audioDatasets[i].soundName == soundName)
            {
                return audioDatasets[i].audioClip;
            }
        }

        return null;
    }
}
