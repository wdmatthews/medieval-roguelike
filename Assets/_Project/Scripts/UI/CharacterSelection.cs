using UnityEngine;
using UnityEngine.UIElements;

namespace MedievalRoguelike.UI
{
    [AddComponentMenu("Medieval Roguelike/UI/Character Selection")]
    public class CharacterSelection : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;

        private Button _startButton;

        public System.Action OnStartGame { get; set; }

        private void Awake()
        {
            VisualElement container = _document.rootVisualElement;
            _startButton = container.Q<Button>("Start");
            _startButton.clicked += StartGame;
            HideStartButton();
        }

        public void ShowStartButton()
        {
            _startButton.style.visibility = new StyleEnum<Visibility>(Visibility.Visible);
        }

        public void HideStartButton()
        {
            _startButton.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
        }

        private void StartGame()
        {
            OnStartGame?.Invoke();
        }
    }
}
