namespace MedievalRoguelike.Characters
{
    public class Attack : Ability
    {
        private AttackSO _attackData;

        public override AbilityType Type => _attackData.Type;

        public override void Initialize(AbilitySO data)
        {
            base.Initialize(data);
            _attackData = (AttackSO)data;
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
