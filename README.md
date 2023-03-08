# Open Event System package for Unity
## overview

Unity events systems can be built out in multiple ways, each with their Pros and Cons. However, the biggest problem I have come across in large projects is when building event systems that are more dynamic, you start to lose track of who is calling what. This makes it harder to debug exactly where a problem is coming from and sometimes leads to the creation of systems that already existed.

This Package aims to solve this by combining a Scriptable Object Event System with a custom editor graph showing all Components that are connected to each event network, whilst also being able to monitor and trigger event calls within the Editor and at Runtime.
