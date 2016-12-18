using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.XPath;
using CarritoSosua.Attributes;
using CarritoSosua.DAL;
using CarritoSosua.ExtensionMethods;
using CarritoSosua.Models;
using Newtonsoft.Json;

namespace CarritoSosua.Controllers
{
    public class HomeController : Controller
    {
        private CarritoSosuaContext db = new CarritoSosuaContext();

        public ActionResult Index()
        {
            ViewBag.Semanas = GetWeekList().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.ToString()
                                  });
            ViewBag.LocalidadId = new SelectList(db.Localidades, "Id", "Name");
            return View();
        }

        public ActionResult List(int localidadId, string semanas)
        {
            string first = semanas.Substring(0, 8);
            DateTime firstDay = DateTime.ParseExact(first, "dd\\/MM\\/yy",
                System.Globalization.CultureInfo.InvariantCulture);


            ViewBag.Turnos = GetTurnosList(localidadId);
            ViewBag.Week = GetWeekDaysList(firstDay, localidadId);
            ViewBag.LocalidadId = new SelectList(db.Localidades.OrderBy(l => l.Name), "Id", "Name");
            ViewBag.Semanas = GetWeekList().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.ToString()
                                  });
            ViewBag.PublicadorId = new SelectList(db.Publicadores.OrderBy(p => p.Name), "Id", "Name");
            ViewBag.FirstDay = firstDay.ToString("dd\\/MM\\/yy");
            ViewBag.SelectedLocalidad = localidadId.ToString();
            return View("Index", CreatePublicadorTurnoLocalidadList(localidadId, firstDay));
        }

        [HttpPost]
        public ActionResult Save(string firstDay, string day, string localidad, string turno, int publicadorId)
        {
            var localidadId = int.Parse(localidad);
            var dayDate = DateTime.ParseExact(day.Substring(day.Length - 10, 10), "dd\\/MM\\/yyyy",
                System.Globalization.CultureInfo.InvariantCulture);
            var timeFrom = DateTime.MinValue;
            if(turno.Substring(4, 1) == " ")
            {
                if (turno.Substring(5, 2) == "AM" || turno.Substring(5, 2) == "PM")
                {
                    timeFrom = DateTime.ParseExact(turno.Substring(0, 7), "h\\:mm tt", 
                System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    timeFrom = DateTime.ParseExact(turno.Substring(0, 4), "H\\:mm",
                System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            else
            {
                if (turno.Substring(6, 2) == "AM" || turno.Substring(6, 2) == "PM")
                {
                    timeFrom = DateTime.ParseExact(turno.Substring(0, 8), "hh\\:mm tt",
                System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    timeFrom = DateTime.ParseExact(turno.Substring(0, 5), "HH\\:mm",
                System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            if (
                db.PublicadorTurnos.Any(
                    x =>
                        x.Day.Year == dayDate.Year &&
                        x.Day.Month == dayDate.Month &&
                        x.Day.Day == dayDate.Day &&
                        x.PublicadorId == publicadorId  &&
                        x.TurnoId ==
                        db.Turnos.FirstOrDefault(
                            t =>
                                t.TimeFrom.Hour == timeFrom.Hour &&
                                t.TimeFrom.Minute == timeFrom.Minute &&
                                t.DayOfWeek == dayDate.DayOfWeek && t.LocalidadId == localidadId).Id))
            {
                return Json(new {errorOccurred = true, errorMessage = "Publicador ya esta asignado!"});
            }
            else
            {
                var nptl = new PublicadorTurno()
                {
                    PublicadorId = publicadorId,
                    Publicador = db.Publicadores.FirstOrDefault(p => p.Id == publicadorId),
                    TurnoId =
                        db.Turnos.FirstOrDefault(
                            t =>
                                t.TimeFrom.Hour == timeFrom.Hour &&
                                t.TimeFrom.Minute == timeFrom.Minute &&
                                t.DayOfWeek == dayDate.DayOfWeek && t.LocalidadId == localidadId).Id,
                    Turno =
                        db.Turnos.FirstOrDefault(
                            t =>
                                t.TimeFrom.Hour == timeFrom.Hour &&
                                t.TimeFrom.Minute == timeFrom.Minute &&
                                t.DayOfWeek == dayDate.DayOfWeek && t.LocalidadId == localidadId),
                    Day = dayDate,
                    Disponible = true
                };
                db.PublicadorTurnos.Add(nptl);
                db.SaveChanges();
            }

            ViewBag.SelectedLocalidad = localidad;
            return Json(new { firstDay = firstDay, localidadId = localidad });
        }

        public ActionResult Delete(string firstDay, string day, string localidad, string turno, int publicadorId)
        {
            var localidadId = int.Parse(localidad);
            var dayDate = DateTime.ParseExact(day.Substring(day.Length - 10, 10), "dd\\/MM\\/yyyy",
                System.Globalization.CultureInfo.InvariantCulture);
            var timeFrom = DateTime.MinValue;
            if (turno.Substring(4, 1) == " ")
            {
                if (turno.Substring(5, 2) == "AM" || turno.Substring(5, 2) == "PM")
                {
                    timeFrom = DateTime.ParseExact(turno.Substring(0, 7), "h\\:mm tt",
                System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    timeFrom = DateTime.ParseExact(turno.Substring(0, 4), "H\\:mm",
                System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            else
            {
                if (turno.Substring(6, 2) == "AM" || turno.Substring(6, 2) == "PM")
                {
                    timeFrom = DateTime.ParseExact(turno.Substring(0, 8), "hh\\:mm tt",
                System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    timeFrom = DateTime.ParseExact(turno.Substring(0, 5), "HH\\:mm",
                System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            var ptl = db.PublicadorTurnos.FirstOrDefault(
                x =>
                    x.Day.Year == dayDate.Year &&
                    x.Day.Month == dayDate.Month &&
                    x.Day.Day == dayDate.Day &&
                    x.PublicadorId == publicadorId &&
                    x.TurnoId ==
                    db.Turnos.FirstOrDefault(
                        t =>
                            t.TimeFrom.Hour == timeFrom.Hour &&
                            t.TimeFrom.Minute == timeFrom.Minute &&
                            t.DayOfWeek == dayDate.DayOfWeek && t.LocalidadId == localidadId).Id);
            if (ptl != null)
            {
                db.PublicadorTurnos.Remove(ptl);
                db.SaveChanges();
            }

            return RedirectToAction("List", new { localidadId = localidadId, semanas = firstDay });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        private static List<string> GetWeekList()
        {
            var startDate = new DateTime();
            //Getting earliest Monday
            for (int i = 0; i < 7; i++)
            {
                if (DateTime.Now.AddDays(-i).DayOfWeek == DayOfWeek.Monday)
                {
                    startDate = DateTime.Now.AddDays(-i);
                    break;
                }
            }
            var endDate = new DateTime();
            //Getting earliest Monday of the month after the next
            for (int i = 0; i < 7; i++)
            {
                if (new DateTime(DateTime.Now.AddMonths(2).Year, DateTime.Now.AddMonths(2).Month, 1).AddDays(i).DayOfWeek == DayOfWeek.Monday)
                {
                    endDate = new DateTime(DateTime.Now.AddMonths(2).Year, DateTime.Now.AddMonths(2).Month, 1).AddDays(i);
                    break;
                }
            }
            //Getting List for this and next month
            List<string> weekList = new List<string>();
            var date = startDate;
            DateTime? first = null;
            DateTime? last = null;
            while (date.Date < endDate.Date)
            {
                if (date.DayOfWeek == DayOfWeek.Monday)
                {
                    first = date;
                }
                if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    last = date;
                }
                if (first.HasValue && last.HasValue)
                {
                    weekList.Add(first.Value.ToString("dd\\/MM\\/yy") + " - " + last.Value.ToString("dd\\/MM\\/yy"));
                    first = null;
                    last = null;
                }
                date = date.AddDays(1);
            }
            return weekList;
        }

        private List<String> GetTurnosList(int localidadId)
        {
            var turnos = db.Set<Turno>().Where(x => x.LocalidadId == localidadId).OrderBy(x => x.TimeFrom.Hour).ThenBy(x => x.TimeFrom.Minute).ThenBy(x => x.TimeFrom.Second);
            var turnoList = new List<String>();
            foreach (var turno in turnos)
            {
                if (!turnoList.Contains(turno.TimeFrom.ToShortTimeString() + " - " + turno.TimeTo.ToShortTimeString()))
                {
                    turnoList.Add(turno.TimeFrom.ToShortTimeString() + " - " + turno.TimeTo.ToShortTimeString());
                }
            }
            return turnoList;
        }

        private List<String> GetWeekDaysList(DateTime firstDay, int localidadId)
        {
            var weekDaysList = new List<String>();
            for (DateTime date = firstDay;
                date < firstDay.AddDays(7);
                date = date.AddDays(1))
            {
                if(db.Turnos.Any(x => x.DayOfWeek == date.DayOfWeek && x.LocalidadId == localidadId))
                {
                    weekDaysList.Add(date.DayOfWeek.ToSpanishDayOfWeekName() + " " + date.ToString("dd\\/MM\\/yyyy"));
                }
            }
            return weekDaysList;
        } 


        private IList<PublicadorTurno> CreatePublicadorTurnoLocalidadList(int localidadId, DateTime firstDay)
        {
            IList<PublicadorTurno> publicadorTurnoLocalidadList = new List<PublicadorTurno>();
            for (DateTime date = firstDay;
                date < firstDay.AddDays(7);
                date = date.AddDays(1))
            {
                List<Turno> turnos = db.Turnos.Where(t => t.DayOfWeek == date.DayOfWeek && t.LocalidadId == localidadId).ToList();
                if (turnos.Count() != 0)
                {
                    foreach (var turno in turnos)
                    {
                        List<PublicadorTurno> publicadorTurnoLocalidades = db.PublicadorTurnos.Where(p =>
                                    DbFunctions.TruncateTime(p.Day) == date.Date && p.TurnoId == turno.Id).Include(x => x.Publicador).Include(x => x.Turno).ToList();
                        if (publicadorTurnoLocalidades.Count() != 0)
                        {
                            foreach (var ptl in publicadorTurnoLocalidades)
                            {
                                publicadorTurnoLocalidadList.Add(ptl);
                            }
                        }
                        else
                        {
                            publicadorTurnoLocalidadList.Add(new PublicadorTurno
                            {
                                TurnoId = turno.Id,
                                Turno = turno,
                                Day = date,
                                Disponible = false
                            });
                        }
                    }
                }
                else
                {
                    publicadorTurnoLocalidadList.Add(new PublicadorTurno
                    {
                        Turno = new Turno()
                        {
                            DayOfWeek = date.DayOfWeek,
                            LocalidadId = localidadId,
                            Localidad = db.Localidades.FirstOrDefault(l => l.Id == localidadId)
                        },
                        Day = date,
                        Disponible = false
                    });
                }
            }
            return publicadorTurnoLocalidadList;
        }
    }
}