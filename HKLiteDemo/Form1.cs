using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HKLite;
using HKLiteDemo.Entity;

namespace HKLiteDemo
{
    public partial class Form1 : Form
    {
        // 操作模块
        private IDao<SysUser> daoUser;
        private IDao<SysRole> daoRole;
        private IDao<SysUserRole> daoUserRole;

        private List<SysUser> listSource = new List<SysUser>();
        private List<SysUser> listAllUser = new List<SysUser>();

        // 用户名，用于插入测试
        private List<string> listName = new List<string> { "Tom", "Jack", "Lucy", "Lina", "Linda", "Jackson", "Mike", "Alice", "Robin", "Nancy" };

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                bindingSource1.DataSource = listSource;
                dataGridView1.DataSource = bindingSource1;
                bindingSource1.ResetBindings(true);

                // 初始化
                DBAccess.Init();
                daoUser = DBAccess.DAL.Dao<SysUser>();
                daoRole = DBAccess.DAL.Dao<SysRole>();
                daoUserRole = DBAccess.DAL.Dao<SysUserRole>();

                BindUser(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }

        #region BindDatas
        // 刷新数据
        private void BindUser(bool reQuery)
        {
            if (reQuery)
            {
                listAllUser = daoUser.QueryAll();
                if (listAllUser == null)
                    listAllUser = new List<SysUser>();
            }
            listSource.Clear();
            if (listAllUser.Count > 0)
                listSource.AddRange(listAllUser);
            bindingSource1.ResetBindings(false);
        }
        private void BindUser(List<SysUser> list)
        {
            listSource.Clear();
            if (list.Count > 0)
                listSource.AddRange(list);
            bindingSource1.ResetBindings(false);
        }

        #endregion

        #region Insert
        // 插入SysRole
        private void CreateRoles()
        {
            var listRole = daoRole.QueryAll();
            if (listRole == null || listRole.Count == 0)
            {
                daoRole.Insert(new SysRole { RoleName = "administrators", Description = "管理员", ModifiedTime = DateTime.Now });
                daoRole.Insert(new SysRole { RoleName = "normal", Description = "普通用户", ModifiedTime = DateTime.Now });
            }
        }

