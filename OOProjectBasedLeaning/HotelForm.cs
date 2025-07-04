using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOProjectBasedLeaning
{

    public partial class HotelForm:DragDropForm
    {

        private Hotel hotel;

        public HotelForm()
        {

            InitializeComponent();

            hotel = new HotelModel("MyHotel");

            Controls.Add(new HotelVacanciesLabel(hotel)//部屋の状態
            {

                Dock = DockStyle.Top,
                Font = new Font("Arial", 16, FontStyle.Bold),

            });
        }

        protected override void OnFormDragEnterSerializable(DragEventArgs dragEventArgs)
        {

            dragEventArgs.Effect = DragDropEffects.Move;

        }

        protected override void OnFormDragDropSerializable(object? serializableObject, DragEventArgs dragEventArgs)
        {

            if (serializableObject is DragDropPanel)
            {

                GuestPanel guestPanel = serializableObject as GuestPanel;
                guestPanel.AddDragDropForm(this, PointToClient(new Point(dragEventArgs.X, dragEventArgs.Y)));

            }

        }

        public Hotel Hotel => hotel;

    }

}
