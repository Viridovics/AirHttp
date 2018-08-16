using System;

namespace AirHttp.Configuration
{
    public class DefaultHttpClientParameters : IHttpClientParameters
    {
        public int TimeoutInMilliseconds 
        { 
            get
            {
                return 100000;
            } 
        }

        public bool SaveCookie 
        {
            get
            {
                return true;
            }
        }
    }
}