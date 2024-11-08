using System.Collections.Generic;

using UnityEngine;

namespace ZombieSurvival.General
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Camp/Talent list", fileName = "New talent list")]
    public class TalentList : ScriptableObject
    {
        [SerializeField] private List<Talent> _talents;

        public List<Talent> Talents => _talents;
    }
}