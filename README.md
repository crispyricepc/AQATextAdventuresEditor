# AQA Text Adventures Editor

This is a small program designed to open .gme files used in AQA's 2019 Computer Science A Level Skeleton Program.
The program is written in multiple different programming languages, but they all require a binary file to run. The binary file stores 3 dynamic lists:
* Characters
* Places
* Items

**Characters** are people like me and you. In the original flag1.gme, AQA included two characters, *"me"* and *"guard"*. The program currently cannot edit characters

**Places** are the areas that the player can explore. Each place has a description and contains items

There are many different types of **Items**, *doors*, *keys* and *containers* just name a few. Items are by far the most complicated element to the .gme file, they store lists called *status*, *result* and *commands* that are stored in plaintext, which means they need to be converted to and from their plaintext version each time.
