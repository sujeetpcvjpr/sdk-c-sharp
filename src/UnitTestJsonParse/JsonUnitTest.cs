using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaddleCheckoutSDK;
using Newtonsoft.Json;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace UnitTestJsonParse
{
    [TestClass]
    public class UnitTestJson
    {
      
        [TestMethod]
        public void EventsTest()
        {
            int BeginEvert = 0;
            int CompleteEvent = 0;
            int CloseEvent = 0;

            CheckoutControl coc = new CheckoutControl("22167", "1234567", "ClivesApp");

            coc.TransactionBeginEvent += (o, e) => { BeginEvert = 1; };
            coc.CheckoutClosed += (o, e) => { CloseEvent = 1; };
            coc.TransactionCompleteEvent += (o, e) => { CompleteEvent = 1; };

            coc.FireCheckoutCloseEvent();
            coc.FireTranactionCompleteEvent(new TransactionCompleteEventArgs());
            coc.FireTransactionBeginEvent();

            Assert.AreEqual(BeginEvert, 1);
            Assert.AreEqual(CompleteEvent, 1);
            Assert.AreEqual(CloseEvent, 1);

        }

        public void EventsTestParent()
        {
            int BeginEvert = 0;
            int CompleteEvent = 0;
            int CloseEvent = 0;

            CheckoutControl coc = new CheckoutControl("22167", "1234567", "ClivesApp");



            coc.TransactionBeginEvent += (o, e) => { BeginEvert = 1; };
            coc.CheckoutClosed += (o, e) => { CloseEvent = 1; };
            coc.TransactionCompleteEvent += (o, e) => { CompleteEvent = 1; };

            coc.FireCheckoutCloseEvent();
            coc.FireTranactionCompleteEvent(new TransactionCompleteEventArgs());
            coc.FireTransactionBeginEvent();

            Assert.AreEqual(BeginEvert, 1);
            Assert.AreEqual(CompleteEvent, 1);
            Assert.AreEqual(CloseEvent, 1);
        }


        [TestMethod]
        public void TestWellFormedJson()
        {
            string json = "{ \"state\":\"processed\",\"checkout\":{ \"checkout_id\":\"11079313-chre4e360a97da7-fe02d82fe7\",\"image_url\":\"\",\"title\":\"ClivesApp1\"},\"order\":{ \"order_id\":2221626,\"total\":\"0.00\",\"currency\":\"USD\",\"formatted_total\":\"US$0.00\",\"completed\":{ \"date\":\"2017-11-09 10:25:53.000000\",\"timezone_type\":3,\"timezone\":\"UTC\"},\"receipt_url\":\"\",\"has_locker\":true,\"customer\":{ \"email\":\"clive.collie@paddle.com\"},\"is_subscription\":false},\"lockers\":[{\"locker_id\":2178001,\"product_id\":519714,\"product_name\":\"ClivesApp1\",\"license_code\":\"C64235AD-7ACC3BBF-39275213-B41FEE3B-32BCED11\",\"download\":\"http://www.paddle.com\",\"instructions\":\"\"}]}";

            ProcessStatus ps = new ProcessStatus();
            ProcessStatus processStatus = ps.MapResponse(json);

            string OrderID = ps.OrderID;
            string CustomerEmail = ps.CustomerEmail;
            string LicenseCode = ps.LicenseCode;
            string LockerID = ps.LockerID;
            Assert.AreEqual(OrderID, "2221626");
            Assert.AreEqual(CustomerEmail, "clive.collie@paddle.com");
            Assert.AreEqual(LicenseCode, "C64235AD-7ACC3BBF-39275213-B41FEE3B-32BCED11");
            Assert.AreEqual(LockerID, "2178001");
        }

        [TestMethod]
        public void TestWellNoLockerJson()
        {
            string json = "{ \"state\":\"processed\",\"checkout\":{ \"checkout_id\":\"11079313-chre4e360a97da7-fe02d82fe7\",\"image_url\":\"\",\"title\":\"ClivesApp1\"},\"order\":{ \"order_id\":2221626,\"total\":\"0.00\",\"currency\":\"USD\",\"formatted_total\":\"US$0.00\",\"completed\":{ \"date\":\"2017-11-09 10:25:53.000000\",\"timezone_type\":3,\"timezone\":\"UTC\"},\"receipt_url\":\"\",\"has_locker\":false,\"customer\":{ \"email\":\"clive.collie@paddle.com\"},\"is_subscription\":false},\"lockers\":[]}";

            ProcessStatus ps = new ProcessStatus();
            ProcessStatus processStatus = ps.MapResponse(json);

            string OrderID = ps.OrderID;
            string CustomerEmail = ps.CustomerEmail;
            string LicenseCode = ps.LicenseCode;
            string LockerID = ps.LockerID;
            Assert.AreEqual(OrderID, "2221626");
            Assert.AreEqual(CustomerEmail, "clive.collie@paddle.com");
            Assert.IsNull(LicenseCode);
            Assert.IsNull(LockerID);

        }

        /// <summary>
        /// Fails because JSON does not match expected format test passed because exception of expected type
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(JsonReaderException))]
        public void TestWellNoOrderJson()
        {

            string json = "{ \"state\":\"processed\",\"checkout\":{ \"checkout_id\":\"11079313-chre4e360a97da7-fe02d82fe7\",\"image_url\":\"\",\"title\":\"ClivesApp1\"},\"order\":{ },\"receipt_url\":\"\",\"has_locker\":false,\"customer\":{ \"email\":\"clive.collie@paddle.com\"},\"is_subscription\":false},\"lockers\":[{\"locker_id\":2178001,\"product_id\":519714,\"product_name\":\"ClivesApp1\",\"license_code\":\"C64235AD-7ACC3BBF-39275213-B41FEE3B-32BCED11\",\"download\":\"http://www.paddle.com\",\"instructions\":\"\"}]}";

            ProcessStatus ps = new ProcessStatus();
            ProcessStatus processStatus = ps.MapResponse(json);

        }

        [TestMethod]
        public void GetLocationInfo()
        {
            string json = "{\"event\":\"Checkout.Location.Submit\",\"checkout\":{\"id\":\"11143159-chre15971211283-3ca3e0ef9f\",\"passthrough\":\"bnVsbA==\"},\"product\":{\"id\":\"519714\",\"name\":\"ClivesApp1\"},\"user\":{\"country\":\"GB\",\"email\":\"clive.collie@paddle.com\",\"id\":3524133}}";
            var cl = ProcessStatus.GetCheckoutLoaded(json);

            Assert.AreEqual(cl.EventType, "Checkout.Location.Submit");
            Assert.AreEqual(cl.User.Email, "clive.collie@paddle.com");
            Assert.AreEqual(cl.User.Country, "GB");

        }

        [TestMethod]
        public void GetLocationInfoNullUser()
        {
            string json = "{\"event\":\"Checkout.Location.Submit\",\"checkout\":{\"id\":\"11143159-chre15971211283-3ca3e0ef9f\",\"passthrough\":\"bnVsbA==\"},\"product\":{\"id\":\"519714\",\"name\":\"ClivesApp1\"},\"user\":null}";
            var cl = ProcessStatus.GetCheckoutLoaded(json);
            Assert.AreEqual(cl.Checkout.ID, "11143159-chre15971211283-3ca3e0ef9f");
            Assert.AreEqual(cl.User.Email, null);
        }

        /// <summary>
        /// Expect a exception throw in order to pass
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestProperties()
        {
            //this will throw because auth not set and price changed
            CheckoutControl coc = new CheckoutControl { ProductID = "prod1", VendorID = "Vend1", AppName = "Appname"  };

            coc.TransactionCompleteEvent += (o, e) => { };
            coc.TransactionErrorEvent += (o, e) => { };

            coc.StartPurchase();
        }
    }
}