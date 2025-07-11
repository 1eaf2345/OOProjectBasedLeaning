using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOProjectBasedLeaning
{
    public class GuestStatusLabel : Label, Observer
    {
        private Guest guest = NullGuest.Instance;

        public GuestStatusLabel()
        {

            InitializeComponent();

        }

        public GuestStatusLabel(Guest guest)
        {

            if (guest is NotifierModelEntity)
            {

                (guest as NotifierModelEntity).AddObserver(this);

            }

            this.guest = guest;

            InitializeComponent();

        }

        private void InitializeComponent()
        {

            this.AutoSize = true;
            this.Font = new Font("Arial", 10, FontStyle.Regular);

            Update(this);

        }

        //public void Update(object sender) 
        //{

        //    if (guest.IsAtCheckIN())
        //    {

        //        Text = "帰宅中";
        //        ForeColor = Color.Red;

        //    }
        //    else if (guest.IsAtCheckOUT())
        //    {

        //        Text = "宿泊中";
        //        ForeColor = Color.Green;

        //    }
        //    else
        //    {

        //        Text = "－－－";
        //        ForeColor = Color.Gray;

        //    }

        //}

        public void Update(object sender) //ゲストの状態
        {

            var room = guest.StayAt();

            if (room is NullObject)
            {

                Text = "帰宅中";
                ForeColor = Color.Orange;

            }
            else
            {

                Text = $"宿泊中:{room.Price:N0}円";
                ForeColor = Color.Green;

            }

        }

    }

    public class GuestNameLabel : Label, Observer
    {

        private Guest guest = NullGuest.Instance;

        public GuestNameLabel(Guest guest)
        {

            if (guest is NotifierModelEntity)
            {

                (guest as NotifierModelEntity).AddObserver(this);

            }

            this.guest = guest;

            InitializeComponent();

        }

        private void InitializeComponent()
        {

            AutoSize = true;

            Update(this);

        }

        public void Update(object sender)
        {

            Text = guest.Name;

        }

    }

}

