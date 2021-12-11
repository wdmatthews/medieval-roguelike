namespace MedievalRoguelike.Characters
{
    public class Dodge : Ability
    {
        private DodgeSO _dodgeData;

        public override AbilityType Type => AbilityType.Dodge;

        public override void Initialize(AbilitySO data)
        {
            base.Initialize(data);
            _dodgeData = (DodgeSO)data;
        }

        public override void Use(Character character)
        {

        }

        public override bool CanBeCancelledBy(AbilitySO ability)
        {
            return false;
        }
    }
}
