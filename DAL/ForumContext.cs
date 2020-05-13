using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Fluent_API_Configurations;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL
{
    public class ForumContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Topic> Topics { get; set; }    
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public ForumContext()
            : base()
        {
        }

        public ForumContext(DbContextOptions options)
            : base(options)
        {
            //Database.EnsureCreated();
            //Seed();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new TopicConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
        }

        private void Seed()
        {

            var adminRole = new IdentityRole<int> { Name = "Admin" };
            var moderatorRole = new IdentityRole<int> { Name = "Moderator" };
            var userRole = new IdentityRole<int> { Name = "User" };

            Roles.Add(adminRole);
            Roles.Add(moderatorRole);
            Roles.Add(userRole);
            SaveChanges();

            User admin = new User()
            {
                UserName = "kvazar2569",
                PasswordHash = "cg/+jWvRrsDXTWAvy5oCkTP1K+S/Uti6niwDI0nSsRE=",
                Email = "kvazar2569@gmail.com",
                Age = DateTime.Now.Year - 2001,
                Gender = "Male",
                AvatarPath = "https://www.instagram.com/p/B9WtDAPDazk/",
                UserRole = adminRole
            };

            User another = new User()
            {
                UserName = "thirdUser",
                PasswordHash = "cg/+jWvRrsDXTWAvy5oCkTP1K+S/Uti6niwDI0nSsRE=",
                Email = "matviichuk.tilda@gmail.com",
                Age = DateTime.Now.Year - 1994,
                Gender = "Male",
                AvatarPath = "https://www.instagram.com/p/B9WtDAPDazk/",
                CapableToBan = false,
                CapableToSilence = false,
                UserRole = userRole
            };

            User mod = new User()
            {
                UserName = "matartem",
                PasswordHash = "cg/+jWvRrsDXTWAvy5oCkTP1K+S/Uti6niwDI0nSsRE=",
                Email = "matartem2307@gmail.com",
                Age = DateTime.Now.Year - 2001,
                Gender = "Male",
                AvatarPath = "https://www.instagram.com/p/B9WtDAPDazk/",
                CapableToBan = false,
                CapableToSilence = true,
                UserRole = moderatorRole
            };

            Users.Add(another);
            Users.Add(mod);
            Users.Add(admin);
            SaveChanges();


            //var userRoleAdmin = new IdentityUserRole<int>()
            //{
            //    RoleId = adminRole.Id,
            //    UserId = admin.Id
            //};

            //var userRoleModerator = new IdentityUserRole<int>()
            //{
            //    RoleId = moderatorRole.Id,
            //    UserId = mod.Id
            //};

            //var userRoleUser = new IdentityUserRole<int>()
            //{
            //    RoleId = userRole.Id,
            //    UserId = another.Id
            //};


            //UserRoles.Add(userRoleAdmin);
            //UserRoles.Add(userRoleModerator);
            //UserRoles.Add(userRoleUser);
            //SaveChanges();

            //admin.UserRole = userRoleAdmin;
            //mod.UserRole = userRoleModerator;
            //another.UserRole = userRoleUser;

            //adminRole.Users.Add(userRoleAdmin);
            //moderatorRole.Users.Add(userRoleModerator);
            //userRole.Users.Add(userRoleUser);

            Topic topic = new Topic()
            {
                Name = "Let'sTalk News"
            };

            Post post = new Post()
            {
                Title = "Let'sTalk forum created",
                User = admin,
                DateCreated = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
                Message = "Hello everyone! I have just created thiss forum. " +
                    "Its name is \"Let'sTalk\" ",
                Topic = topic
            };

            Comment comment = new Comment()
            {
                DateCreated = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
                User = mod,
                Post = post,
                Message = "This is first comment ever here"
            };

            Topics.Add(topic);
            Posts.Add(post);
            Comments.Add(comment);
            SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
