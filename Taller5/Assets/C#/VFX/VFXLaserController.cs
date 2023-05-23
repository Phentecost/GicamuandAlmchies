using Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code_VFX
{
    public class VFXLaserController : MonoBehaviour
    {
        public LineRenderer laser;
        public Laser L;
        [SerializeField] ParticleSystem startBeam, endBeam, startParticles, endParticles;

        public void SetUP(Vector3 i)
        {
            laser.SetPosition(1,i);
            laser.SetPosition(0,i);
            startBeam.Stop();
            startParticles.Stop();
            endBeam.Stop();
            endParticles.Stop();
        }

        public void StartFirstHalfVFX() 
        {
            startBeam.Play();
        }

        public void StartSecondHalfVFX() 
        {
            startParticles.Play();
        }

        public void StartRestVFX(Vector3 i) 
        {
            endBeam.gameObject.transform.position = i;
            endParticles.gameObject.transform.position = i;
            endBeam.Play();
            endParticles.Play();
        }

        public void EndFirstHalfVFX() 
        {
            endBeam.Stop();
            endParticles.Stop();
        }

        public void EndAllVFX() 
        {
            startBeam.Stop();
            startParticles.Stop();
        }
    }
}
