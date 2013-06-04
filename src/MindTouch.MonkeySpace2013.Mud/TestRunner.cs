using System;
using System.Linq;
using System.Net;
using System.Threading;
using MindTouch.ConsoleUI;

namespace MindTouch.MonkeySpace2013.Mud {
    public static class TestRunner {

        public static void Main(string[] args) {
            var text = new[] {
@"Lorem ipsum dolor sit amet, consectetur adipiscing elit.
Donec vel nisi ac enim ullamcorper laoreet at ultricies arcu.
Sed vel nibh ut nunc accumsan laoreet sed non massa. ",
@"Curabitur nulla neque, sodales semper volutpat sit amet, ornare quis elit.
Donec fringilla nisl id dolor tempor quis ullamcorper enim adipiscing.",
@"

In vel nisi odio. Phasellus metus nisi, pulvinar ut pellentesque at, porta quis quam.
Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.
",
@"Nulla aliquam lacus quis lectus ullamcorper non ornare nisi tincidunt.
Donec venenatis pharetra enim eu sodales. Nulla tempor vestibulum lorem, sed sagittis purus tristique in. Vestibulum in nibh quis nunc ullamcorper tempus vel in turpis.

",
@"Fusce laoreet pulvinar felis at ornare.
Cras et tortor ipsum, a fermentum sapien. Nam congue massa at quam sollicitudin tincidunt.
Donec ut bibendum urna. Praesent vel nisl purus.
",
@"Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Aenean nibh massa, venenatis ac consectetur non, mollis vel ante. Sed imperdiet mauris nunc, sit amet volutpat magna."};
            var host = new ConsoleViewHost();
            var v1 = new FramedView(host, 5, 0, 70, 10);
            var v2 = new FramedView(host, 10, 15, 40, 10);
            v1.Title = "Top Pane";
            var v3 = new FramedView(host, 40, 10, Console.WindowWidth - 40, Console.WindowHeight - 10);
            host.Invalidate();
            while(true) {
                foreach(var t in text) {
                    v1.Write(t);
                    v2.Write(t);
                    v3.Write(t);
                    Thread.Sleep(500);
                }
            }
        }


    }
}