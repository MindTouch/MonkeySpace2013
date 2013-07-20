using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindTouch.MonkeySpace2013.Mud.Dungeon {
    public class Room : Space {
        public List<Item> Items = new List<Item>();
        public List<Player> Players = new List<Player>();
        public Dictionary<Direction, Space> Spaces = new Dictionary<Direction, Space>();

        public virtual string Look(Player who) {
            var builder = new StringBuilder();
            builder.AppendLine(Name);
            builder.AppendLine();
            builder.AppendLine(Description);
            builder.AppendLine();
            foreach(var kvp in Spaces) {
                var connector = kvp.Value as Connector;
                if(connector != null) {
                    var to = connector.GetTo(this);
                    builder.AppendFormat("A {0} leads {2} to the {1} ", connector.Name, to.Name, kvp.Key.ToString().ToLower());
                } else {
                    builder.AppendFormat("The {0} is {1}", kvp.Value.Name, GetDirection(kvp.Key));
                }
                builder.AppendLine();
            }
            if(Items.Any()) {
                builder.AppendLine("You see: " + string.Join(", ", Items.Select(x => x.Name)) + ".");
                builder.AppendLine();
            }
            var others = Players.Where(x => x != who);
            if(others.Any()) {
                if(others.Count() == 1) {
                    builder.AppendLine(others.First().Name + " is here.");
                } else {
                    builder.AppendLine(string.Join(", ", others.Select(x => x.Name)) + " are here.");
                }
            } else {
                builder.AppendLine("You are alone.");
            }
            return builder.ToString();
        }

        private string GetDirection(Direction direction) {
            var name = direction.ToString().ToLower();
            switch(direction) {
            case Direction.Up:
            case Direction.Down:
                return name;
            default:
                return "to the " + name;
            }
        }

        public virtual Space this[Direction direction] {
            get {
                var space = Spaces.ContainsKey(direction) ? Spaces[direction] : null;
                if(space == null) {
                    return null;
                }
                var connector = space as Connector;
                return connector != null ? connector.GetTo(this) : space;
            }
            set { Spaces[direction] = value; }
        }

        public virtual void Enter(Player player) {
            if(player.Location != null) {
                player.Location.Leave(player);
            }
            foreach(var p in Players) {
                p.Tell(null, player, "arrived.");
            }
            Players.Add(player);
            player.Location = this;
        }

        public void Leave(Player player) {
            Players.Remove(player);
            foreach(var p in Players) {
                p.Tell(null, player, "left.");
            }
        }
    }
}