# BackEndAssignment

To run and test open the .sln file, build and run "LogTest" and then "NumberOfLinesTest" tests <b>individually</b>.

The reason for this is that during server shut-down the remaining entries in the log queue are flushed to the file. meaning that the correct number of lines will only show after the server shuts down.
