using System.Collections.Generic;
using Android.Util;
using FamilyLocator.Model;
using SQLite;

namespace FamilyLocator.Service
{
    public class Database
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        private string locationDb = "Location.db";

        public bool createDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, locationDb)))
                {
                    connection.CreateTable<Location>();
                    connection.CreateTable<Identity>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        //Add or Insert Operation  

        public bool insertIntoTable(Location location)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, locationDb)))
                {
                    connection.Insert(location);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public List<Location> selectTable()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, locationDb)))
                {
                    return connection.Table<Location>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        //Edit Operation  

        //public bool updateTable(Location person)
        //{
        //    try
        //    {
        //        using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, locationDb)))
        //        {
        //            connection.Query<Location>("UPDATE Person set Name=?, Department=?, Email=? Where Id=?", person.Name, person.Department, person.Email, person.Id);
        //            return true;
        //        }
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        Log.Info("SQLiteEx", ex.Message);
        //        return false;
        //    }
        //}
        //Delete Data Operation  

        //public bool removeTable(Location person)
        //{
        //    try
        //    {
        //        using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, locationDb)))
        //        {
        //            connection.Delete(person);
        //            return true;
        //        }
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        Log.Info("SQLiteEx", ex.Message);
        //        return false;
        //    }
        //}
        //Select Operation  

        public bool selectTable(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, locationDb)))
                {
                    connection.Query<Location>("SELECT * FROM Location Where Id=?", Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx: ", ex.Message);
                return false;
            }
        }
    }
}