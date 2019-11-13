# HKLite
* A simple ORM for .net, support Sqlite,Sqlce,Mysql and SQL Server.<br/>
* Can use in both windows and windows ce applications while using Sqlite or Sqlce.
* Auto create if the database file does not exist while using Sqlite or Sqlce.
* Written in VS2008.
## Create entity class
```C#
[HKLite.Entity]
public class SysUser
{
    [HKLite.Entity(PrimaryKey = true, Identity = true)]
    public int ID { get; set; }

    [HKLite.Entity(Length = "20")]
    public string UserName { get; set; }

    [HKLite.Entity(Length = "50")]
    public string Email { get; set; }

    [HKLite.Entity(Length = "50")]
    public string CName { get; set; }

    public DateTime ModifiedTime { get; set; }
}
```
## Init HKLite
```C#
// database version
private static int dbVersion = 1;
private static IDALayer dal;

/// <summary>
/// Init database
/// </summary>
/// <returns></returns>
public static bool Init()
{
    string dir = AppDomain.CurrentDomain.BaseDirectory;
    string dbPath = string.Format(@"{0}\db\mydb.db", dir);
    dal = DALayerBuilder.CreateLayer(dbPath, null, "HKLiteDemo.Entity.DLL", dbVersion, false);
    dal.CreatedDatabase += new OnCreatedDatabase(DatabaseCreated);
    dal.UpgradedDataBase += new OnUpdatedDataBase(UpgradedDataBase);
    dal.Init();

    return true;
}

/// <summary>
/// Do after database created
/// </summary>
private static void DatabaseCreated()
{
}

/// <summary>
/// Do after database upgraded
/// </summary>
private static void UpgradedDataBase(int oldVersion, int newVersion)
{
}
```
## Get a Dao instance
```C#
var daoUser = DBAccess.DAL.Dao<SysUser>();
```
## Insert
```C#
daoUser.Insert(new SysUser { UserName = "Tom", CName = "Tom", Email = "Tom@gmail.com"});
```
## Update
```C#
// update by key
var entity = listSource[0];
entity.CName = entity.UserName + "'s new name";
daoUser.UpdateByKey(entity);

// update by other condition
var updateBuilder = daoUser.UpdateBuilder();
updateBuilder.Update().Set("CName", "Jack'new name").Where().Equal("UserName", "Jack");
updateBuilder.Run();
```
## Query
```C#
// query all data
var listUser = daoUser.QueryAll();

// query by key
var entity = daoUser.QueryByKey(1);

// query by other condition and then order
var queryBuilder = daoUser.QueryBuilder();
queryBuilder.Select().Where().Equal("UserName", "Jack").OrderByDesc("UserName");
var listUser = queryBuilder.Query();

// query top records
var queryBuilder = daoUser.QueryBuilder();
queryBuilder.Select(2).Where().Like("CName", "J");
var listUser = queryBuilder.Query();

// query several columns
var queryBuilder = daoUser.QueryBuilder();
queryBuilder.Select("ID","UserName","Email").Where().Like("CName", "J");
var listUser = queryBuilder.Query();

// query by custom condition
var queryBuilder = daoUser.QueryBuilder();
queryBuilder.Select().Where().Custom("where UserName <> 'Tom'");
var listUser = queryBuilder.Query();

// query by sql 
var queryBuilder = daoUser.QueryBuilder();
queryBuilder.CustomSqlCommand =
    @"select A.* from SysUser A join SysUserRole B on A.ID=B.UserID 
                                join SysRole C on C.ID=B.RoleID
                                where C.RoleName='administrators'";
var listUser = queryBuilder.Query();
```
## Delete
```C#
// delete by key
daoUser.DeleteByKey(1);

// delete by other condition
var deleteBuilder = daoUser.DeleteBuilder();
deleteBuilder.Delete().Where().Equal("UserName", "Tom");
deleteBuilder.Run();

// delete all data
daoUser.DeleteAll();
```
## More
For more usage, please refer to the demo.
