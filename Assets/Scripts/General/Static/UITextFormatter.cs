using Unity.VisualScripting;
using UnityEngine;

namespace ZombieSurvival.General
{
    public static class UITextFormatter
    {
        public static string GetColoredString(string str, Color color)
        {
            return "<color=#" + color.ToHexString() + ">" + str + "</color>";
        }

        public static string GetBoldString(string str)
        {
            return "<b>" + str + "</b>";
        }
        
        public static string GetItalicString(string str)
        {
            return "<i>" + str + "</i>";
        }

        public static string GetSizedString(string str, int size)
        {
            return "<size=" + size + ">" + str + "</size>";
        }
    }
}