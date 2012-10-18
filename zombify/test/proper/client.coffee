Browser = require('zombie')

class Client
  @browser = new Browser()

  constructor: (base) ->
    @base = base

  visit_hotel_list: (done) =>
    browser.visit @base, @handleVisit(done)

  hotel_room_is_booked: (id) =>
    hotel = @browser.querySelector '#hotel-' + id
    hotel.getAttribute('class').indexOf('booked') != -1

  handle_visit: (cb) =>
    =>
      if(@browser.statusCode != 200)
        message = @browser.querySelector('h1').textContent
        stacktrace = @browser.querySelector('tbody pre').textContent
        console.error(message, stacktrace)
      cb()

