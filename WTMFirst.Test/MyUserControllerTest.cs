using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using WTMFirst.Controllers;
using WTMFirst.ViewModel.MyUserVMs;
using WTMFirst.Model;
using WTMFirst.DataAccess;

namespace WTMFirst.Test
{
    [TestClass]
    public class MyUserControllerTest
    {
        private MyUserController _controller;
        private string _seed;

        public MyUserControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<MyUserController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as MyUserListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(MyUserVM));

            MyUserVM vm = rv.Model as MyUserVM;
            MyUser v = new MyUser();
			
            v.ITCode = "cFM";
            v.Password = "epP2nO";
            v.Name = "6mk2";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<MyUser>().FirstOrDefault();
				
                Assert.AreEqual(data.ITCode, "cFM");
                Assert.AreEqual(data.Password, "epP2nO");
                Assert.AreEqual(data.Name, "6mk2");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            MyUser v = new MyUser();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.ITCode = "cFM";
                v.Password = "epP2nO";
                v.Name = "6mk2";
                context.Set<MyUser>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(MyUserVM));

            MyUserVM vm = rv.Model as MyUserVM;
            v = new MyUser();
            v.ID = vm.Entity.ID;
       		
            v.ITCode = "77ZBqX";
            v.Password = "75V";
            v.Name = "NrU";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.ITCode", "");
            vm.FC.Add("Entity.Password", "");
            vm.FC.Add("Entity.Name", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<MyUser>().FirstOrDefault();
 				
                Assert.AreEqual(data.ITCode, "77ZBqX");
                Assert.AreEqual(data.Password, "75V");
                Assert.AreEqual(data.Name, "NrU");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            MyUser v = new MyUser();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.ITCode = "cFM";
                v.Password = "epP2nO";
                v.Name = "6mk2";
                context.Set<MyUser>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(MyUserVM));

            MyUserVM vm = rv.Model as MyUserVM;
            v = new MyUser();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<MyUser>().Count(), 0);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            MyUser v = new MyUser();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.ITCode = "cFM";
                v.Password = "epP2nO";
                v.Name = "6mk2";
                context.Set<MyUser>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            MyUser v1 = new MyUser();
            MyUser v2 = new MyUser();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.ITCode = "cFM";
                v1.Password = "epP2nO";
                v1.Name = "6mk2";
                v2.ITCode = "77ZBqX";
                v2.Password = "75V";
                v2.Name = "NrU";
                context.Set<MyUser>().Add(v1);
                context.Set<MyUser>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(MyUserBatchVM));

            MyUserBatchVM vm = rv.Model as MyUserBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<MyUser>().Count(), 0);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as MyUserListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }


    }
}
