using Microsoft.AspNetCore.Identity;
using RenzGrandWeddingblazor.ph.Data.Entities;
using System;

namespace RenzGrandWeddingblazor.Data.Entities
{
    public class Role : IdentityRole<int>
    {
        public Role()
        {
        }

        public Role(string roleName) : base(roleName)
        {
        }

        public string RoleDescription { get; set; }      
        #region Default Properties
        public bool IsDeleted { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeletedById { get; set; }
        public DateTime? DeleteDate { get; set; }

        public User CreatedBy { get; set; }
        public User UpdatedBy { get; set; }
        public User DeletedBy { get; set; }
        #endregion
    }
}
