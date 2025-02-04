﻿// ================================================================================================================================
// File:        NetworkPacket.cs
// Description:	Stores data inside a single packet that is going to be transmitted over the network to the game server
//              Also data recieved from the game server can be placed into one of these to make reading the values much easier
// Author:	    Harley Laurie https://www.github.com/Swaelo/
// ================================================================================================================================

using System;
using UnityEngine;

public class NetworkPacket
{
    public string PacketData;  //Total set of data currently stored in this packet
    public string RemainingPacketData; //Remaining set of data to be read from this packet

    //default constructor
    public NetworkPacket(string PacketData = "")
    {
        //Initialize both data strings with whatever data has been passed to the constructor
        this.PacketData = PacketData;
        RemainingPacketData = PacketData;
    }

    //copy constructor
    public NetworkPacket(NetworkPacket CopyFrom)
    {
        this.PacketData = CopyFrom.PacketData;
        this.RemainingPacketData = PacketData;
    }

    //Checks the current value of the RemainingPacketData to check if there is any data left to be read from it
    public bool FinishedReading()
    {
        //Return true if there is no more data left to read from the RemainingPacketData
        if(RemainingPacketData == "" || RemainingPacketData == " " || RemainingPacketData == null)
            return true;
        //Otherwise we return false is there is still some data left to read
        return false;
    }

    //Resets RemainingPacketData back to total PacketData value
    public void ResetRemainingData()
    {
        RemainingPacketData = PacketData;
    }

    /// <summary>
    /// Adds the packet number to the front of the packet data, then resets RemainingPacketData to the new PacketData value
    /// </summary>
    /// <param name="PacketNumber">Order number to place at the front of the packet</param>
    public void AddPacketOrderNumber(int PacketNumber)
    {
        string NewPacketData = PacketNumber.ToString() + " " + PacketData;
        PacketData = NewPacketData;
        RemainingPacketData = NewPacketData;
    }

    //Writes an interger value onto the end of the current PacketData
    public void WriteInt(int IntValue)
    {
        PacketData += IntValue.ToString() + " ";
    }
    //Reads an integer value from the front of the RemainingPacketData, then removes it from that string
    public int ReadInt()
    {
        //Get the int value from the RemainingPacketData
        string IntValueString = RemainingPacketData.Substring(0, RemainingPacketData.IndexOf(' '));
        int IntValue = Int32.Parse(IntValueString);
        //Trim the int value from the RemainingPacketData
        RemainingPacketData = RemainingPacketData.Substring(RemainingPacketData.IndexOf(' ') + 1);
        //Return the final integer value that was requested
        return IntValue;
    }

    //Writes a floating point value onto the end of the current PacketData
    public void WriteFloat(float FloatValue)
    {
        PacketData += FloatValue.ToString() + " ";
    }
    //Reads a floating point value from the front of the RemainingPacketData, then removes it from that string
    public float ReadFloat()
    {
        //Get the float value from the RemainingPacketData
        string FloatValueString = RemainingPacketData.Substring(0, RemainingPacketData.IndexOf(' '));
        float FloatValue = float.Parse(FloatValueString);
        //Trim the float value from the RemainingPacketData
        RemainingPacketData = RemainingPacketData.Substring(RemainingPacketData.IndexOf(' ') + 1);
        //Return the final floating point value that was requested
        return FloatValue;
    }

    //Writes a boolean value onto the end of the current PacketData
    public void WriteBool(bool BoolValue)
    {
        PacketData += BoolValue ? "1 " : "0 ";
    }
    //Reads a boolean value from the front of the RemainingPacketData, then removes it from that string
    public bool ReadBool()
    {
        //Get the bool value from the RemainingPacketData
        string BoolValueString = RemainingPacketData.Substring(0, RemainingPacketData.IndexOf(' '));
        bool BoolValue = BoolValueString == "1" ? true : false;
        //Trim the bool value from the RemainingPacketData
        RemainingPacketData = RemainingPacketData.Substring(RemainingPacketData.IndexOf(' ') + 1);
        //Return the final bool value that was requested
        return BoolValue;
    }

