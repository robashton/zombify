var Browser = require('zombie')
var should = require('should')
var Driver = require('./driver')

describe("Listing the hotel rooms", function() {
  var driver = new Driver('../Hotelier/Hotelier')
    , client = null


  before(function(done) {
    driver.start(done)
  })

  describe("With a single hotel room", function() {
    before(function(done) {
      driver.invoke('InMemoryHotel.AddHotelRoom', {
        id: '200',
        number: '101'
      }, done)
    })

    describe("Viewing the home page", function() {
      before(function(done) {
        client = new Browser()
        client.visit(driver.baseHref, done)
      })
      after(function(done) {
        driver.stop(done)
      })

      it("should have the hotel with the correct number", function() {
        client.querySelector('.hotel .number').textContent
          .should.equal('101')
      })

      it("should have a link to manage the hotel room", function() {
        client.querySelector('.hotel a').getAttribute('href')
          .should.include('200')
      })
    })
  })
})
