using UnityEngine.Audio;
using System;
using UnityEngine;

namespace Code
{
   
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private Sound[] sounds;
        public static AudioManager instance { get; private set; } = null;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }

            instance= this;

            DontDestroyOnLoad(gameObject);

            foreach (Sound sound in sounds) 
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.loop = sound.loop;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
            }
        }

        public void PlayAudio(int id) 
        {
            Sound s = Array.Find(sounds, s => s.ID == id);
            s.source.Play();
        }

        public Sound GetSound(int id) 
        {
            return Array.Find(sounds, s => s.ID == id);
        }
    }
}
