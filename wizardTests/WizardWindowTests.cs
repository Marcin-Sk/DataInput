using Microsoft.VisualStudio.TestTools.UnitTesting;
using wizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wizard.Tests
{
    [TestClass()]
    public class WizardWindowTests
    {
        [TestMethod()]
        public void firstNameTextBox_TextChangedTest()
        {
            PrivateType privateType = new PrivateType(typeof(WizardWindow));

            Type[] parameterTypes =
            {
                typeof(object),
                typeof(object)
            };

            object[] parameterValues =
            {
                "sfad",
                "adfasdf"
            };

            //object sender = "John";
            //Page1.CanSelectNextPage expected = true;
           
            //Assert.Fail("Verify the correctness!");
        }

    }
}