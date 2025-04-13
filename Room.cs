namespace DungeonExplorer
{
    public class Room
    {
        private string _name;
        private string _description;
        private bool _creature;
        private Room _north;
        private Room _east;
        private Room _south;
        private Room _west;

        public Room(string name, string description, bool creature)
        {
            Name = name;
            Description = description;
            Creature = creature;
        }

        // Getters and setters, for the rooms, name, description, if there is a monster or not, and the directions
        public string GetDescription()
        {
            return _description;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string Description
        {
            get => _description;
            set => _description = value;
        }
        public bool Creature
        {
            get => _creature;
            set => _creature = value;
        }

        public Room North
        {
            get => _north;
            set => _north = value;
        }

        public Room East
        {
            get => _east;
            set => _east = value;
        }
        
        public Room South
        {
            get => _south;
            set => _south = value;
        }

        public Room West
        {
            get => _west;
            set => _west = value;
        }
    }
}