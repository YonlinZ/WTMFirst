using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using WTMFirst.Controllers;
using WTMFirst.ViewModel.SchoolVMs;
using WTMFirst.Model;
using WTMFirst.DataAccess;

namespace WTMFirst.Test
{
    [TestClass]
    public class SchoolControllerTest
    {
        private SchoolController _controller;
        private string _seed;

        public SchoolControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<SchoolController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as SchoolListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(SchoolVM));

            SchoolVM vm = rv.Model as SchoolVM;
            School v = new School();
			
            v.SchoolCode = "qpW";
            v.SchoolName = "1cjpZpxcZ";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<School>().FirstOrDefault();
				
                Assert.AreEqual(data.SchoolCode, "qpW");
                Assert.AreEqual(data.SchoolName, "1cjpZpxcZ");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            School v = new School();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.SchoolCode = "qpW";
                v.SchoolName = "1cjpZpxcZ";
                context.Set<School>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(SchoolVM));

            SchoolVM vm = rv.Model as SchoolVM;
            v = new School();
            v.ID = vm.Entity.ID;
       		
            v.SchoolCode = "pPV";
            v.SchoolName = "vZNGJ27";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.SchoolCode", "");
            vm.FC.Add("Entity.SchoolName", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<School>().FirstOrDefault();
 				
                Assert.AreEqual(data.SchoolCode, "pPV");
                Assert.AreEqual(data.SchoolName, "vZNGJ27");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            School v = new School();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.SchoolCode = "qpW";
                v.SchoolName = "1cjpZpxcZ";
                context.Set<School>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(SchoolVM));

            SchoolVM vm = rv.Model as SchoolVM;
            v = new School();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<School>().Count(), 0);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            School v = new School();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.SchoolCode = "qpW";
                v.SchoolName = "1cjpZpxcZ";
                context.Set<School>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            School v1 = new School();
            School v2 = new School();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.SchoolCode = "qpW";
                v1.SchoolName = "1cjpZpxcZ";
                v2.SchoolCode = "pPV";
                v2.SchoolName = "vZNGJ27";
                context.Set<School>().Add(v1);
                context.Set<School>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(SchoolBatchVM));

            SchoolBatchVM vm = rv.Model as SchoolBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<School>().Count(), 0);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as SchoolListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }


    }
}
