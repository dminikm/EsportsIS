using System;
using System.Collections.Generic;
using BusinessLayer;
using DatabaseService.Gateway;

namespace DatabaseTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var evt = EventGateway.CreateTrainingEvent("test", 0, 10, new List<int>());
            var evt2 = EventGateway.CreateCustomEvent("test 2", 0, 0, 2, "#000000", new List<int>());
            evt2.Description = "Test 2, but different!";

            EventGateway.Update(evt2);

            var evt2_c = EventGateway.Find(evt2.EventID.IfNone(() => 1));

            EventGateway.Delete(evt2);
        }
    }
}
