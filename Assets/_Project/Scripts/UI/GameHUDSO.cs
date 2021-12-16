using UnityEngine;

namespace MedievalRoguelike.UI
{
    [CreateAssetMenu(fileName = "Game HUD", menuName = "Medieval Roguelike/UI/Game HUD")]
    public class GameHUDSO : ScriptableObject
    {
        public GameHUD HUD { get; set; }
    }
}
