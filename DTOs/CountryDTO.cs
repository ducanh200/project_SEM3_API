using System;
using Microsoft.AspNetCore.Mvc;

namespace SEM3_API.DTOs

{
    public class CountryDTO : ControllerBase
    {

        public int id {  get; set; }

        public string name { get; set; }
    }
}
