var Browser = require('zombie')
var should = require('should')
var Driver = require('zombify')

describe("the running of the web server", function() {
  var driver = new Driver('../Hotelier/')
    , client = null

  before(function(done) {
    driver.start(done)
  })

  describe("vising the home page", function() {
    before(function(done) {
      client = new Browser()
      client.visit(driver.baseHref, done)
    })

    it('should return HTTP 200', function() {
      client.statusCode.should.equal(200)
    })
  })
})
