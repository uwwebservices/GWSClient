using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using UW.Web.Services.GWSClient.Utils;

namespace UW.Web.Services.GWSClient.Test
{
    [TestClass]
    public class EnumsTests
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        public void Enums_TryParse_True_If_Correct_String()
        {
            var httpok = HttpStatusCode.OK;
            Assert.IsTrue(Enums.TryStrictParse<TestState>(httpok.ToString(), out var state));
            Assert.AreEqual(TestState.ok, state);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void Enums_TryParse_True_If_Correct_Underlying()
        {
            var httpok = (int)HttpStatusCode.OK;
            Assert.IsTrue(Enums.TryStrictParse<TestState>(httpok.ToString(), out var state));
            Assert.AreEqual(TestState.ok, state);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void Enums_TryParse_False_If_Incorrect_String()
        {
            var httpok = HttpStatusCode.BadRequest;
            Assert.IsFalse(Enums.TryStrictParse<TestState>(httpok.ToString(), out _));
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void Enums_TryParse_False_If_Incorrect_Underlying_Value()
        {
            var httpok = (int)HttpStatusCode.BadRequest;
            Assert.IsFalse(Enums.TryStrictParse<TestState>(httpok.ToString(), out _));
        }
    }

    enum TestState
    {
        ok = 200,
        fail = 500
    }
}