        // 插入SysUserRole
        private void CreateUserRoles()
        {
            var listRole = daoRole.QueryAll();
            var listUserRole = daoUserRole.QueryAll();
            if (listAllUser == null || listAllUser.Count == 0)
                return;
            if (listRole == null || listRole.Count == 0)
                return;
            if (listUserRole == null)
                listUserRole = new List<SysUserRole>();
            foreach (var item in listAllUser)
            {
                if (listUserRole.FindIndex(o => o.UserID == item.ID) < 0)
                {
                    // Tom和Jack 为管理员
                    int roleID = (item.UserName == "Tom" || item.UserName == "Jack") ? listRole[0].ID : listRole[1].ID;
                    daoUserRole.Insert(new SysUserRole { UserID = item.ID, RoleID = roleID, ModifiedTime = DateTime.Now });
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // 插入SysUser          
            if (listAllUser.Count >= listName.Count)
            {
                MessageBox.Show("用户名已被用完，请删除后再试！");
                return;
            }
            string name = listName.Find(o => listAllUser.FindIndex(u => u.UserName == o) < 0);
            daoUser.Insert(new SysUser { UserName = name, CName = name, Email = name + "@gmail.com", ModifiedTime = DateTime.Now });

            BindUser(true);
        }

        #endregion

        #region Update
        private void btnUpdateByKey_Click(object sender, EventArgs e)
        {
            // 通过主键更新
            if (listSource.Count > 0)
            {
                var entity = listSource[0];
                entity.CName = entity.UserName + "'s new name";
                daoUser.UpdateByKey(entity);

                BindUser(true);
            }
        }

        private void btnUpdateByCondition_Click(object sender, EventArgs e)
        {
            // 通过其他条件更新
            var updateBuilder = daoUser.UpdateBuilder();
            updateBuilder.Update().Set("CName", "Jack'new name").Where().Equal("UserName", "Jack");
            updateBuilder.Run();

            BindUser(true);
        }
        #endregion

        #region Query
        private void btnQueryByKey_Click(object sender, EventArgs e)
        {
            // 通过主键获取一个实体
            int userID = 1;
            if (listAllUser.Count > 0)
                userID = listAllUser[0].ID;
            var entity = daoUser.QueryByKey(userID);

            List<SysUser> listUser = new List<SysUser>();
            if (entity != null)
                listUser.Add(entity);

            BindUser(listUser);
        }

        private void btnQueryByCondition_Click(object sender, EventArgs e)
        {
            // 通过其他条件查询并排序
            var queryBuilder = daoUser.QueryBuilder();
            queryBuilder.Select().Where().Equal("UserName", "Jack").OrderByDesc("UserName");
            var listUser = queryBuilder.Query();

            BindUser(listUser);
        }

        private void btnQueryTop_Click(object sender, EventArgs e)
        {
            // 获取前2条记录
            var queryBuilder = daoUser.QueryBuilder();
            queryBuilder.Select(2).Where().Like("CName", "J");
            var listUser = queryBuilder.Query();

            BindUser(listUser);
        }

        private void btnQueryColumns_Click(object sender, EventArgs e)
        {
            // 查询指定列
            var queryBuilder = daoUser.QueryBuilder();
            queryBuilder.Select("ID","UserName","Email").Where().Like("CName", "J");
            var listUser = queryBuilder.Query();

            BindUser(listUser);
        }

        private void btnQueryCustomerCondition_Click(object sender, EventArgs e)
        {
            // 通过自定义条件查询
            var queryBuilder = daoUser.QueryBuilder();
            queryBuilder.Select().Where().Custom("where UserName <> 'Tom'");
            var listUser = queryBuilder.Query();

            BindUser(listUser);
        }

        private void btnQuerySql_Click(object sender, EventArgs e)
        {
            // 先创建数据，以便有数据可查
            CreateRoles();
            CreateUserRoles();

            // 查询所有角色为"administrators"的用户，该查询中只要返回结果的列名与实体类SysUser属性名匹配，即可将数据放在泛型集合List中
            var queryBuilder = daoUser.QueryBuilder();
            queryBuilder.CustomSqlCommand =
                @"select A.* from SysUser A join SysUserRole B on A.ID=B.UserID 
                                            join SysRole C on C.ID=B.RoleID
                                            where C.RoleName='administrators'";
            var listUser = queryBuilder.Query();

            BindUser(listUser);
        }
        #endregion

        #region Delete
        private void btnDeleteByKey_Click(object sender, EventArgs e)
        {
            // 通过主键删除
            if (listSource.Count > 0)
            {
                daoUser.DeleteByKey(listSource[0].ID);

                BindUser(true);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 通过其他条件删除
            if (listSource.Count > 0)
            {
                var deleteBuilder = daoUser.DeleteBuilder();
                deleteBuilder.Delete().Where().Equal("UserName", listSource[listSource.Count - 1].UserName);
                deleteBuilder.Run();

                BindUser(true);
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            // 删除所有数据
            daoUser.DeleteAll();
            daoRole.DeleteAll();
            daoUserRole.DeleteAll();

            BindUser(true);
        }
        #endregion

        #region Transtion
        private void btnTrans1_Click(object sender, EventArgs e)
        {
            // 事务操作1
            var trans = DBAccess.DAL.TransacBuilder();
            var insertBuilder = trans.InsertBuilder<SysUser>();
            var updateBuilder = trans.UpdateBuilder<SysUser>();
            var deleteBuilder = trans.DeleteBuilder<SysUser>();

            if (listAllUser.Count >= listName.Count)
                deleteBuilder.Delete().Where().Equal("ID", listAllUser[listAllUser.Count - 1].ID);
            else
            {
                string name = listName.Find(o => listAllUser.FindIndex(u => u.UserName == o) < 0);
                insertBuilder.Insert(new SysUser { UserName = name, CName = name, Email = name + "@gmail.com", ModifiedTime = DateTime.Now });
            }
            updateBuilder.Update().Set("CName", "Lucy's new name").Where().Equal("UserName", "Lucy");
            trans.Run();

            BindUser(true);
        }

        private void btnTrans2_Click(object sender, EventArgs e)
        {
            // 事务操作2
            var trans = DBAccess.DAL.GetTransaction();
            try
            {
                DBAccess.DAL.ExecuteNonQuery(trans, "update SysUser set CName='Lina''s new name' where UserName='Lina'");
                DBAccess.DAL.ExecuteNonQuery(trans, "update SysUser set CName='Linda''s new name' where UserName='Linda'");
                trans.Commit();

                BindUser(true);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        private void btnTrans3_Click(object sender, EventArgs e)
        {
            // 事务操作3
            DBAccess.DAL.ExecuteTransac(
                "update SysUser set CName='Jackson''s new name' where UserName='Jackson'",
                "update SysUser set CName='Mike''s new name' where UserName='Mike'"
                );

            BindUser(true);
        }

        #endregion

     




    }
}
