using UnityEngine;

namespace MedievalRoguelike.UI
{
    [CreateAssetMenu(fileName = "Game HUD", menuName = "Medieval Roguelike/Managers/Game HUD")]
    public class GameHUDSO : ScriptableObject
    {
        public GameHUD HUD { get; set; }
    }
}
