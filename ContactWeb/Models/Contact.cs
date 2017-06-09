using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactWeb.Models
{
    //Our Contact object
    public class Contact
    {
        //Use properties to model the attributes.
        //ID is required for controllers and models later.

        public int Id { get; set; } //allows us to get and set an integer into that Id property
        public Guid UserId { get; set; } //allows us to store the user
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhonePrimary { get; set; }
        public string PhoneSecondary { get; set; }
        public DateTime Birthday { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

    }
}