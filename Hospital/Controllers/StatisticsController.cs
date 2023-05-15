using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DAL.Models;
using BLL.Service;
using BLL.ReadModels;
using System.Text;

namespace Hospital.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly AppointmentService _appointmentService;

        public StatisticsController(AppointmentService appointmentService) 
        {
            _appointmentService = appointmentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LinearRegressionForYear()
        {            

            List<Data> dataForChart = new List<Data>
            {
                new Data(_appointmentService.GetAll().Where(x => x.AppointmentDate.Month == 1).ToList().Count),
                new Data(_appointmentService.GetAll().Where(x => x.AppointmentDate.Month == 2).ToList().Count),
                new Data(_appointmentService.GetAll().Where(x => x.AppointmentDate.Month == 3).ToList().Count),
                new Data(_appointmentService.GetAll().Where(x => x.AppointmentDate.Month == 4).ToList().Count),
                new Data(_appointmentService.GetAll().Where(x => x.AppointmentDate.Month == 5).ToList().Count),
                new Data(_appointmentService.GetAll().Where(x => x.AppointmentDate.Month == 6).ToList().Count),
                new Data(_appointmentService.GetAll().Where(x => x.AppointmentDate.Month == 7).ToList().Count),
                new Data(_appointmentService.GetAll().Where(x => x.AppointmentDate.Month == 8).ToList().Count),
                new Data(_appointmentService.GetAll().Where(x => x.AppointmentDate.Month == 9).ToList().Count),
                new Data(_appointmentService.GetAll().Where(x => x.AppointmentDate.Month == 10).ToList().Count),
                new Data(_appointmentService.GetAll().Where(x => x.AppointmentDate.Month == 11).ToList().Count),
                new Data(_appointmentService.GetAll().Where(x => x.AppointmentDate.Month == 12).ToList().Count)
            };

            var rm = new HospitalDataReadModel(

                chart: new Chart(
                    caption: "Patient statistics",
                    subCaption: "Based on data collected last year",
                    numberPrefix: "",
                    theme: "fusion",
                    radarfillcolor: "#ffffff",
                    xAxisName: "Month",
                    yAxisNamevenues: "Volume of appointments"),

                categories: new List<Category>
                {
                    new Category(
                        new List<BLL.ReadModels.Label>
                        {
                            new Label("Jan"),
                            new Label("Feb"),
                            new Label("Mar"),
                            new Label("Apr"),
                            new Label("May"),
                            new Label("Jun"),
                            new Label("Jul"),
                            new Label("Jug"),
                            new Label("Sep"),
                            new Label("Oct"),
                            new Label("Nov"),
                            new Label("Dec")
                        }),
                },

                dataset: new List<BLL.ReadModels.DataSet>
                {
                    new BLL.ReadModels.DataSet(
                        seriesname: "Patient average",
                        dataForChart
                        )
                });

            return Ok(rm);
        }

        
        //public IActionResult Month()
        //{

        //    var firstWeek = _appointmentService.GetAll().Where(x => x.AppointmentDate.AddMonths(-0).Day > 0 && x.AppointmentDate.AddMonths(-0).Day < 8).ToList().Count;
        //    var secondWeek = _appointmentService.GetAll().Where(x => x.AppointmentDate.AddMonths(-0).Day > 7 && x.AppointmentDate.AddMonths(-0).Day < 15).ToList().Count;
        //    var thirdWeek = _appointmentService.GetAll().Where(x => x.AppointmentDate.AddMonths(-0).Day > 14 && x.AppointmentDate.AddMonths(-0).Day < 22).ToList().Count;
        //    var fourthWeek = _appointmentService.GetAll().Where(x => x.AppointmentDate.AddMonths(-0).Day > 21 && x.AppointmentDate.AddMonths(-0).Day < 29).ToList().Count;
        //    var fifthWeek = _appointmentService.GetAll().Where(x => x.AppointmentDate.AddMonths(-0).Day > 28).ToList().Count;

        //    List<Data> dataForChart = new List<Data>
        //    {
        //        new Data(firstWeek),
        //        new Data(secondWeek),
        //        new Data(thirdWeek),
        //        new Data(fourthWeek),
        //        new Data(fifthWeek)
        //    };

        //    var rm = new HospitalDataReadModel(

        //        chart: new Chart(
        //            caption: "Patient statistics",
        //            subCaption: "Based on data collected last Month",
        //            numberPrefix: "",
        //            theme: "fusion",
        //            radarfillcolor: "#ffffff",
        //            xAxisName: "Month",
        //            yAxisNamevenues: "Volume of appointments for weeks"),

        //        categories: new List<Category>
        //        {
        //            new Category(
        //                new List<BLL.ReadModels.Label>
        //                {
        //                    new Label("First Week"),
        //                    new Label("Second Week"),
        //                    new Label("Third Week"),
        //                    new Label("Fourth Week"),
        //                    new Label("Fifth Week"),
        //                }),
        //        },

        //        dataset: new List<BLL.ReadModels.DataSet>
        //        {
        //            new BLL.ReadModels.DataSet(
        //                seriesname: "Patient average",
        //                dataForChart
        //                )
        //        });

        //    return Ok(rm);
        //}
    }
}
