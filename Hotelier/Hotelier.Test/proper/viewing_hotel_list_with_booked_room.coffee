System = require './system'

Scenario "Viewing hotel list with a booked room", ->
  system = new System()
  client = system.add_client()

  Given "A hotel room which is booked", (done) ->
    system.start ->
      system.add_hotel_room {
        id: 100
        number: 101
        capacity: 2
      }, ->
        system.create_booking {
          roomid: 100
        }, done

  When "The user visits the hotel list", (done) ->
    client.visit_hotel_list done
    
  Then "The hotel room should display that it is booked", ->
    client.hotel_room_is_booked(100).should.equal(true)
    
  after (done) ->
    system.stop done

