Driver = require '../driver'
Client = require './client'

class System
  constructor: ->
    @driver = new Driver '../Hotelier/Hotelier'

  start: (done) =>
    @driver.start done

  stop: (done) =>
    @driver.stop done

  add_hotel_room: (data, done) =>
    @driver.invoke 'InMemoryHotel.AddHotelRoom', data, done

  create_booking: (data, done) =>
    @driver.invoke 'InMemoryHotel.CreateBooking', data, done

  add_client: =>
    new Client(@driver.baseHref)
    

module.exports = System
