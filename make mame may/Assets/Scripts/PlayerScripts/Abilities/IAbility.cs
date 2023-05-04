namespace Player
{
    namespace Abilities
    {
        public interface IAbility
        {
            // Boolean to determine if the ability can be triggered
            bool CanTrigger();

            // Function to trigger the ability
            void Trigger();
        }
    }
}