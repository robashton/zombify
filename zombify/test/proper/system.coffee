Driver = require '../driver'
Client = require './client'

class System
  @driver = new Driver '../Hotelier/Hotelier'

  start: (done) =>
    @driver.start done

  stop: (done) =>
    @driver.stop done

  add_hotel_room: (done) =>
    @driver.invoke 'InMemoryHotel.AddHotelRoom', data, done

  add_client: =>
    new Client(@driver.baseHref)
    
