using System;
using System.Text;
using MindTouch.Clacks.Client;

namespace MindTouch.MonkeySpace2013.Client {
    public static class RequestExtensions {
        public static Request WithData(this Request r, string data) {
            return r.WithData(Encoding.ASCII.GetBytes(data));
        }
        public static Response Exec(this ClacksClient c, string command, Action<Request> callback = null) {
            var request = Request.Create(command);
            if(callback != null) {
                callback(request);
            }
            return c.Exec(request);
        }
    }
}