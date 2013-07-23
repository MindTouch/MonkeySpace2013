using System.Collections.Generic;

namespace MindTouch.ConsoleUI {
    public interface IPopup {
        void Show();
        void Hide();
        void SetContent(IEnumerable<string> lines);
    }
}