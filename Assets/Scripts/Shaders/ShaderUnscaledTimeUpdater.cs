using UnityEngine;

namespace ZombieSurvival.Shaders
{
    public class ShaderUnscaledTimeUpdater : MonoBehaviour
    {
        [SerializeField] private Material _shaderMaterial;

        private void LateUpdate()
        {
            _shaderMaterial.SetFloat("_UnscaledTime", Time.unscaledTime);
        }
    }
}