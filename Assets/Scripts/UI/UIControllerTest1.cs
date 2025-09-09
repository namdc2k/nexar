using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI{
    public class UIControllerTest1 : MonoBehaviour{
        [SerializeField] Button buttonReload;

        private void Start() {
            buttonReload.onClick.AddListener(() => { SceneManager.LoadScene(0); });
        }
    }
}