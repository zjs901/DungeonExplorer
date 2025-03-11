namespace DungeonExplorer
{
    public class Room
    {
        private string _name;
        private string _description;
        private bool _monster;
        private bool _north;
        private bool _south;

        public Room(string name, string description, bool monster, bool north, bool south)
        {
            Name = name;
            Description = description;
            Monster = monster;
            North = north;
            South = south;

        }

        // Getters and setters, for the rooms, name, description, if there is a monster or not, and the directions
        public string GetDescription()
        {
            return _description;
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        public bool Monster
        {
            get
            {
                return _monster;
            }
            set
            {
                _monster = value;
            }
        }
        public bool North
        {
            get
            {
                return _north;
            }
            set
            {
                _north = value;
            }
        }
        public bool South
        {
            get
            {
                return _south;
            }
            set
            {
                _south = value;
            }
        }
    }
}