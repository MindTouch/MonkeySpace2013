using System;
using System.Collections.Generic;
using System.Drawing;

namespace MindTouch.TextUI {
    public abstract class Host : IHost {

        protected readonly object _syncroot = new object();
        protected readonly List<IPanel> _panels = new List<IPanel>();
        private bool invalidating = false;

        public Size Size { get; protected set; }

        public IEnumerable<IPanel> Children { get { return _panels; } }

        public void AddChild(IPanel panel) {
            lock(_syncroot) {
                panel.Blur();
                panel.Hide();
                panel.Parent = this as IPanelHost;
                panel.Environment = this as IEnvironment;
                panel.PanelChanged += OnPanelChanged;
                _panels.Add(panel);
                panel.Invalidate();
            }
        }

        private void OnPanelChanged(object sender, InvalidationArgs args) {
            var panel = sender as IPanel;
            if(panel == null) {
                return;
            }
            AssertPanel(panel);
            lock(_syncroot) {
                var invalidator = false;
                try {
                    if(!invalidating) {
                        invalidating = true;
                        invalidator = true;
                    }
                    if(panel.HasFocus) {
                        _panels.Remove(panel);
                        foreach(var p in _panels) {
                            p.Blur();
                        }
                        _panels.Add(panel);
                    }
                    if(invalidator) {
                        Update();
                    }
                } finally {
                    if(invalidator) {
                        invalidating = false;
                    }
                }
            }
        }

        protected void AssertPanel(IPanel panel) {
            if(!_panels.Contains(panel)) {
                throw new InvalidOperationException("Panel does belong to this host");
            }
        }

        protected abstract void Update();

        public void RemoveChild(IPanel panel) {
            lock(_syncroot) {
                panel.PanelChanged -= OnPanelChanged;
                if(!_panels.Remove(panel)) {
                    return;
                }
                panel.Parent = null;
                panel.Environment = null;
                Update();
            }
        }

        public event EventHandler DimensionsChanged;

        protected void OnDimensionsChanged() {
            if(DimensionsChanged != null) {
                DimensionsChanged(this, EventArgs.Empty);
            }
        }
    }
}