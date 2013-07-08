using System.Drawing;

namespace MindTouch.TextUI {
    public interface IScreen {
        void Write(Point position, char[] line, int start, int width);
    }

    public class Screen : IScreen {
        public Screen() {
            
        }

        public void Write(Point position, char[] line, int start, int width) {
            throw new System.NotImplementedException();
        }
    }
}