    //Writes a ServerPacketType enum value onto the end of the current PacketData
    public void WriteType(ServerPacketType PacketType)
    {
        WriteInt((int)PacketType);
    }

    //Writes a ClientPacketType enum value onto the end of the current PacketData
    public void WriteType(ClientPacketType PacketType)
    {
        //Just convert the enum to an integer value and write that in
        WriteInt((int)PacketType);
    }
    //Reads a ServerPacketType enum value from the front of the RemainingPacketData, then removes it from that string
    public ServerPacketType ReadType()
    {
        //Get the packet type value from the RemainingPacketData
        string PacketTypeString = RemainingPacketData.Substring(0, RemainingPacketData.IndexOf(' '));
        ServerPacketType PacketTypeValue = (ServerPacketType)Int32.Parse(PacketTypeString);
        //Trim the packet type value from the RemainingPacketData
        RemainingPacketData = RemainingPacketData.Substring(RemainingPacketData.IndexOf(' ') + 1);
        //Return the final ServerPacketType enum value that was requested
        return PacketTypeValue;
    }

    //Writes a string value onto the end of the current PacketData
    public void WriteString(string StringValue)
    {
        //Write an integer value representing the length of the string first
        WriteInt(StringValue.Length);
        PacketData += StringValue + " ";
    }
    //Reads a string value from the front of the RemainingPacketData, then removes it from that string
    public string ReadString()
    {
        //First read an integer to get the length of the string that is going to be read from the RemainingPacketData
        int StringLength = ReadInt();
        //Use the length to get the correct amount of data for the string value that is being requested
        string StringValue = RemainingPacketData.Substring(0, StringLength);
        //Trim the string value from the RemainingPacketData
        RemainingPacketData = RemainingPacketData.Substring(StringLength + 1);
        //Return the final string value that was requested
        return StringValue;
    }

    //Writes the 3 floating point values of a Vector3 onto the end of the current PacketData
    public void WriteVector3(Vector3 Vector3Value)
    {
        WriteFloat(Vector3Value.x);
        WriteFloat(Vector3Value.y);
        WriteFloat(Vector3Value.z);
    }
    //Reads 3 floating point values from the front of the RemainingPacketData, then removes them from that string, returning them in a Vector3 format
    public Vector3 ReadVector3()
    {
        //Create a new Vector3 object to store all the values within
        Vector3 Vector3Value = new Vector3();
        //Read 3 floating point values from the RemainingPacketData and store them in the Vector3 object as the X,Y and Z values
        Vector3Value.x = ReadFloat();
        Vector3Value.y = ReadFloat();
        Vector3Value.z = ReadFloat();
        //Return the final Vector3 object that was requested
        return Vector3Value;
    }

    //Writes the 4 floating point values of a Quaternion onto the end of the current PacketData
    public void WriteQuaternion(Quaternion QuaternionValue)
    {
        WriteFloat(QuaternionValue.x);
        WriteFloat(QuaternionValue.y);
        WriteFloat(QuaternionValue.z);
        WriteFloat(QuaternionValue.w);
    }
    //Reads 4 floating point values from the front of the RemainingPacketData, then removes them from that string, returning them in a Quaternion format
    public Quaternion ReadQuaternion()
    {
        //Create a new Quaternion object to store all the values within
        Quaternion QuaternionValue = new Quaternion();
        //Read the next 4 floating point values from the RemainingPacketData and store them in the Quaternion object as the X,Y,Z and W values
        QuaternionValue.x = ReadFloat();
        QuaternionValue.y = ReadFloat();
        QuaternionValue.z = ReadFloat();
        QuaternionValue.w = ReadFloat();
        //Return the final quaternion value that was requested
        return QuaternionValue;
    }
}
