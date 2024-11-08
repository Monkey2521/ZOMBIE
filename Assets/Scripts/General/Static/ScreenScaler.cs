using UnityEngine;

namespace ZombieSurvival.General
{
    public static class ScreenScaler 
    {
        public readonly static int ReferencedScreedWidth = 1080;
        public readonly static int ReferencedScreedHeight = 1920;

        public static int ScreenWidth => Screen.width;
        public static int ScreenHeight => Screen.height;

        public static float DeltaWidth => (float)ScreenWidth / ReferencedScreedWidth;
        public static float DeltaHeight => (float)ScreenHeight / ReferencedScreedHeight;

        public static float MinDelta => DeltaWidth < DeltaHeight ? DeltaWidth : DeltaHeight;
    }
}