using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fyxme.Models
{
    public class CarMMY
    {
        public int CarMMYId { get; set; }
        public int CarYear { get; set; }
        public string CarModel { get; set; }
        public string CarMaker { get; set; }
        public string ManufacturerCode { get; set; }
        public string ManufacturerName { get; set; }
        public int Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    /*public class CarsMMY
    {
        public List<CarMMY> carsMMY = new List<CarMMY>();

        public void AddCarMMY(CarMMY carMMY)
        {
            carsMMY.Add(carMMY);
        }
    }*/
}