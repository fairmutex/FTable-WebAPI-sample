using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;
using FTable.API.Core.DTOs;
using FTable.API.Core.DTOs.FTable;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FTable.API.Core.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private string filePath;
        public SampleController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            this.filePath = this._hostingEnvironment.ContentRootPath + "/data/data.json";
        }

        /// <summary>
        /// Get all data (Not Used in FTable Remote Sample)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
               // var filePath = HostingEnvironment.MapPath(@"~/App_Data/data.json");
                JsonSerializer s = new JsonSerializer();
                var json = System.IO.File.ReadAllText(this.filePath);
                IEnumerable<PersonDTO> Persons = JsonConvert.DeserializeObject<IEnumerable<PersonDTO>>(json);
                return Ok(Persons);
            }
            catch (Exception ex)
            {

                return NotFound();
            }
        }


        /// <summary>
        /// Get by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
            try
            {
                
                //var filePath = HostingEnvironment.MapPath(@"~/App_Data/data.json");
                JsonSerializer s = new JsonSerializer();
                var json = System.IO.File.ReadAllText(this.filePath);
                IEnumerable<PersonDTO> Persons = JsonConvert.DeserializeObject<IEnumerable<PersonDTO>>(json);
                PersonDTO Person = Persons.First(x => x.Id == Id);
                return Ok(Person);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Update Record (Not used by FTable Sample)
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Person"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{Id}")]
        public async Task<IActionResult> Put(int Id, [FromBody]PersonDTO Person)
        {
            try
            {
                //var filePath = HostingEnvironment.MapPath(@"~/App_Data/data.json");
                JsonSerializer s = new JsonSerializer();
                var json = System.IO.File.ReadAllText(this.filePath);
                IEnumerable<PersonDTO> Persons = JsonConvert.DeserializeObject<IEnumerable<PersonDTO>>(json);
                IEnumerable<PersonDTO> Temp = Persons.Where(x => x.Id != Person.Id);
                Persons = Temp.Concat(new[] { Person });
                json = JsonConvert.SerializeObject(Persons);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                using (StreamWriter writer = System.IO.File.CreateText(filePath))
                {
                    await writer.WriteAsync(json);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }



        /// <summary>
        /// Updating property by value dynamically 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ColumnName"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
       // [AllowAnonymous]
        [HttpPut]
        [Route("{Id}/{ColumnName}")]
        public async Task<IActionResult> Put(int Id, string ColumnName, [FromBody]dynamic Value)
        {
            try
            {
                ColumnName = char.ToUpper(ColumnName[0]) + ColumnName.Substring(1);
                //var filePath = HostingEnvironment.MapPath(@"~/App_Data/data.json");
                PersonDTO Person = null;
                JsonSerializer s = new JsonSerializer();
                var json = System.IO.File.ReadAllText(this.filePath);
                IEnumerable<PersonDTO> Persons = JsonConvert.DeserializeObject<IEnumerable<PersonDTO>>(json);
                Persons.Where(w => w.Id == Id).ToList().ForEach(x =>
                {
                    Person = x;
                    PropertyInfo property = typeof(PersonDTO).GetProperty(ColumnName);
                    if (property.PropertyType == typeof(string))
                    {
                        property.SetValue(Person, Value, null);
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        property.SetValue(Person, Int32.Parse(Value), null);
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        property.SetValue(Person, Value, null);
                    }
                });
                json = JsonConvert.SerializeObject(Persons);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                using (StreamWriter writer = System.IO.File.CreateText(filePath))
                {
                    await writer.WriteAsync(json);
                }
                return Ok(Person);
            }
            catch (Exception ex)
            {

                return NotFound();
            }

        }


        /// <summary>
        /// Delete Row
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                //var filePath = HostingEnvironment.MapPath(@"~/App_Data/data.json");
                JsonSerializer s = new JsonSerializer();
                var json = System.IO.File.ReadAllText(this.filePath);
                IEnumerable<PersonDTO> Persons = JsonConvert.DeserializeObject<IEnumerable<PersonDTO>>(json);
                Persons = Persons.Where(x => x.Id != Id);
                json = JsonConvert.SerializeObject(Persons);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                using (StreamWriter writer = System.IO.File.CreateText(filePath))
                {
                    await writer.WriteAsync(json);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create Record
        /// </summary>
        /// <param name="Person"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Sample")]
        public async Task<IActionResult> Post([FromBody]PersonDTO Person)
        {
            try
            {
                //var filePath = HostingEnvironment.MapPath(@"~/App_Data/data.json");
                JsonSerializer s = new JsonSerializer();
                var json = System.IO.File.ReadAllText(this.filePath);
                IEnumerable<PersonDTO> Persons = JsonConvert.DeserializeObject<IEnumerable<PersonDTO>>(json);
                Person.Id = Persons.Last().Id + 1;
                Persons = Persons.Concat(new[] { Person });

                json = JsonConvert.SerializeObject(Persons);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                using (StreamWriter writer = System.IO.File.CreateText(filePath))
                {
                    await writer.WriteAsync(json);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        #region FTables
        /// <summary>
        /// FTables Backend Driven
        /// </summary>
        /// <param name="FTableDataModifier"></param>
        /// <returns></returns>
        [HttpOptions]
        [HttpPost]
        [Route("FTables")]
        public async Task<IActionResult> FTables([FromBody]FTableDataModifierDTO FTableDataModifier) //[FromBody]FTableParameters parameters)
        {
            var globalSearchValue = FTableDataModifier.Search.Value;
            var pagingLength = FTableDataModifier.PageSize;
            var pagingStartIndex = (FTableDataModifier.currentPage - 1) * FTableDataModifier.PageSize;

            //FTableDataModifier.Filters


            // get Property names and types
            List<string> columnNamesList = new List<string>();
            //List<string> columnTypeList = new List<string>();
            foreach (var prop in (new PersonDTO()).GetType().GetProperties())
            {
                columnNamesList.Add(prop.Name);
                //columnTypeList.Add(prop.GetType().Name);
            }
            string[] columnNames = columnNamesList.ToArray();




            // Global Search
            var globalSearch = "1=1";
            if (globalSearchValue != "")
            {
                var globalSearchJoiner = ".ToString().Contains(\"" + globalSearchValue.ToLower() + "\") ";
                globalSearch = string.Join(globalSearchJoiner + "or ", columnNames);
                globalSearch = globalSearch + globalSearchJoiner;
            }






            // Column Filters
            var filters = new List<string>();
            foreach (var filter in FTableDataModifier.Filters)
            {
                filter.ColumnName = char.ToUpper(filter.ColumnName[0]) + filter.ColumnName.Substring(1);
                if (filter.Type == "string")
                {
                    var Value = filter.Apply["value"].ToString();
                    filters.Add(filter.ColumnName + ".Contains(\"" + Value + "\")");
                }
                else if (filter.Type == "number")
                {
                    var Min = "";

                    if (filter.Apply.ContainsKey("min"))
                        Min = filter.Apply["min"].ToString();
                    var Max = "";
                    if (filter.Apply.ContainsKey("max"))
                        Max = filter.Apply["max"].ToString();

                    if (Min.Length > 0 && Max.Length > 0)
                    {
                        filters.Add(filter.ColumnName + " >= " + Min + " and " + filter.ColumnName + " <= " + Max);
                    }
                    else if (Min.Length > 0)
                    {
                        filters.Add(filter.ColumnName + " >= " + Min);
                    }
                    else if (Max.Length > 0)
                    {
                        filters.Add(filter.ColumnName + " <= " + Max);
                    }

                }
                else if (filter.Type == "email")
                {
                    var email = new List<string>();

                    var LocalPart = "";
                    if (filter.Apply.ContainsKey("localPart"))
                    {
                        LocalPart = filter.Apply["localPart"].ToString();
                        if (LocalPart.Length > 0)
                        {
                            email.Add("(" + filter.ColumnName + ".IndexOf(\"" + LocalPart + "\") > -1)");
                            email.Add("(" + filter.ColumnName + ".IndexOf(\"" + LocalPart + "\") <= " + filter.ColumnName + ".IndexOf(\"@\"))");
                        }
                    }

                    // Domain part of the email
                    var Domain = "";
                    if (filter.Apply.ContainsKey("domain"))
                    {
                        Domain = filter.Apply["domain"].ToString();
                        if (Domain.Length > 0)
                            email.Add("(" + filter.ColumnName + ".IndexOf(\"@\") <= " + filter.ColumnName + ".IndexOf(\"" + Domain + "\"))");
                    }
                    if (email.Count > 0)
                        filters.Add("(" + string.Join(" and ", email) + ")");
                }
                else if (filter.Type == "checkbox")
                {
                    var checkboxes = new List<string>();
                    var Values = filter.Apply["values"];
                    if (Values.Count() > 0)
                    {
                        foreach (var value in Values)
                        {
                            checkboxes.Add(filter.ColumnName + ".Contains(\"" + value + "\")");
                        }
                    }
                    else
                    {
                        checkboxes.Add("1=0");
                    }
                    if (checkboxes.Count > 0)
                        filters.Add("(" + string.Join(" or ", checkboxes) + ")");
                }
                else if (filter.Type == "date")
                {

                    var dates = new List<string>();
                    var minDay = filter.Apply["minDay"].ToString();
                    if (minDay.Length > 0)
                    {
                        dates.Add(filter.ColumnName + ".Day >=" + minDay);
                    }
                    var minMonth = filter.Apply["minMonth"].ToString();
                    if (minMonth.Length > 0)
                    {
                        dates.Add(filter.ColumnName + ".Month >=" + minMonth);
                    }
                    var minYear = filter.Apply["minYear"].ToString();
                    if (minYear.Length > 0)
                    {
                        dates.Add(filter.ColumnName + ".Year >=" + minYear);
                    }
                    var maxDay = filter.Apply["maxDay"].ToString();
                    if (maxDay.Length > 0)
                    {

                        dates.Add(filter.ColumnName + ".Day <=" + maxDay);
                    }
                    var maxMonth = filter.Apply["maxMonth"].ToString();
                    if (maxMonth.Length > 0)
                    {
                        dates.Add(filter.ColumnName + ".Month <=" + maxMonth);
                    }
                    var maxYear = filter.Apply["maxYear"].ToString();
                    if (maxYear.Length > 0)
                    {
                        dates.Add(filter.ColumnName + ".Year <=" + maxYear);
                    }
                    if (dates.Count > 0)
                        filters.Add("(" + string.Join(" and ", dates) + ")");
                }
            }
            var columnFilterLogic = "1=1";
            if (filters.Count > 0)
            {
                columnFilterLogic = string.Join(" and ", filters);
            }


            // Order By Then By
            string SortOrders = string.Join(", ", FTableDataModifier.Orders.Select(x => x.ColumnName + " " + x.Direction));
            if (SortOrders == "") SortOrders = "Id Asc";



            FTableResultDTO FTableResult = new FTableResultDTO();

            //var filePath = HostingEnvironment.MapPath(@"~/App_Data/data.json");
            JsonSerializer s = new JsonSerializer();
            var json = System.IO.File.ReadAllText(this.filePath);
            IQueryable<PersonDTO> Persons = JsonConvert.DeserializeObject<IEnumerable<PersonDTO>>(json).AsQueryable();

            var Result = Persons
                     .Where(globalSearch)
                     .Where(columnFilterLogic)
                     .OrderBy(SortOrders)
                     .Skip(pagingStartIndex)
                     .Take(pagingLength).ToList();

            FTableResult.totalRows = Persons.Count();
            FTableResult.totalRowsAfterModifications = Persons.Where(globalSearch).Where(columnFilterLogic).Count();

            FTableResult.Page = Result;
            return Ok(FTableResult);
        }

        #endregion FTables
    }


}
