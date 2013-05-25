using System.Collections.Generic;
using System.Linq;

namespace Droog.MonkeySpace2013.Mud {

    public enum Direction {
        North,
        South,
        East,
        West,
        Up,
        Down
    }
    public class Player : Entity {
        public List<Item> Inventory = new List<Item>();
        public Room Location;
        public TheHouse TheHouse;
        public Queue<Speech> Heard = new Queue<Speech>();

        public void ChangeName(string name) {
            TheHouse.Players[name] = this;
            TheHouse.Players.Remove(Name);
            Name = name;
        }

        public IEnumerable<string> Listen() {
            while(Heard.Any()) {
                var speech = Heard.Dequeue();
                if(speech.Where != Location) {
                    continue;
                }
                yield return string.Format("{1} said \"{0}\".", speech.What, speech.Who == this ? "You" : speech.Who.Name);
            }
        }

        public string Look() {
            return Location.Look(this);
        }

        public string Go(Direction direction) {
            var location = Location[direction] as Room;
            if(location == null) {
                return "You can't go that way.";
            }
            location.Enter(this);
            return Location.Look(this);
        }

        public void Say(string said) {
            foreach(var player in Location.Players) {
                player.Tell(Location, this, said);
            }
        }

        private void Tell(Room location, Player player, string said) {
            Heard.Enqueue(new Speech(location, player, said));
        }
    }

    public class Speech {
        public Room Where;
        public Player Who;
        public string What;

        public Speech(Room where, Player who, string what) {
            Where = where;
            Who = who;
            What = what;
        }
    }
}