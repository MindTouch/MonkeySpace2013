var clacks = require('../lib/clacks.js');

var client = new clacks.Client(11211, '192.168.0.99');
client.port = 11211;
client.host = '192.168.0.99';

client.on('connect', function(){
  client.query('JOIN jack',function(error,result) {
    client.get('foo', function(error,result) {
      console.log('foo:'+result);
    });
  });
});

client.connect();