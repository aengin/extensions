﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Signum.Web.Selenium;
using Selenium;
using System.Diagnostics;
using Signum.Engine;
using Signum.Entities.Authorization;
using Signum.Test;
using Signum.Web.Extensions.Sample.Test.Properties;
using Signum.Engine.Maps;
using Signum.Engine.Authorization;

namespace Signum.Web.Extensions.Sample.Test
{
    [TestClass]
    public class LinesTests : Common
    {
        public LinesTests() : base()
        {

        }

        [ClassInitialize()]
        public static void LaunchSelenium(TestContext testContext)
        {
            Common.LaunchSelenium(testContext);
        }

        [ClassCleanup]
        public static void MyTestCleanup()
        {
            Common.MyTestCleanup();
        }

        [TestMethod]
        public void EntityLine()
        {
            CheckLoginAndOpen("/Signum.Web.Extensions.Sample/View/Band/1");
            
            //view
            selenium.Click("LastAward_btnView");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#LastAward_sfEntity:visible"));

            //cancel 
            selenium.Click("LastAward_sfBtnCancel");
            Assert.IsFalse(selenium.IsElementPresent("jq=#LastAward_sfEntity:visible"));

            //delete
            selenium.Click("LastAward_btnRemove");
            Assert.IsTrue(selenium.IsElementPresent("jq=#LastAward_sfToStr:visible"));
            Assert.IsFalse(selenium.IsElementPresent("jq=#LastAward_sfLink:visible"));

            //create with implementations
            selenium.Click("LastAward_btnCreate");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #LastAwardTemp"));
            selenium.Click("GrammyAwardDN");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#LastAward_Category"));
            selenium.Type("LastAward_Category", "prueba");
            selenium.Click("LastAward_sfBtnOk");
            selenium.WaitAjaxFinished(() => !selenium.IsElementPresent("jq=#divASustituir + #LastAwardTemp"));
            Assert.IsFalse(selenium.IsElementPresent("jq=#LastAward_sfToStr:visible"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#LastAward_sfLink:visible"));

            //find with implementations
            selenium.Click("LastAward_btnRemove");
            selenium.Click("LastAward_btnFind");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #LastAwardTemp"));
            selenium.Click("GrammyAwardDN");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#LastAward_btnSearch"));
            selenium.Click("LastAward_btnSearch");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#LastAward_tblResults > tbody > tr"));
            selenium.Click("LastAward_rowSelection");
            selenium.Click("LastAward_sfBtnOk");
            selenium.WaitAjaxFinished(() => !selenium.IsElementPresent("jq=#LastAward_sfToStr:visible"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#LastAward_sfLink:visible"));
        }

        [TestMethod]
        public void EntityLineDetail()
        {
            CheckLoginAndOpen("/Signum.Web.Extensions.Sample/Music/BandDetail");

            //Value is opened by default
            Assert.IsTrue(selenium.IsElementPresent("jq=#LastAward_sfDetail #LastAward_Category"));
            
            //Delete
            selenium.Click("LastAward_btnRemove");
            Assert.IsFalse(selenium.IsElementPresent("jq=#LastAward_sfDetail #LastAward_Category"));

            //create with implementations
            selenium.Click("LastAward_btnCreate");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #LastAwardTemp"));
            selenium.Click("AmericanMusicAwardDN");
            selenium.WaitAjaxFinished(() => !selenium.IsElementPresent("jq=#divASustituir + #LastAwardTemp"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#LastAward_sfDetail #LastAward_Category"));
            selenium.Type("LastAward_Category", "prueba");

            //find with implementations
            selenium.Click("LastAward_btnRemove");
            selenium.Click("LastAward_btnFind");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #LastAwardTemp"));
            selenium.Click("AmericanMusicAwardDN");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#LastAward_btnSearch"));
            selenium.Click("LastAward_btnSearch");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#LastAward_tblResults > tbody > tr"));
            selenium.Click("LastAward_rowSelection");
            selenium.Click("LastAward_sfBtnOk");
            selenium.WaitAjaxFinished(() => !selenium.IsElementPresent("jq=#divASustituir + #LastAwardTemp"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#LastAward_sfDetail #LastAward_Category"));
        }

        [TestMethod]
        public void EntityLineInPopup()
        {
            CheckLoginAndOpen("/Signum.Web.Extensions.Sample/View/Album/1");
            
            //open popup
            selenium.Click("Author_btnView");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#Author_sfEntity:visible"));

            //view
            selenium.Click("Author_LastAward_btnView");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#Author_LastAward_sfEntity:visible"));
            selenium.Click("Author_LastAward_sfBtnOk");
            selenium.WaitAjaxFinished(() => !selenium.IsElementPresent("jq=#Author_LastAward_sfEntity:visible"));

            //delete
            selenium.Click("Author_LastAward_btnRemove");
            Assert.IsTrue(selenium.IsElementPresent("jq=#Author_LastAward_sfToStr:visible"));
            Assert.IsFalse(selenium.IsElementPresent("jq=#Author_LastAward_sfLink:visible"));
            
            //create with implementations
            selenium.Click("Author_LastAward_btnCreate");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #Author_LastAwardTemp"));
            selenium.Click("AmericanMusicAwardDN");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#Author_LastAward_Category"));
            selenium.Type("Author_LastAward_Category", "prueba");
            selenium.Click("Author_LastAward_sfBtnOk");
            selenium.WaitAjaxFinished(() => !selenium.IsElementPresent("jq=#divASustituir + #Author_LastAwardTemp"));
            Assert.IsFalse(selenium.IsElementPresent("jq=#Author_LastAward_sfToStr:visible"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Author_LastAward_sfLink:visible"));
            
            //find with implementations
            selenium.Click("Author_LastAward_btnRemove");
            selenium.Click("Author_LastAward_btnFind");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #Author_LastAwardTemp"));
            selenium.Click("AmericanMusicAwardDN");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#Author_LastAward_btnSearch"));
            selenium.Click("Author_LastAward_btnSearch");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#Author_LastAward_tblResults > tbody > tr"));
            selenium.Click("Author_LastAward_rowSelection");
            selenium.Click("Author_LastAward_sfBtnOk");
            selenium.WaitAjaxFinished(() => !selenium.IsElementPresent("jq=#divASustituir + #Author_LastAwardTemp"));
            Assert.IsFalse(selenium.IsElementPresent("jq=#Author_LastAward_sfToStr:visible"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Author_LastAward_sfLink:visible"));
        }

        [TestMethod]
        public void EntityList()
        {
            CheckLoginAndOpen("/Signum.Web.Extensions.Sample/View/Band/1");

            //Create and cancel
            selenium.Click("Members_btnCreate");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #Members_4Temp"));
            selenium.Click("Members_4_sfBtnCancel");
            Assert.IsFalse(selenium.IsElementPresent("jq=#divASustituir + #Members_4Temp"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members > option:nth-child(4)"));
            Assert.IsFalse(selenium.IsElementPresent("jq=#Members > option:nth-child(5)"));
            Assert.IsFalse(selenium.IsElementPresent("jq=#Members_4_sfRuntimeInfo"));

            //Create and ok
            selenium.Click("Members_btnCreate");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #Members_4Temp"));
            selenium.Type("Members_4_Name", "prueba");
            selenium.Click("Members_4_sfBtnOk");
            selenium.WaitAjaxFinished(() => !selenium.IsElementPresent("jq=#divASustituir + #Members_4Temp"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_4_sfEntity"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_4_sfRuntimeInfo"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members > option:nth-child(5)"));

            //Delete
            selenium.Click("Members_btnRemove");
            Assert.IsFalse(selenium.IsElementPresent("jq=#Members_4_sfRuntimeInfo"));
            Assert.IsFalse(selenium.IsElementPresent("jq=#Members > option:nth-child(5)"));

            //Find multiple
            selenium.Click("Members_btnFind");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #Members_4Temp"));
            selenium.Click("Members_4_btnSearch");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#Members_4_tblResults > tbody > tr"));
            selenium.Click("Members_4_rowSelection_4");
            selenium.Click("Members_4_rowSelection_5");
            selenium.Click("Members_4_sfBtnOk");
            selenium.WaitAjaxFinished(() => !selenium.IsElementPresent("jq=#divASustituir + #Members_4Temp"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_4_sfRuntimeInfo"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members > option:nth-child(5)"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_5_sfRuntimeInfo"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members > option:nth-child(6)"));

            //Create with implementations
            selenium.Click("OtherAwards_btnCreate");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #OtherAwardsTemp"));
            selenium.Click("GrammyAwardDN");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #OtherAwards_0Temp"));
            Assert.IsFalse(selenium.IsElementPresent("jq=#divASustituir + #OtherAwards_Temp"));
            selenium.Type("OtherAwards_0_Category", "prueba");
            selenium.Click("OtherAwards_0_sfBtnOk");
            selenium.WaitAjaxFinished(() => !selenium.IsElementPresent("jq=#OtherAwards_0Temp"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#OtherAwards_0_sfEntity"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#OtherAwards_0_sfRuntimeInfo"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#OtherAwards > option:nth-child(1)"));

            //find with implementations
            selenium.Click("OtherAwards_btnFind");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #OtherAwardsTemp"));
            selenium.Click("GrammyAwardDN");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #OtherAwards_1Temp"));
            Assert.IsFalse(selenium.IsElementPresent("jq=#divASustituir + #OtherAwards_Temp"));
            selenium.Click("OtherAwards_1_btnSearch");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#OtherAwards_1_tblResults > tbody > tr"));
            selenium.Click("OtherAwards_1_rowSelection_0");
            selenium.Click("OtherAwards_1_sfBtnOk");
            selenium.WaitAjaxFinished(() => !selenium.IsElementPresent("jq=#divASustituir + #OtherAwards_1Temp"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#OtherAwards_1_sfRuntimeInfo"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#OtherAwards > option:nth-child(2)"));

            //Delete
            selenium.Click("OtherAwards_btnRemove");
            Assert.IsFalse(selenium.IsElementPresent("jq=#OtherAwards_1_sfRuntimeInfo"));
            Assert.IsFalse(selenium.IsElementPresent("jq=#OtherAwards > option:nth-child(2)"));

            //View
            selenium.Select("jq=#OtherAwards", "id=OtherAwards_0_sfToStr");
            selenium.DoubleClick("jq=#OtherAwards > option:nth-child(1)");
            Assert.IsTrue(selenium.IsElementPresent("jq=#OtherAwards_0_sfEntity:visible"));
            selenium.Click("jq=#OtherAwards_0_sfBtnCancel");
            Assert.IsFalse(selenium.IsElementPresent("jq=#OtherAwards_0_sfEntity:visible"));
        }

        [TestMethod]
        public void EntityListInPopup()
        {
            CheckLoginAndOpen("/Signum.Web.Extensions.Sample/View/Band/1");

            //open popup
            selenium.Select("jq=#Members", "id=Members_0_sfToStr");
            selenium.DoubleClick("jq=#Members_0_sfToStr");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#Members_0_panelPopup:visible"));
            
            //create
            selenium.Click("Members_0_Friends_btnCreate");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #Members_0_Friends_1Temp"));

            selenium.Type("Members_0_Friends_1_Name", "prueba");
            selenium.Click("Members_0_Friends_1_sfBtnOk");
            Assert.IsFalse(selenium.IsElementPresent("jq=#divASustituir + #Members_0_Friends_1Temp"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_0_Friends > option:nth-child(1)"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_0_Friends_1_sfRuntimeInfo"));

            //find multiple
            selenium.Click("Members_0_Friends_btnFind");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #Members_0_Friends_2Temp"));
            selenium.Click("Members_0_Friends_2_btnSearch");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#Members_0_Friends_2_tblResults > tbody > tr"));
            selenium.Click("Members_0_Friends_2_rowSelection_4");
            selenium.Click("Members_0_Friends_2_rowSelection_5");
            selenium.Click("Members_0_Friends_2_sfBtnOk");

            //delete multiple
            selenium.Select("Members_0_Friends", "id=Members_0_Friends_1_sfToStr");
            selenium.AddSelection("Members_0_Friends", "id=Members_0_Friends_2_sfToStr");
            selenium.Click("Members_0_Friends_btnRemove");
            selenium.Click("Members_0_Friends_btnRemove");
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_0_Friends > option:nth-child(2)"));
            Assert.IsFalse(selenium.IsElementPresent("jq=#Members_0_Friends > option:nth-child(3)"));
        }

        [TestMethod]
        public void EntityListDetail()
        {
            CheckLoginAndOpen("/Signum.Web.Extensions.Sample/Music/BandDetail");

            //1st element is shown by default
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_sfDetail #Members_0_Name"));
            
            //create
            selenium.Click("Members_btnCreate");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#Members_sfDetail #Members_4_Name"));
            selenium.Type("Members_4_Name", "prueba");
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_4_sfRuntimeInfo"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members > option:nth-child(5)"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_sfDetail #Members_4_Name"));

            //delete
            selenium.Click("Members_btnRemove");
            Assert.IsFalse(selenium.IsElementPresent("jq=#Members_4_sfRuntimeInfo"));
            Assert.IsFalse(selenium.IsElementPresent("jq=#Members > option:nth-child(5)"));
            Assert.IsFalse(selenium.IsElementPresent("jq=#Members_sfDetail #Members_4_Name"));

            //find multiple
            selenium.Click("Members_btnFind");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #Members_4Temp"));
            selenium.Click("Members_4_btnSearch");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#Members_4_tblResults > tbody > tr"));
            selenium.Click("Members_4_rowSelection_4");
            selenium.Click("Members_4_rowSelection_5");
            selenium.Click("Members_4_sfBtnOk");
            selenium.WaitAjaxFinished(() => !selenium.IsElementPresent("jq=#divASustituir + #Members_4Temp"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_4_sfRuntimeInfo"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members > option:nth-child(5)"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_5_sfRuntimeInfo"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members > option:nth-child(6)"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_sfDetail #Members_5_Name"));

            //create with implementations
            selenium.Click("OtherAwards_btnCreate");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #OtherAwardsTemp"));
            selenium.Click("GrammyAwardDN");
            selenium.WaitAjaxFinished(() => !selenium.IsElementPresent("jq=#divASustituir + #OtherAwards_Temp"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#OtherAwards_sfDetail #OtherAwards_0_Category"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#OtherAwards_0_sfRuntimeInfo"));
            selenium.Type("OtherAwards_0_Category", "prueba");

            //find with implementations
            selenium.Click("OtherAwards_btnFind");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #OtherAwardsTemp"));
            selenium.Click("GrammyAwardDN");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #OtherAwards_1Temp"));
            Assert.IsFalse(selenium.IsElementPresent("jq=#divASustituir + #OtherAwards_Temp"));
            selenium.Click("OtherAwards_1_btnSearch");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#OtherAwards_1_tblResults > tbody > tr"));
            selenium.Click("OtherAwards_1_rowSelection_0");
            selenium.Click("OtherAwards_1_sfBtnOk");
            selenium.WaitAjaxFinished(() => !selenium.IsElementPresent("jq=#divASustituir + #OtherAwards_1Temp"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#OtherAwards_1_sfRuntimeInfo"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#OtherAwards > option:nth-child(2)"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#OtherAwards_sfDetail #OtherAwards_1_Category"));

            //Delete
            selenium.Click("OtherAwards_btnRemove");
            Assert.IsFalse(selenium.IsElementPresent("jq=#OtherAwards_1_sfRuntimeInfo"));
            Assert.IsFalse(selenium.IsElementPresent("jq=#OtherAwards > option:nth-child(2)"));

            //View detail
            selenium.Select("jq=#OtherAwards", "id=OtherAwards_0_sfToStr");
            selenium.DoubleClick("jq=#OtherAwards > option:nth-child(1)");
            Assert.IsTrue(selenium.IsElementPresent("jq=#OtherAwards_sfDetail #OtherAwards_0_Category"));
        }

        [TestMethod]
        public void EntityRepeater()
        {
            CheckLoginAndOpen("/Signum.Web.Extensions.Sample/Music/BandRepeater");

            //All elements are shown
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_sfItemsContainer > div.repeaterElement:nth-child(4)"));
            
            //Create
            selenium.Click("Members_btnCreate");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#Members_sfItemsContainer > #Members_4_sfRepeaterItem"));
            selenium.Type("Members_4_Name", "prueba");
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_4_sfRuntimeInfo"));

            //delete
            selenium.Click("Members_4_btnRemove");
            Assert.IsFalse(selenium.IsElementPresent("jq=#Members_4_sfRuntimeInfo"));
            Assert.IsFalse(selenium.IsElementPresent("jq=#Members_sfItemsContainer > #Members_4_sfRepeaterItem"));

            //find multiple
            selenium.Click("Members_btnFind");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #Members_4Temp"));
            selenium.Click("Members_4_btnSearch");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#Members_4_tblResults > tbody > tr"));
            selenium.Click("Members_4_rowSelection_4");
            selenium.Click("Members_4_rowSelection_5");
            selenium.Click("Members_4_sfBtnOk");
            selenium.WaitAjaxFinished(() => !selenium.IsElementPresent("jq=#divASustituir + #Members_4Temp"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_4_sfRuntimeInfo"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_sfItemsContainer > #Members_4_sfRepeaterItem"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_5_sfRuntimeInfo"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#Members_sfItemsContainer > #Members_5_sfRepeaterItem"));

            //create with implementations
            selenium.Click("OtherAwards_btnCreate");
            selenium.WaitAjaxFinished(() => selenium.IsElementPresent("jq=#divASustituir + #OtherAwardsTemp"));
            selenium.Click("GrammyAwardDN");
            selenium.WaitAjaxFinished(() => !selenium.IsElementPresent("jq=#divASustituir + #OtherAwards_Temp"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#OtherAwards_sfItemsContainer > #OtherAwards_0_sfRepeaterItem"));
            Assert.IsTrue(selenium.IsElementPresent("jq=#OtherAwards_0_sfRuntimeInfo"));
            selenium.Type("OtherAwards_0_Category", "prueba");
            
            //find with implementations
            selenium.Click("OtherAwards_btnFind");
        }
    }
}