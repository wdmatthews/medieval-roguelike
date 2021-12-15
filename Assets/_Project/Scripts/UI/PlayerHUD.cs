using UnityEngine.UIElements;

namespace MedievalRoguelike.UI
{
    public class PlayerHUD
    {
        private Label _nameLabel;
        private VisualElement _healthBar;

        public void Initialize(VisualTreeAsset hudAsset, VisualElement container, string name, float healthPercent)
        {
            VisualElement hud = hudAsset.Instantiate().ElementAt(0);
            _nameLabel = hud.Q<Label>("Name");
            _healthBar = hud.Q<VisualElement>("HealthBarFill");
            container.Add(hud);
            _nameLabel.text = name;
            UpdateHealth(healthPercent);
        }

        public void UpdateHealth(float healthPercent)
        {
            _healthBar.style.width = new StyleLength(new Length(100 * healthPercent, LengthUnit.Percent));
        }
    }
}
