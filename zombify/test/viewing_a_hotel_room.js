var should = require('should')
var Driver = require('./driver')
var Browser = require('zombie')

describe("Viewing a hotel room", function() {
  var driver = new Driver('../Hotelier/Hotelier')
    , client = null

  before(function(done) {
    driver.start(done)
  })
  after(function(done) {
    driver.stop(done)
  })

  describe('With a single hotel room', function() {
    before(function(done) {
      driver.invoke('InMemoryHotel.AddHotelRoom', {
        id: '200',
        number: '101',
        capacity: 3
      }, done)
    })

    describe("Viewing the room's page", function(done) {
      before(function(done) {
        client = new Browser({debug: true})
        client.visit(driver.baseHref + '/Room?id=200', handleVisit(client, done))
      })

      it('Should have the hotel room number as the header', function() {
        client.querySelector('h1').textContent
          .should.equal('101')
      })

      it('Should state the room\'s capacity', function() {
         client.querySelector('.capacity').textContent
          .should.equal('3')
      })
    })
  })
})


function handleVisit(client, cb) {
  return function() {
    if(client.statusCode !== 200) {
      console.log(client.html())
    }
    cb()
  }
}