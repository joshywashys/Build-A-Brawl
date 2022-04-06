using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnHit : MonoBehaviour
{
    public float playAboveDamage = 1;

    [Header("Sounds")]
    public List<AudioClip> onHitClips;
    public List<AudioClip> onDestroyClips;

    private GameObject audioStorage;
    private GameObject onHitStorage;
    private GameObject onDestroyStorage;
    private List<AudioSource> onHitSources;
    private List<AudioSource> onDestroySources;

    void Start()
    {
        onHitSources = new List<AudioSource>();
        onDestroySources = new List<AudioSource>();

        audioStorage = new GameObject("Audio Storages");
        onHitStorage = new GameObject("On Hit Sounds");
        onDestroyStorage = new GameObject("On Destroy Sounds");

        audioStorage.transform.parent = transform;
        onHitStorage.transform.parent = audioStorage.transform;
        onDestroyStorage.transform.parent = audioStorage.transform;

        audioStorage.transform.position = transform.position;
        onHitStorage.transform.position = audioStorage.transform.position;
        onDestroyStorage.transform.position = audioStorage.transform.position;

        foreach (AudioClip currClip in onHitClips)
        {
            AudioSource newSource = onHitStorage.AddComponent<AudioSource>();
            newSource.clip = currClip;
            onHitSources.Add(newSource);
        }

        foreach (AudioClip currClip in onDestroyClips)
        {
            AudioSource newSource = onDestroyStorage.AddComponent<AudioSource>();
            newSource.clip = currClip;
            onDestroySources.Add(newSource);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude > playAboveDamage)
        {
            int rand = Random.Range(0, onHitSources.Count);
            onHitSources[rand].Play();
        }
    }

    private void OnDestroy()
    {
        int rand = Random.Range(0, onDestroySources.Count);
        AudioSource.PlayClipAtPoint(onDestroySources[rand].clip, transform.position);
    }
}
