using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CASP.SoundManager
{

    public class SoundManager : SingletoneBase<SoundManager>
    {
        public Sound[] sounds;
        [Header("Sound Pitch Value")]
        public float pitchValue = 1;


        private void Awake()
        {

            if (Instance.GetInstanceID() != this.GetInstanceID())
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            foreach (var s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.loop = s.Loop;
                s.source.DORestart();
                s.source.clip = s.Clip;
                s.source.volume = s.Volume;
                s.source.pitch = s.Pitch;
            }
        }


        public void Play(string name, bool loop)
        {
            Sound s = System.Array.Find(sounds, sound => sound.Name == name);
            if (s == null)
                return;
            if (loop)
            {
                s.source.Play();
            }
            else
            {
                if (!s.source.isPlaying)
                {
                    s.source.PlayOneShot(s.Clip);
                }
            }
        }
        public void Stop(string name)
        {
            Sound s = System.Array.Find(sounds, sound => sound.Name == name);
            if (s == null)
                return;
            s.source?.Stop();
        }
    }
}

