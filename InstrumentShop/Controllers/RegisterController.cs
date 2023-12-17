﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using InstrumentShop.Models;
using System.Drawing.Imaging;
using System.IO;

namespace InstrumentShop.Controllers
{

    public class RegisterController : Controller
    {
        string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mark\source\repos\InstrumentShop\InstrumentShop\App_Data\Database1.mdf;Integrated Security=True";




        public ActionResult Register(Register model)
        {
            List<Department> departments = new List<Department>();
            string user = model.Username;
            string pass = model.Password;
            string fname = model.fname;
            string mi = model.mi;
            string lname = model.lname;
            DateTime dob = model.DateofBirth;
            string phone = model.Phone;
            string address = model.Address;
            string email = model.Email;
            int dep_id = model.Department;
           
            try
            {
                using (var db = new SqlConnection(connString))
                {
                    db.Open();

                    // Fetch departments
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT dep_id, dep_name FROM DEPARTMENT";
                        DataTable dt = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(dt);

                        // Convert the DataTable to a list of Department objects
                        foreach (DataRow row in dt.Rows)
                        {
                            Department department = new Department
                            {
                                DepartmentId = Convert.ToInt32(row["dep_id"]),
                                DepartmentName = row["dep_name"].ToString()
                            };
                            departments.Add(department);
                        }

                        // Pass the list of departments to the view
                        ViewBag.DepList = new SelectList(departments, "DepartmentId", "DepartmentName");
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }


            try
            {
               
                using (var db1 = new SqlConnection(connString))
                {
                    db1.Open();
                    using (var cmd1 = db1.CreateCommand())
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.CommandText = "INSERT INTO [USERS] (user_fname,user_mi,user_lname,user_dob,user_phone,user_address,user_email,user_username,user_password,role_id,dep_id)" +
                            "VALUES(@fname,@mi,@lname,@dob,@phone,@address,@email,@user,@pass,@role_id,@DepartmentId)";
                        cmd1.Parameters.AddWithValue("@fname", fname);
                        cmd1.Parameters.AddWithValue("@mi", mi);
                        cmd1.Parameters.AddWithValue("@lname", lname);
                        cmd1.Parameters.AddWithValue("@dob", dob);
                        cmd1.Parameters.AddWithValue("@phone", phone);
                        cmd1.Parameters.AddWithValue("@address", address);
                        cmd1.Parameters.AddWithValue("@email", email);
                        cmd1.Parameters.AddWithValue("@user", user);
                        cmd1.Parameters.AddWithValue("@pass", pass);
                        cmd1.Parameters.AddWithValue("@role_id", 2);
                        cmd1.Parameters.AddWithValue("@DepartmentId", dep_id);
                        var ctr = cmd1.ExecuteNonQuery();
                        try
                        {

                            if (ctr >= 1)
                            {
                                return Json(new { success = true, message = "Data is saved" });
                            }
                            else
                            {
                                return Json(new { success = false, message = "Failed to save data" });
                            }
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError("", e.Message);
                            return View(model);
                        }

                    }
                }            
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }


        }
    }
}









