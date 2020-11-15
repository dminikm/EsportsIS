using System;
using System.Collections.Generic;
using DatabaseService;
using DatabaseService.Gateway;
using DataTypes;
using LanguageExt;

namespace DatabaseTester
{
    class Program
    {
        static void Main(string[] args)
        {
            EventGateway.CreateTrainingEvent("test1", 0, 0, new List<Participant>() { new Participant() { Type = "player", ParticipantID = 1 } });
            EventGateway.CreateTrainingEvent("test2", 0, 0, new List<Participant>());
            EventGateway.CreateTrainingEvent("test3", 0, 0, new List<Participant>());
            EventGateway.CreateTrainingEvent("test4", 0, 0, new List<Participant>());
            EventGateway.CreateTrainingEvent("test5", 0, 0, new List<Participant>());
            EventGateway.CreateTrainingEvent("test6", 0, 0, new List<Participant>());
        }
    }
}
