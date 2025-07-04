using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOProjectBasedLeaning
{
    public class HotelVacanciesLabel : Label, Observer
    {

        private Hotel hotel;

        public HotelVacanciesLabel(Hotel hotel)
        {

            if (hotel is NotifierModel)
            {

                (hotel as NotifierModel).AddObserver(this);

            }

            this.hotel = hotel;

            Update(this);

        }

        public void Update(object sender)
        {

            if (hotel.IsVacancies())
            {

                Text = "空室あり";
                ForeColor = Color.Green;

            }
            else
            {

                Text = "満室";
                ForeColor = Color.Red;

            }

        }

    }
}
