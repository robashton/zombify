Zombify
=========

I am just trying a few things out to see how feasible a pre-done nuget package is

I want the same end-to-end testing ease that I currently experience in node

So far
======

- Can spin up web server to run ASP.NET project (and tear it down)
- Can enable IPC between web server and tests written in JS (for white box testing)
- Can launch zombie up to browse the test instance of that site


    var driver = new Driver('/Path/To/ASP.NET/Site')
    driver.start()

    var browser = new Browser()
    browser.visit(driver.baseHref)
    
    // etc

    driver.stop()


The plan
====

- Formalise some of the spike code
- Create NuGet package(s) for some form of integration with VS (post-build running most likely)
- Experiment with pooling XSP processes for faster tests


Can I use it?
====

- Not yet most likely, really am just trying to see where I am with it
- See this commit for what the tests should look like though

https://github.com/robashton/zombify/blob/77d73e34f25957ca700c2ea63abdf74eb3dbcfdb/zombify/test/proper/viewing_hotel_list_with_booked_room.coffee
