using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OOProjectBasedLeaning
{

    public class GuestPanel : DragDropPanel
    {

        private Guest guest;

        public GuestPanel(Guest guest)
        {

            this.guest = guest;

            (this.guest as AbstractGuest).OnCheckOutCanceled = RestorePanelToHotel; //GuestからHotelFormに戻す処理ができるように設定


            InitializeComponent();

        }

        private void InitializeComponent()
        {

            Label guestNameLabel = new Label
            {
                Text = guest.Name,
                AutoSize = true,
                Location = new Point(10, 40)
            };

            TextBox guestNameTextBox = new TextBox
            {
                Text = guest.Name,
                Location = new Point(120, 36),
                Width = 160
            };

            Label gusetStatusLabel = new GuestStatusLabel(guest) //StatusLabelの追加　//宿泊中の場合、値段を表示
            {
                Location = new Point(10, 10),
            };


            Controls.Add(guestNameLabel);
            Controls.Add(guestNameTextBox);
            Controls.Add(gusetStatusLabel); //StatusLabelを追加
        }

        protected override void OnPanelMouseDown()
        {
            DoDragDropMove();

            //if(GetForm() is HomeForm)
            //{
            //    guest.Name = "チェックアウト";
            //} 
            //else
            //{
            //    guest.Name = "Drop at " + DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");
            //}

            try
            {

                if (GetForm() is HotelForm) //ドロップされたフォームがHotelか確認
                {

                    guest.GoTo((GetForm() as HotelForm).Hotel);

                }
                else if (GetForm() is HomeForm)//ドロップされたフォームがHomeか確認
                {

                    guest.GoTo((GetForm() as HomeForm).Home);

                }

            }
            catch (IsNotVacanciesException ex)
            {

                // TODO: Handle the exception when there are no vacancies in the hotel or home.
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (OnlyMembersCanStayInSuiteRoomsException ex)
            {

                // TODO: Handle the exception when a guest who is not a member tries to stay in a suite room.
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        private void RestorePanelToHotel() //Hotelの指定した位置にパネルを戻す
        {
            var hotelForm = Application.OpenForms.OfType<HotelForm>().FirstOrDefault();
            if (hotelForm != null)
            {
                this.Parent = hotelForm;
                this.Location = new Point(30, 30); // 任意の戻したい位置
            }
        }


    }

}
