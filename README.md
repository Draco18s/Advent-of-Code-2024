
C# Solution for Advent of Code 2024

https://adventofcode.com/2024/

```
      --------Part 1---------   --------Part 2---------
Day       Time    Rank  Score       Time    Rank  Score
 25   00:16:35  1792      0       >24h  10572      0
 24   00:12:57   638      0       >24h  12464      0
 23   00:16:40  1842      0   00:37:38   1683      0
 22   00:08:01   970      0   01:48:31   3560      0
 21   04:16:38  3469      0   14:14:25   5763      0
 20   00:17:43   490      0   01:38:33   2496      0
 19   00:53:08  4881      0   13:27:24  17272      0
 18   01:28:50  6254      0   01:34:34   5724      0
 17   01:07:06  4897      0   22:09:30  15089      0
 16   00:10:41   532      0   03:10:52   4674      0
 15   00:22:56  1258      0   02:30:30   3804      0
 14   00:11:46   822      0   00:26:24    487      0
 13   00:12:27   912      0   01:01:18   2882      0
 12   00:40:10  4580      0   02:13:58   4630      0
 11   00:06:29  1016      0   00:14:57    818      0
 10   00:18:36  2729      0   00:21:21   2335      0
  9   01:08:24  7855      0   01:30:36   4400      0
  8   00:39:35  5338      0   00:56:05   5359      0
  7   00:30:05  5332      0   00:54:09   6228      0
  6   00:12:37  1757      0   00:36:46   2181      0
  5   00:10:09  1353      0   00:14:38    850      0
  4   00:18:03  3594      0   00:35:19   4255      0
  3   00:03:39   826      0   00:16:16   2655      0
  2   00:19:15  6619      0   00:33:23   5168      0
  1   00:05:20  2163      0   00:10:31   2878      0
```

*Day 1: dumb error on part 1 cost me 60 seconds. Unknown error on part 2 cost me another bit (>60 seconds). I got an answer that was 0.3% off and I don't know how and can't reproduce it. Edit: Figured it out. I must have fat-fingered a ctrl-x when pressing ctrl-s on my input file and lost a line.*
*Day 2: started 11 minutes late because I DIDN'T SET AN ALARM.*
*Day 3: eeey sub-1000. Part 2 took two guesses. My first attempt was incorrectly determining the most recent do/don't instruction.*
*Day 5: eeey sub-1000. Part 2 was "oh I can juse use a custom comparison sort operation."*
*Day 6: solved it using an inefficient solution and found the better one after it was done. The 9 minute execution time was longer than the amount of time than I expected it to take me to work out better.*
*Day 7: part 1 ran "fast enough," part 2 did not. While it was crunching I worked out the optimization.*
*Day 8: accidentally did part 2 for part 1 before seeing the "and double distance" extra note. And took longer on part 2 than should have, because I was too lenient in the difference of doubles...*
*Day 9: I kept getting the wrong answer over and over again. Took forever to locate that I was erroneously creating 0-sized free memory blocks that were messing with the checksum calculation.*
*Day 10: Started late because exact-start timer didn't chime. Oh well.*
*Day 11: Straight forward fishbuckets problem.*
*Day 12: Part 2: Had to get help from Reddit for this one.*
*Day 13: Part 2 I didn't spot the line-line intersection right away.*
*Day 14: A puzzle from a couple years ago gave me an idea how to approach part 2, turns out that part 1 actually helped solve part 2, but was not obvious.*
*Day 15: Part 2 had a complex failure state that was hard to locate.*
*Day 16: Part 2 decided to be stubborn, for no reason.*
*Day 17: Part 2 was absoltely bonkers hard and was basically reversing a one-way cryptograpic cypher algorithm. Yuck.*
*Day 18: For some reason, pathfinding this maze just did. Not. Want. To. Complete. In a reasonable time frame (like, more open nodes than points on the maze level of bullshittery). Ended up doing a depth-first flood-fill, yuck. Part 2 was relatively trivial at that point. Once again finding a need for a lib pathfind solution.*
*Day 20: Part 2 stymied me for the longest time and when I found the problem I felt stupid. It was exactly the problem Adam had and even when he said it I still couldn't see it.*
*Day 21: Part 2 should work, everything I've debugged says that its accurate. Done it by hand, done it by using part 1's code. Everything agrees how long the sequences should be at depth 3 and above...except someone else's solution. I get depth 1 and 2 correct. Depth 3 should be the same expansion from depth 2 as 2 was from 1. Yet my sequences are the tiniest bit larger at each step (like, 184 to 180). And I just cannot find the reason why. I will probably have to find someone else's solution that actually generates the sequences and be able to compare them to find it. Probably--like in part 1--there's a ridiculously subtle faster path two depths higher if the initial path uses `[...]Avv>A` instead of `[...]A>vvA`. 1 depth higher is the same length. And no other subsequence differs when reversed. Nothing I could change about my pathfinder was capable of making that particular path "more optimal" and had to hard code in a string replacement.*
*Day 22: Part 2 worked on every puzzle input I could get my hands on...except my own. Turns out one of my monkeys would hit the sell conditions as soon it **could** be possible, but I had initialized the lastPrice to -inf rather than to the monkey's secret number... Basically, I got unlucky and could have had the right answer an hour sooner.*
*Day 23: This was pretty straight forward, all things considered.*
*Day 24: Part 2 was an utter nightmare, taking the better part of two days to locate 3 of the pairs by hand (one of which I wasn't too sure about). The last set wasn't even found algorithmically, but required printing out the graph with the help of a redditor's code and examining it for discrepances in the areas I knew there were issues (bits 9 and 38)*
*Day 25: A breeze. Part 2 (star 50) just barely missed the less than 24 hour mark, having to finish up 24p2 and doing so 1 minute before the 24 hour mark and forgetting I had to revisit Day 25's page.*