Browser = require('zombie')

class Client

  constructor: (base) ->
    @base = base
    @browser = new Browser()


  # Put appropriate methods here to abstract what the client is doing


  handle_visit: (cb) =>
    =>
      if(@browser.statusCode != 200)
        message = @browser.querySelector('h1').textContent
        stacktrace = @browser.querySelector('tbody pre').textContent
        console.error(message, stacktrace)
      cb()

module.exports = Client
