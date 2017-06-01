using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.OleDb;
using System.Data;
using System.Text;



namespace MovieService1.Models
{
   
    public class MovieContext : DbContext
    {
        
        public MovieContext()
           
            : base("name=MovieContext")
        {
            
        }
        //add Db sets to the Context class so that Entity Framework 
        //will include the Movies and 
        public DbSet<Movie> Movies { get; set; }
        public DbSet<FilmProfessional> FilmProfessional { get; set; }

        
        //Get the Connection String to Excel Spreadsheet
        public string GetConnectionString()
        {
            Dictionary<string, string> props = new Dictionary<string, string>();
            props["Provider"] = "Microsoft.ACE.OLEDB.12.0";
            props["Extended Properties"] = "Excel 12.0 XML";
            props["Data Source"] = "C:\\Users\\zapucsx\\Documents\\MovieDatabase.xlsx";

            StringBuilder sb = new StringBuilder();

            foreach(KeyValuePair<string,string> prop in props)
            {
                sb.Append(prop.Key);
                sb.Append("=");
                sb.Append(prop.Value);
                sb.Append(';');
            }
            return sb.ToString();
        }
    }
   
 



}


        
       
        
