var spawn = require('child_process').spawn
var http = require('http')

var Driver = function(dir, options) {
  this.dir = dir
  this.options = options || {}
  this.options.port = this.options.port || 8081
  this.process = null
  this.baseHref = 'http://localhost:' + this.options.port
}

Driver.prototype = {
  start: function(cb) {
    this.process = spawn('xsp2', [ '--port', this.options.port], {
      cwd: this.dir
    })
    this.process.stdout.setEncoding('utf8')
    this.process.stdout.on('data', this.onStdOut.bind(this))
    this.process.stderr.setEncoding('utf8')
    this.process.stderr.on('data', this.onStdErr.bind(this))
    this.process.on('exit', this.onExit.bind(this))
    this.waitForConnection(cb)
  },
  waitForConnection: function(cb) {
    var self = this
    http.get(this.baseHref, function(res) {
      cb()
    }).on('error', function() {
      self.waitForConnection(cb)
    })
  },
  onStdOut: function(data) {
//    console.log(data)
  },
  onStdErr: function(data) {
 //   console.log(data)
  },
  onExit: function(code) {
  //  console.log(code)
  },
  stop: function(err, cb) {
    this.process.kill()
    cb()
  }
}


module.exports = Driver
