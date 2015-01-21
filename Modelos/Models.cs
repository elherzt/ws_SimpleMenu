using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{

    public class User
    {
        public User() 
        {
 
        }
        [Key]
        public int IdUser { get; set; }
        [Required]
        [MaxLength(50)]
        public string email { get; set; }
        [Required]
        [MaxLength(50)]
        public string username { get; set; }
        [Required]
        [MaxLength(50)]
        public string password { get; set; }
        public int reference_id { get; set; }
        public bool locked { get; set; }
        public bool verificated { get; set; }
        public string token { get; set; }
        public DateTime register { get; set; }
        public virtual List<Login> Logins { get; set; }
        public virtual List<Rol_User> Roles { get; set; }
        public virtual List<Lock> Locks { get; set; }
    }


    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string clave { get; set; }
        public string body { get; set; }
    }

    public class Rol
    {
        [Key]
        public int IdRol { get; set; }
        public int reference_id { get; set; }
        public string description { get; set; }
    }

    public class Rol_User
    {
        [Key]
        public int Id { get; set; }
        public int IdRol { get; set; }
        public virtual Rol Rol { get; set; }
        public int IdUser { get; set; }
        //public virtual User User { get; set; }
    }

    public class Status
    {
        [Key]
        public int IdStatus { get; set; }
        public string description { get; set; }
    }

    public class Login
    {
        [Key]
        public int IdLogin { get; set; }
        public int IdUser {get; set; }
        public int id_reference { get; set; }
        //public virtual User User { get; set; }
        public int IdStatus { get; set; }
        //public virtual Status Status { get; set; }
        public DateTime date { get; set; }
        public string ip_address { get; set; }
        public string browser { get; set; }
    }


    public class Lock
    {
        [Key]
        public int IdLock { get; set; }
        public int IdUser { get; set; }
        // public virtual User User { get; set; }
        public DateTime date { get; set; }
    }


    public class UserContext : DbContext
    {
        public UserContext() : base("PruebasConnection")
        {
            Database.SetInitializer<UserContext>(new CreateDatabaseIfNotExists<UserContext>());
        }
        public DbSet<User> Users {get; set;}
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Rol_User> Roles_Users { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Lock> Locks { get; set; }
        public DbSet<Message> Messages { get; set; }
    }

}
