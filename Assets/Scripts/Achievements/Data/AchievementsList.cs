using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvival.Achievements
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Achievements/Achievement list", fileName = "New achievements list")]
    public class AchievementsList : ScriptableObject
    {
        [SerializeField] private List<Achievement> _achievements;

        public List<Achievement> Achievements => _achievements;
    }
}