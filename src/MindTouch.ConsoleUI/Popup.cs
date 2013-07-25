using System.Collections.Generic;
using System.Linq;

namespace MindTouch.ConsoleUI {
    public class Popup : IPopup {
        private readonly IViewHost _host;
        private FramedView _view;

        public Popup(IViewHost host) {
            _host = host;
            _view = new FramedView(host, 0, 0, 2, 2);
        }

        public void Show() {
            _view.Top = _host.CursorTop - _view.Height;
            _view.Left = _host.CursorLeft;
            _view.Show();
        }

        public void Hide() {
            _view.Hide();
        }

        public void SetContent(IEnumerable<string> lines) {
            _view.Clear();
            _view.Height = lines.Count()+3;
            foreach(var line in lines) {
                if(_view.Width < line.Length + 2) {
                    _view.Width = line.Length + 2;
                }
                _view.WriteLine(line);
            }
        }
    }
}