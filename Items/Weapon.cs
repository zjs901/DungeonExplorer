namespace DungeonExplorer.Items
{
    public abstract class Weapon : Item
    {
        public Weapon(string name, string description, int damage)
            :base(name, description)
        {
            EquippedDamage = damage;
        }
        
        public int EquippedDamage
        {
            get;
            set;
        }
        
        
    }
}