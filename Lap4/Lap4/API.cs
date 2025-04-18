﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lap4
{
    internal class API
    {
        public class Photo
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Url { get; set; }
            public string ThumbnailUrl { get; set; }
        }

        public class Comment
        {
            public int Id { get; set; }
            public int PostId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Body { get; set; }
        }

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public Address Address { get; set; }
            public string Phone { get; set; }
            public string Website { get; set; }
            public Company Company { get; set; }
        }

        public class Address
        {
            public string Street { get; set; }
            public string Suite { get; set; }
            public string City { get; set; }
            public string Zipcode { get; set; }
        }

        public class Company
        {
            public string Name { get; set; }
            public string CatchPhrase { get; set; }
            public string Bs { get; set; }
        }

    }
}
