using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TapiLib
{
    public class CommonCalls
    {
        [DllImport("Tapi32.dll", SetLastError = true)]
        public static extern long tapiRequestMakeCall(string lpszDestAddress, string lpszAppName, string lpszCalledParty, string lpszComment);


        public static void MakeDialerCall(string PhoneNumber, String AppName, string CalledParty, string Comment)
        {
            if (PhoneNumber == null)
            {
                throw new Exception("Phonenumber cannot be null");
            }

            try
            {
                tapiRequestMakeCall(PhoneNumber, AppName, CalledParty, Comment);

            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
