using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using meGaton.DataResources;
using meGaton.Models;
using meGaton.ViewModels;
using meGaton.Views;
using MaterialDesignThemes.Wpf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTest {
    [TestClass]
    public class UnitTest {
        [TestMethod]
        public void BinaryPathTest() {
            Console.WriteLine(System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'));
        }

        [TestMethod]
        public void DBTest(){
            var db=new SQLiteConnector();

        }
    }
}
