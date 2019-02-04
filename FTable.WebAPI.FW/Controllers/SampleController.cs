﻿// Install-Package MultipartDataMediaFormatter.V2
using FTable.WebAPI.FW.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Http;
using Newtonsoft.Json;
using System.IO;
using System.Web.Hosting;
using System.Threading.Tasks;
using System.Reflection;
using FTable.WebAPI.FW.DTOs.FTable;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace FTable.WebAPI.FW.Controllers
{
  
   // [Authorize]
    public class SampleController : ApiController
    {


/// <summary>
/// Get all data (Not Used in FTable Remote Sample)
/// </summary>
/// <returns></returns>
        [HttpGet]
        [Route("api/v1/Sample")]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var filePath = HostingEnvironment.MapPath(@"~/App_Data/data.json");
                JsonSerializer s = new JsonSerializer();
                var json = File.ReadAllText(filePath);
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
        [Route("api/v1/Sample/{Id}")]
        public async Task<IHttpActionResult> Get(int Id)
        {
            try
            {
                var filePath = HostingEnvironment.MapPath(@"~/App_Data/data.json");
                JsonSerializer s = new JsonSerializer();
                var json = File.ReadAllText(filePath);
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
        [Route("api/v1/Sample/{Id}")]
        public async Task<IHttpActionResult> Put(int Id, [FromBody]PersonDTO Person)
        {
            try
            {
                var filePath = HostingEnvironment.MapPath(@"~/App_Data/data.json");
                JsonSerializer s = new JsonSerializer();
                var json = File.ReadAllText(filePath);
                IEnumerable<PersonDTO> Persons = JsonConvert.DeserializeObject<IEnumerable<PersonDTO>>(json);
                IEnumerable<PersonDTO> Temp = Persons.Where(x => x.Id != Person.Id);
                Persons = Temp.Concat(new[] { Person });
                json = JsonConvert.SerializeObject(Persons);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                using (StreamWriter writer = File.CreateText(filePath))
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
        [Route("api/v1/Sample/{Id}/{ColumnName}")]
        public async Task<IHttpActionResult> Put(int Id, string ColumnName, [FromBody]dynamic Value)
        {
            try
            {
                ColumnName = char.ToUpper(ColumnName[0]) + ColumnName.Substring(1);
                var filePath = HostingEnvironment.MapPath(@"~/App_Data/data.json");
                PersonDTO Person = null;
                JsonSerializer s = new JsonSerializer();
                var json = File.ReadAllText(filePath);
                IEnumerable<PersonDTO> Persons = JsonConvert.DeserializeObject<IEnumerable<PersonDTO>>(json);
                Persons.Where(w => w.Id == Id).ToList().ForEach(x =>
                {
                    Person = x;
                    PropertyInfo property = typeof(PersonDTO).GetProperty(ColumnName);
                    if (property.PropertyType == typeof(string))
                    {
                        property.SetValue(Person, Value, null);
                    } else if(property.PropertyType == typeof(int))
                    {
                        property.SetValue(Person, Int32.Parse(Value), null);
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        property.SetValue(Person, Value, null);
                    }
                });
                json = JsonConvert.SerializeObject(Persons);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                using (StreamWriter writer = File.CreateText(filePath))
                {
                    await writer.WriteAsync(json);
                }
                return Ok(Person);
            } catch(Exception ex)
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
        [Route("api/v1/Sample/{Id}")]
        public async Task<IHttpActionResult> Delete(int Id)
        {
            try
            {
                var filePath = HostingEnvironment.MapPath(@"~/App_Data/data.json");
                JsonSerializer s = new JsonSerializer();
                var json = File.ReadAllText(filePath);
                IEnumerable<PersonDTO> Persons = JsonConvert.DeserializeObject<IEnumerable<PersonDTO>>(json);
                Persons = Persons.Where(x => x.Id != Id);
                json = JsonConvert.SerializeObject(Persons);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                using (StreamWriter writer = File.CreateText(filePath))
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
        [Route("api/v1/Sample")]
        public async Task<IHttpActionResult> Post([FromBody]PersonDTO Person)
        {
            try
            {
                var filePath = HostingEnvironment.MapPath(@"~/App_Data/data.json");
                JsonSerializer s = new JsonSerializer();
                var json = File.ReadAllText(filePath);
                IEnumerable<PersonDTO> Persons = JsonConvert.DeserializeObject<IEnumerable<PersonDTO>>(json);
                Person.Id = Persons.Last().Id + 1;
                Persons = Persons.Concat(new[] { Person });

                json = JsonConvert.SerializeObject(Persons);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                using (StreamWriter writer = File.CreateText(filePath))
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
        [HttpPost]
        [Route("api/v1/Sample/FTables")]
        public IHttpActionResult FTables([FromBody]FTableDataModifierDTO FTableDataModifier) //[FromBody]FTableParameters parameters)
        {
            var globalSearchValue = FTableDataModifier.Search.Value;
            var pagingLength = FTableDataModifier.PageSize;
            var pagingStartIndex = (FTableDataModifier.currentPage-1) * FTableDataModifier.PageSize;
           
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
            if (globalSearchValue == null)
                globalSearchValue = "";
            else
            {
                var globalSearchJoiner = ".ToString().Contains(\"" + globalSearchValue.ToLower() + "\") ";
                globalSearch = string.Join(globalSearchJoiner + "or ", columnNames);
                globalSearch = globalSearch + globalSearchJoiner;
            }



     


            // Column Filters
           var filters = new List<string>();
            foreach (var filter in FTableDataModifier.Filters)
            {
                filter.ColumnName  = char.ToUpper(filter.ColumnName[0]) + filter.ColumnName.Substring(1);
                if (filter.Type == "string")
                {
                    var Value = filter.Apply["value"].ToString();
                    filters.Add( filter.ColumnName + ".Contains(\"" + Value + "\")" );
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
                        filters.Add(filter.ColumnName+" <= " +Max);
                    }
                    
                }
                else if (filter.Type == "email")
                {
                    // TODO
                    var First = "";
                    if (filter.Apply.ContainsKey("first"))
                        First = filter.Apply["first"].ToString();
                    var Second = "";
                    if (filter.Apply.ContainsKey("second"))
                        Second = filter.Apply["second"].ToString();
                    if (First.Length > 0 && Second.Length > 0)
                        filters.Add(filter.ColumnName+ ".Split(\"@\")[0].Contains(\"" + First + "\") and "+ filter.ColumnName + ".Split(\"@\")[1].Contains(\"" + Second + "\")");
                    else if (First.Length > 0)
                        filters.Add(filter.ColumnName + ".Split(\"@\")[0].Contains(\"" + First + "\")");
                    else if (Second.Length > 0)
                        filters.Add(filter.ColumnName + ".Split(\"@\")[1].Contains(\"" + Second + "\")");
                     

                }
                else if (filter.Type == "checkbox")
                {
                    var checkboxes = new List<string>();
                    var Values = filter.Apply["values"];
                    //filters.Add(filter.ColumnName + " >= " + Min + " and " + filter.ColumnName + " <= " + Max);
                    foreach (var value in Values)
                    {
                        checkboxes.Add(filter.ColumnName + ".Contains(\"" + value + "\")");
                    }
                        filters.Add("("+string.Join(" or ", checkboxes) + ")");
                }
                else if (filter.Type == "date")
                {
                    // TODO
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

            var filePath = HostingEnvironment.MapPath(@"~/App_Data/data.json");
            JsonSerializer s = new JsonSerializer();
            var json = File.ReadAllText(filePath);
            IEnumerable<PersonDTO> Persons = JsonConvert.DeserializeObject<IEnumerable<PersonDTO>>(json);

           var Result = Persons.Where(globalSearch)
                    .Where(columnFilterLogic)
                    .OrderBy(SortOrders)
                    .Skip(pagingStartIndex)
                    .Take(pagingLength).ToList();

            FTableResult.totalRows = Persons.Count();
            FTableResult.totalRowsAfterModifications = Persons.Count();
            FTableResult.Page = Result;
            return Ok(FTableResult);
        }

        #endregion FTables
    }

}

