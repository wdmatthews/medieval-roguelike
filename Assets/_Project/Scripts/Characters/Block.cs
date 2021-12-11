namespace MedievalRoguelike.Characters
{
    public class Block : Ability
    {
        private BlockSO _blockData;

        public override AbilityType Type => AbilityType.Block;

        public override void Initialize(AbilitySO data)
        {
            base.Initialize(data);
            _blockData = (BlockSO)data;
        }

        public override void Use(Character character)
        {

        }

        public override bool CanBeCancelledBy(AbilitySO ability)
        {
            return false;
        }

        public override void Cancel(Character character)
        {
            
        }
    }
}
