namespace MedievalRoguelike.Characters
{
    public abstract class Ability
    {
        protected AbilitySO _data;

        public AbilitySO Data => _data;
        public abstract AbilityType Type { get; }
        public virtual bool CanUse => true;

        public static Ability Create(AbilityType type, AbilitySO data)
        {
            Ability ability = null;

            if (type == AbilityType.Attack1 || type == AbilityType.Attack2
                || type == AbilityType.Attack3) ability = new Attack();
            else if (type == AbilityType.Dodge) ability = new Dodge();
            else if (type == AbilityType.Block) ability = new Block();

            ability.Initialize(data);
            ability.Reset();
            return ability;
        }

        public virtual void Initialize(AbilitySO data)
        {
            _data = data;
        }

        public abstract void Use(Character character);

        public abstract bool CanBeCancelledBy(AbilitySO ability);

        public virtual void OnUpdate() { }

        public virtual void OnAnimationTick(Character character) { }

        public virtual void OnAnimationEnd(Character character) { }

        public virtual void Cancel(Character character) { }

        public virtual void Reset() { }
    }
}
