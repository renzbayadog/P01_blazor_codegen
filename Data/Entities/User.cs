using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RenzGrandWeddingblazor.ph.Data.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        
        #region Relationships
        public virtual ICollection<IdentityUserRole<int>> Roles { get; } = new List<IdentityUserRole<int>>();

        public virtual ICollection<IdentityUserClaim<int>> Claims { get; } = new List<IdentityUserClaim<int>>();

        public virtual ICollection<IdentityUserLogin<int>> Logins { get; } = new List<IdentityUserLogin<int>>();

        #endregion

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
