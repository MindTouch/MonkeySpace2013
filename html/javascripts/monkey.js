/*------------------------------------------------------------------------*
 * Copyright 2013 Arne F. Claassen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *-------------------------------------------------------------------------*/
(function (root, $, _) {
  Josh.Example = (function (root, $, _) {

    // Enable console debugging, when Josh.Debug is set and there is a console object on the document root.
    var _console = (Josh.Debug && root.console) ? root.console : {
      log: function () {
      }
    };
    var ws = new Websock();

    var callQueue = [];
    var online = false;
    ws.on('message', receive);
    ws.on('open', function (e) {
      _console.log("WebSockets.onopen");
      connect();
    });
    ws.on('close', function (e) {
      _console.log("WebSockets.onclose");
      disconnect();
    });
    ws.on('error', function (e) {
      _console.log("WebSockets.onerror");
      disconnect();
    });

    var shell = Josh.Shell({console: _console});
    shell.templates.look = _.template("<div><% _.each(lines, function(line) { %><div><%=line%></div><% }); %></div>");
    shell.templates.input_cmd = _.template('<div id="<%- id %>"><div class="listener"></div><span class="prompt"></span>&nbsp;<span class="input"><span class="left"/><span class="cursor"/><span class="right"/></span></div>');

    function send(cmd, args, data, callback) {
      var msg = cmd;
      if (args) {
        for (var i = 0; i < args.length; i++) {
          msg += " " + args[i];
        }
      }
      if (data) {
        msg += " " + data.length + "\r\n";
        msg += data + "\r\n";
      } else {
        msg += "\r\n";
      }
      var bytes = [];
      for (var i = 0; i < msg.length; i++) {
        bytes.push(msg.charCodeAt(i));
      }
      callQueue.push(callback);
      ws.send(bytes);
    }

    function look(callback) {
      send("LOOK",null,null,function(cmd, args, data) {
        callback(shell.templates.look({lines: data}));
      });
    }

    function onLeave() {
      online = false;
      shell.onNewPrompt(function (callback) {
        callback("offline $");
      });
      shell.clearCommandHandler("go");
      shell.clearCommandHandler("leave");
      shell.setCommandHandler("join", {
          exec: function (cmd, args, callback) {
            onJoin();
            send("JOIN",null, args.shift(), function() {
              look(callback);
            });
          }
        }
      );
    }

    function onJoin() {
      online = true;
      function listen() {
        if(!online) {
          return;
        }
        _console.log("listening");
        send("LISTEN",null,null,function(cmd,args,data) {
          if(data) {
            for(var i=0;i<data.length;i++){
              if(data[i]) {
                $('#shell-cli .listener').append("<div>"+data[i]+"</div>");
              }
            }
          }
          root.setTimeout(listen,1000);
        });
      }
      shell.onNewPrompt(function (callback) {
        callback("online $");
      });
      root.setTimeout(listen,1000);
      shell.clearCommandHandler("join");
      shell.setCommandHandler("go", {
          exec: function (cmd, args, callback) {
            send("GO",args,null,function(cmd,args,data) {
              callback(shell.templates.look({lines: data}));
            })
          },
          completion: function (cmd, arg, line, callback) {
            callback(shell.bestMatch(arg,["north","south","east","west","up","down"]));
          }
        }
      );
      shell.setCommandHandler("look", {
          exec: function (cmd, args, callback) {
            look(callback);
          }
        }
      );
      shell.setCommandHandler("say", {
          exec: function (cmd, args, callback) {
            send("SAY",null,args.join(" "), function() {
              callback();
            })
          }
        }
      );
      shell.setCommandHandler("leave", {
          exec: function (cmd, args, callback) {
            onLeave();
            send("LEAVE",null,null,function(cmd,args,callback) {
              callback("going offline");

            });
          }
        }
      );
    }

    function connect() {
      onLeave();
      shell.activate();
    }

    function disconnect() {
    }

    function receive() {
//      _console.log("receive");
      var arr = ws.rQshiftBytes(ws.rQlen());
      var str = "";
      var chr;
//      _console.log("Received array '" + arr + "'");
      while (arr.length > 0) {
        chr = arr.shift();
        str += String.fromCharCode(chr);
      }
//      _console.log("received: " + str);
      var lines = str.split("\r\n");
      var line1 = lines[0].split(" ");
      var cmd = line1[0];
      var args = _.rest(line1);
      var data = _.rest(lines);
//      _console.log("cmd: "+cmd);
//      _console.log("args: " + args);
//      _console.log("data: " + data);
//      _console.log(data);
      var callback = callQueue.shift();
      callback(cmd,args,data);
    }


    $(document).ready(function () {
      var $consolePanel = $('#shell-panel');
      _console.log("connecting");
      var uri = "ws://" + root.location.hostname + ":8888";
      _console.log("connecting to " + uri);
      ws.open(uri);
    });
  })(root, $, _);
})(this, $, _);
