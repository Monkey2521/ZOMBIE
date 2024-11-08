using UnityEngine;
using ZombieSurvival.General.Sounds;

namespace ZombieSurvival.Events
{
    public interface ISoundPlayHandler : ISubscriber
    {
        public void OnSoundPlay(SoundType sound);
    }
}