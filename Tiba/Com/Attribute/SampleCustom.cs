using System;
using System.Collections.Generic;
using System.Text;

namespace NEETLibrary.Tiba.Com.Attribute
{
    [System.AttributeUsage(System.AttributeTargets.Class |
                            System.AttributeTargets.Method |
                            System.AttributeTargets.Property |
                           System.AttributeTargets.Struct)
    ]
    public class AuthorAttribute : System.Attribute
    {
        private string name;
        public double version;

        public AuthorAttribute(string name)
        {
            this.name = name;
            version = 1.0;
        }
    }


    public class SumpleTest
    {
        [Author("P. Ackerman", version = 1.1)]
        public void TestMethod (){
        
        }
    
    }

}
