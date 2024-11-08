using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using ZombieSurvival.General;

namespace ZombieSurvival.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [Header("Debug settings")]
        [SerializeField] private bool _isDebug;

        [Header("Settings")]
        [SerializeField] private string _path = "LoadingData.dat";
        [SerializeField] private Text _applicationVersionText;
        [SerializeField] private Text _loadingDescription;

        [SerializeField] private LoadingBar _loadingBar;

        private bool _onLoad;
        private bool _skipTutorial;

        [Header("Test")]
        public int loadingTime;
        private float _timer;

        private void OnEnable()
        {
            _applicationVersionText.text = Application.version;
            _loadingBar.Initialize();
            _timer = 0;

            if (!LoadData())
            {
                SaveData();
            }
        }

        private void FixedUpdate()
        {
            _timer += Time.fixedDeltaTime;

            if (_timer <= loadingTime)
            {
                _loadingBar.SetValue((int)(_timer / loadingTime * 100));
            }
            if (_timer >= loadingTime)
            {
                _loadingBar.SetValue(100);
            }

            float updater = _timer % 1.5f;
            if (updater <= 0.5f)
            {
                _loadingDescription.text = "Loading.";
            }
            else if (updater <= 1f)
            {
                _loadingDescription.text = "Loading..";
            }
            else if (updater <= 1.5f)
            {
                _loadingDescription.text = "Loading...";
            }

            if (_timer >= loadingTime && !_onLoad)
            {
                _onLoad = true;

                if (_skipTutorial)
                {
                    SceneManager.LoadSceneAsync(GameData.MainMenuScene);
                }
                else
                {
                    SceneManager.LoadSceneAsync(GameData.FirstTutorialScene);
                }
            }
        }

        private bool LoadData()
        {
            if (GameData.Load(GameData.DefaultPath + _path) is LoadingData data)
            {
                _skipTutorial = data.skipTutorial;

                return true;
            }

            return false;
        }

        private bool SaveData()
        {
            LoadingData data = new LoadingData();

            data.skipTutorial = true;

            return GameData.Save(GameData.DefaultPath + _path, data);
        }

        [System.Serializable]
        private class LoadingData : SerializableData
        {
            public bool skipTutorial;
        }
#if DEBUG
        [ContextMenu("Reset data")]
        private void ResetData()
        {
            if (File.Exists(GameData.DefaultPath + _path))
            {
                File.Delete(GameData.DefaultPath + _path);
            }
        }
#endif
    }
}