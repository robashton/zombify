Driver = require 'zombify'
Client = require './client'

class System
  constructor: ->
    @driver = new Driver '../{Replace this with your own website name'

  start: (done) =>
    @driver.start done

  stop: (done) =>
    @driver.stop done

  add_client: =>
    new Client(@driver.baseHref)
    

module.exports = System
