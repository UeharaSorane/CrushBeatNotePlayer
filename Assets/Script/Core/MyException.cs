using System;

namespace MyException
{
    public class TrackDataException : Exception { }
    public class TrackDataNotFoundException : TrackDataException { }
    public class TrackInfoHasProblemException : TrackDataException { }
}