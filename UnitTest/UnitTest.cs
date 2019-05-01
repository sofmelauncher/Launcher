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
    

        }

        [TestMethod]
        public void PathSplitTest(){
            var bin_path = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            var path = bin_path + "\\1\\game\\DOUTONBORI.exe";
            var directory_index = path.LastIndexOf("\\");
            var final_path = path.Substring(0, directory_index);
            Console.WriteLine(final_path);
        }

        [TestMethod]
        public void LoggerTest()
        {
            Logger.Inst.Log("test");
        }
    }
}
