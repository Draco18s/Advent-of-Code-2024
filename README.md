
C# Solution for Advent of Code 2024

https://adventofcode.com/2024/

```
      --------Part 1---------   --------Part 2---------
Day       Time    Rank  Score       Time    Rank  Score
  9   01:08:24  7855      0   01:30:36  4400      0
  8   00:39:35  5338      0   00:56:05  5359      0
  7   00:30:05  5332      0   00:54:09  6228      0
  6   00:12:37  1757      0   00:36:46  2181      0
  5   00:10:09  1353      0   00:14:38   850      0
  4   00:18:03  3594      0   00:35:19  4255      0
  3   00:03:39   826      0   00:16:16  2655      0
  2   00:19:15  6619      0   00:33:23  5168      0
  1   00:05:20  2163      0   00:10:31  2878      0
```

*Day 1: dumb error on part 1 cost me 60 seconds. Unknown error on part 2 cost me another bit (>60 seconds). I got an answer that was 0.3% off and I don't know how and can't reproduce it. Edit: Figured it out. I must have fat-fingered a ctrl-x when pressing ctrl-s on my input file and lost a line.*
*Day 2: started 11 minutes late because I DIDN'T SET AN ALARM.*
*Day 3: eeey sub-1000. Part 2 took two guesses. My first attempt was incorrectly determining the most recent do/don't instruction.*
*Day 5: eeey sub-1000. Part 2 was "oh I can juse use a custom comparison sort operation."*
*Day 6: solved it using an inefficient solution and found the better one after it was done. The 9 minute execution time was longer than the amount of time than I expected it to take me to work out better.*
*Day 7: part 1 ran "fast enough," part 2 did not. While it was crunching I worked out the optimization.*
*Day 8: accidentally did part 2 for part 1 before seeing the "and double distance" extra note. And took longer on part 2 than should have, because I was too lenient in the difference of doubles...*
*Day 9: I kept getting the wrong answer over and over again. Took forever to locate that I was erroneously creating 0-sized free memory blocks that were messing with the checksum calculation.*