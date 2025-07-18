using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OOProjectBasedLeaning
{
    public interface Hotel : Place
    {
        void CheckIn(Guest guest);
        void CheckIn(List<Guest> guests);
        void CheckOut(Guest guest);
        void CheckOut(List<Guest> guests);
        bool IsVacancies();
    }

    public class HotelModel : NotifierModelEntity, Hotel
    {
        private List<Room> vacantRooms;
        private List<Room> guestBook = new List<Room>();

        public HotelModel() : this(string.Empty)
        {
            InitializeComponent();
        }

        public HotelModel(string name)
        {
            Name = name;
            InitializeComponent();
        }

        private void InitializeComponent() //会員限定部屋を作成
        {
            vacantRooms = new List<Room>
            {
                // 8F
                new RegularRoom(801, 18000, this), new RegularRoom(802, 18000, this),//会員ゲストが使用可能
                // 10F
                new SuiteRoom(1001, 360000, this), new SuiteRoom(1002, 300000, this) //会員限定
            };
        }

        private Room AcquireRoom()
        {
            Room room = vacantRooms.First();
            vacantRooms.Remove(room);
            return room;
        }

        private void ReleaseRoom(Room room)
        {
            vacantRooms.Add(room);
        }

        public void CheckIn(Guest guest) //チェックイン処理
        {
            if (guest.StayAt() is not NullObject)
            {
                throw new AlreadyCheckedInException(guest); // ← 例外を出す
            }

            if (IsVacancies()) //空き室確認
            {
                Room room = AcquireRoom(); //空き室を1つ取得
                try
                {
                    guestBook.Add(room.AddGuest(guest)); //ゲストを追加
                    Notify();
                }
                catch (OnlyMembersCanStayInSuiteRoomsException ex)
                {
                    ReleaseRoom(room);
                    MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                throw new IsNotVacanciesException();
            }
        }

        public void CheckIn(List<Guest> guests) //チェックイン処理(複数)
        {
            if (guests.Any(g => g.StayAt() is not NullObject))
            {
                throw new AlreadyCheckedInException(guests); // ← 複数人の例外を出す
            }

            if (IsVacancies())
            {
                Room room = AcquireRoom();
                try
                {
                    guestBook.Add(room.AddGuests(guests)); //複数のゲストを追加
                    Notify();
                }
                catch (OnlyMembersCanStayInSuiteRoomsException ex)
                {
                    ReleaseRoom(room);
                    MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                throw new IsNotVacanciesException();
            }
        }

        public void CheckOut(Guest guest) //チェックアウト処理
        {
            Room room = guest.StayAt(); //滞在部屋を確認
            if (room.CheckOutGuest(guest)) //部屋からゲストを削除後、部屋の確認
            {
                guestBook.Remove(room); //部屋に誰もいない場合、宿泊記録を削除
                ReleaseRoom(room); //部屋を空き部屋にもどす
            }
            Notify();
        }

        public void CheckOut(List<Guest> guests) //チェックアウト処理(複数)
        {
            guests.ForEach(guest => CheckOut(guest)); //順番にCheckOut処理を行う
        }

        public bool IsVacancies()
        {
            return vacantRooms.Count > 0;
        }
    }

    public class NullHotel : Hotel
    {
        private static readonly Hotel instance = new NullHotel();
        private NullHotel() { }
        public static Hotel Instance => instance;

        public void CheckIn(Guest guest)
        {
        }

        public void CheckIn(List<Guest> guests)
        {
        }

        public void CheckOut(Guest guest)
        {
        }

        public void CheckOut(List<Guest> guests)
        {
        }

        public bool IsVacancies()
        {
            return false;
        }
    }
}

