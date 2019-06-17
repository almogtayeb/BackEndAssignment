# BackEndAssignment

To run and test open the .sln file, build and run "LogTest" and then "NumberOfLinesTest" tests <b>individually</b>.

The reason for this is that during server shut-down the remaining entries in the log queue are flushed to the file. meaning that the correct number of lines will only show after the server shuts down.

## Description

In this assignment I have implemented a PriorityQueue based on a SortedDictionary in which the keys are the priorities and the values are queues of the actual elements.<br>
<br>
When a Log request is received, the message is stored in this PriorityQueue until a periodic task writes it to the file.<br><br>

