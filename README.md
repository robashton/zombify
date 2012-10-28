Zombify
=========

Making end-to-end testing with ASP.NET MVC dead easy.

What
======

- Install the NuGet Package
- Write some tests in either Coffeescript or Javascript

```coffee
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
```

- Run Zombify 

```text
    zombify.bat mytests
```

- View results that look like this

```text
    Scenario Viewing hotel list with a booked room
      Given a hotel room which is booked
      When the user visits the hotel list
      then the hotel room should display that it is booked
```

- Profit

Feature List
======

- Contains all you need in order to do end-to-end testing
  - A standalone web server
  - A headless browser
- IPC between your Javascript and .NET (for white box testing)
- Comes with coffeescript and a pile of BDD language helpers
- Base context classes for driving/abstracting tests against the application


Running Zombify
======

- Install the NuGet package to your test project (MyProject.Tests for example)
- Write tests inside the 'test' folder in this project
- Folder structure should look like this

```text
    MyApp.sln
    MyApp\WebApp.csproj
    MyApp\Views\Home\etc

    MyApp.Test\MyApp.Tests.csproj
    MyApp.Test\test\mocha.opts
    MyApp.Test\test\viewing_a_page.coffee
    MyApp.Test\test\riding_a_pony.coffee
```

- Run the batch file provided in the package

```text
    .\packages\zombify.0.0.3\bin\zombify.bat MyApp.Test
```

- Feel free to automate this as part of your build process or Visual Studio activity

All the bits are included
======

Zombie:
```coffee
    # This is a headless browser called Zombie, you can find the documentation
    # here: http://zombie.labnotes.org

    Browser = require 'zombie'
```

Mocha:
```coffee
    # This is the test runner being used, meaning you can do things like
    # Find the documentation for that here: https://visionmedia.github.com/mocha

    describe "sitting down", ->
      it "sits down", ->
        ok(seat.has_bum_on_it())
```

Mocha-Cakes:
```coffee
    # I don't really like the default language of Mocha, so I use Mocha cakes to give us
    # You can find the documentation for that here: https://github.com/quangv/mocha-cakes/

    Scenario "Sitting down", ->
      Given "A chair", ->
        chair = new Chair()
      When "Somebody sits down", ->
        person.sit_down_on(chair)
      Then "The chair is full", ->
        ok(chair.has_bum_on_it())
```

Should:
```coffee
    # Assets are old hat, I want to should all the things
    # Documentation for this can be found here: https://github.com/visionmedia/should.js
    chair.has_bum_on_it().should.equal(true)
```

Zombify:
```coffee
    # This is the bit that runs a web server and allows IPC and whatever to occur
    # documentation: You're reading it

    driver = require('zombify')
    
    server = new Driver('/path/to/asp.net/mvc/application')
    server.start ->
      server.invoke 'method', { one: chicken, two: pony }
      server.stop
```

Talking to the server from JS/Coffeescript
=====

So, there is a zombify driver present and it is running your awesome website, but you want to reach into the backend to
set up some known state (and you don't mind running with scissors)

Let's create a class in C#

```csharp
    public class HotelCommands {
      IHotelRepository hotels;

      public HotelCommands(IHotelRepository repository) {
        hotels = repository;
      }

      public void AddHotel(string name, int capacity) {
        hotels.Add(new Hotel(name, capacity));
      }
    }
```

How do we get to thise from JS/Coffeescript?
Simples - create a method on your global application class (found in global.asax.cs) like so

```csharp
    public class MvcApplication : HttpApplication
    {
        public static IEnumerable<object> RetrieveZombieHandlers() {
          yield return container.Get<HotelCommands>();
        }
    }
```

That's it - no need to add assembly references or faff around with interfaces or whatever
It can now be called from the zombify driver in Coffeescript like this

```coffee
    server.invoke 'HotelCommands.AddHotel', {
      name: "The ritz"
      capacity: 2000 
    }, -> 
      console.log('Done')
```

Or in Javascript like this

```javascript
    server.invoke('HotelCommands.AddHotel', {
      name: "The ritz",
      capacity: 2000
    }, function() {
      console.log('done')
    })
```

The rest is up to you
======

These are bits, and they are bits that work together well - feel free to fork this project and do it your own way
You can either use the handy Zombify System + Client base classes, or just go and use the Zombify driver and Zombie browser
manually - it's up to you



