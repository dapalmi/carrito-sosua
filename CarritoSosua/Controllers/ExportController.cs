using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using CarritoSosua.DAL;
using CarritoSosua.ExtensionMethods;
using CarritoSosua.Models;

namespace CarritoSosua.Controllers
{
    public class ExportController : Controller
    {
        private CarritoSosuaContext db = new CarritoSosuaContext();

        // GET: Export
        public ActionResult Index()
        {
            var monthList = new List<string>();
            monthList.Add(DateTime.Now.Month.MonthToSpanishName());
            monthList.Add(DateTime.Now.AddMonths(1).Month.MonthToSpanishName());
            ViewBag.MonthList = monthList.Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.ToString()
                                  });
            return View();
        }

        public ActionResult ExportData(string monthList)
        {
            int month = monthList.MonthNameToNumber();

            var ptList = db.PublicadorTurnos.Where(x => x.Day.Month == month).ToList();

            GridView gv = new GridView();
            gv.DataSource = GetDataSource(month);
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Horario.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index");
        }

        private DataTable GetDataSource(int month)
        {
            var turnoList = GetTurnosList(month);

            DataTable exportTable = new DataTable();
            exportTable.Columns.Add("Puesto", typeof(string));
            exportTable.Columns.Add("Dia", typeof(string));
            foreach (var turno in turnoList)
            {
                exportTable.Columns.Add(turno.TimeFrom.ToShortTimeString() + " - " + turno.TimeTo.ToShortTimeString(), typeof(string)); 
            }

            var localidadList = GetLocalidadList(month);

            for (DateTime date = new DateTime(DateTime.Now.Year, month, 1); date < new DateTime(DateTime.Now.Year, month, 1).AddMonths(1); date = date.AddDays(1))
            {
                foreach (var l in localidadList)
                {
                    if (db.Turnos.Any(x => x.LocalidadId == l.Id && x.DayOfWeek == date.DayOfWeek))
                    {
                        DataRow dataRow = exportTable.NewRow();
                        dataRow["Dia"] = date.ToString("dd\\/MM\\/yyyy");
                        dataRow["Puesto"] = l.Name;
                        foreach (var turno in turnoList)
                        {
                            if (
                                db.Turnos.Any(
                                    x =>
                                        x.LocalidadId == l.Id && x.DayOfWeek == date.DayOfWeek &&
                                            x.TimeFrom.Hour == turno.TimeFrom.Hour &&
                                            x.TimeFrom.Minute == turno.TimeFrom.Minute &&
                                            x.TimeFrom.Second == turno.TimeFrom.Second &&
                                            x.TimeTo.Hour == turno.TimeTo.Hour &&
                                            x.TimeTo.Minute == turno.TimeTo.Minute &&
                                            x.TimeTo.Second == turno.TimeTo.Second))
                            {
                                var names =
                                    db.PublicadorTurnos.Where(
                                        x =>
                                            x.Day.Year == date.Year &&
                                            x.Day.Month == date.Month &&
                                            x.Day.Day == date.Day &&
                                            x.Turno.DayOfWeek == date.DayOfWeek &&
                                            x.Turno.TimeFrom.Hour == turno.TimeFrom.Hour &&
                                            x.Turno.TimeFrom.Minute == turno.TimeFrom.Minute &&
                                            x.Turno.TimeFrom.Second == turno.TimeFrom.Second &&
                                            x.Turno.TimeTo.Hour == turno.TimeTo.Hour &&
                                            x.Turno.TimeTo.Minute == turno.TimeTo.Minute &&
                                            x.Turno.TimeTo.Second == turno.TimeTo.Second).ToList();
                                var publishers = String.Empty;
                                foreach (var name in names)
                                {
                                    if (publishers == String.Empty)
                                    {
                                        publishers = name.Publicador.Name;
                                    }
                                    else
                                    {
                                        publishers = publishers + " y " + name.Publicador.Name;
                                    }
                                }
                                if (publishers == String.Empty)
                                    dataRow[
                                        turno.TimeFrom.ToShortTimeString() + " - " + turno.TimeTo.ToShortTimeString()] =
                                        "no asignaciones";
                                else
                                    dataRow[
                                        turno.TimeFrom.ToShortTimeString() + " - " + turno.TimeTo.ToShortTimeString()] =
                                        publishers;
                            }
                            else
                            {
                                dataRow[turno.TimeFrom.ToShortTimeString() + " - " + turno.TimeTo.ToShortTimeString()] =
                                    "-";
                            }
                        }
                        exportTable.Rows.Add(dataRow);
                    }
                }
            }
            return exportTable;
        }

        private List<Localidad> GetLocalidadList(int month)
        {
            var ptList = db.PublicadorTurnos.Where(x => x.Day.Month == month).OrderBy(x => x.Turno.Localidad.Name).ThenBy(x => x.Day).ThenBy(x => x.Turno.TimeFrom).ThenBy(x => x.Publicador.Name).ToList();
            var localidadList = new List<Localidad>();
            foreach (var pt in ptList)
            {
                if (!localidadList.Any(x => x.Id == pt.Turno.LocalidadId))
                {
                    localidadList.Add(pt.Turno.Localidad);
                }
            }
            return localidadList;
        }

        private List<Turno> GetTurnosList(int month)
        {
            var turnos = db.Set<Turno>().OrderBy(x => x.TimeFrom.Hour).ThenBy(x => x.TimeFrom.Minute).ThenBy(x => x.TimeFrom.Second);
            var turnoList = new List<Turno>();
            foreach (var turno in turnos)
            {
                if (!turnoList.Any(x => x.TimeFrom.TimeOfDay == turno.TimeFrom.TimeOfDay && x.TimeTo.TimeOfDay == turno.TimeTo.TimeOfDay))
                {
                    turnoList.Add(turno);
                }
            }
            return turnoList;
        }
    }
}