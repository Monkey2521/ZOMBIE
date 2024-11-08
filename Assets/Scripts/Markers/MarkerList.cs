using System.Collections.Generic;

using UnityEngine;

namespace ZombieSurvival.General
{
    [System.Serializable]
    public class MarkerList
    {
        [SerializeField] private List<Marker> _markers;
        [Tooltip("Min markers match count for getting upgrade")]
        [SerializeField] private int _minMatchesCount = 1;

        public List<Marker> Markers => _markers;

        public bool IsStrike(List<Marker> markers)
        {
            int matches = 0;

            foreach (Marker marker in markers)
            {
                if (Contains(marker)) matches++;

                if (matches >= _minMatchesCount) return true;
            }

            return false;
        }

        public bool IsStrike(MarkerList markers) => IsStrike(markers.Markers);

        public bool IsStrike(Marker marker)
        {
            if (_minMatchesCount == 1)
            {
                return Contains(marker);
            }

            else return false;
        }

        private bool Contains(Marker marker) => _markers.Find(item => item.Equals(marker));
    }
}