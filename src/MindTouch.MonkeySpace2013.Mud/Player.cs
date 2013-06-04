using System.Collections.Generic;
using System.Linq;

namespace MindTouch.MonkeySpace2013.Mud {
    public class Player : Entity, IPlayer {
        public List<Item> Inventory = new List<Item>();
        public Room Location;
        public TheHouse TheHouse;
        public Queue<Speech> Heard = new Queue<Speech>();

        public IEnumerable<string> Listen() {
            while(Heard.Any()) {
                var speech = Heard.Dequeue();
                if(speech.Where == null) {
                    yield return string.Format("{0} {1}", speech.Who == this ? "You" : speech.Who.Name, speech.What);
                    continue;
                }
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

        public string GetLocation() {
            return Location.Name;
        }

        public void Tell(Room location, Player player, string said) {
            Heard.Enqueue(new Speech(location, player, said));
        }

        public void Dispose() {
            TheHouse.Leave(this);
        }
    }
}